using System;
using CCUtil.Controllers;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace CCUtil.Projectiles
{
  public class ProjectileHandler : Singleton<ProjectileHandler>
  {
    //Component Options
    [SerializeField] LayerMask mask;
    [SerializeField] bool drawDebug;
    [SerializeField] Mesh tracerMesh;

    [SerializeField] Material material;

    bool pauseProjectilesAdvance = false;
    public static void PauseProjectilesAdvancement(bool p) => instance.pauseProjectilesAdvance = p;

    //Actual instance data containers, updated every frame
    NativeList<ProjectileData> projectiles;
    NativeArray<RaycastCommand> raycastCommands;
    NativeArray<RaycastHit> raycastResults;
    NativeArray<Matrix4x4> matricesArray;
    Matrix4x4[] matrices = new Matrix4x4[1023];

    //job handle for starting the job
    JobHandle jobHandle;

    //physics options based gravity value, set in awake
    Vector3 gravity;

    //Event OnCollision
    public static event Action<ProjectileCollisionData> OnProjectileCollision;

    protected override void Awake()
    {
      base.Awake();
      gravity = Physics.gravity;

      //persistent list of projectiles
      projectiles = new NativeList<ProjectileData>(Allocator.Persistent);
    }

    //Dispose of data. Jobs will not use GC.
    void OnDestroy()
    {
      OnProjectileCollision = null;
      if (projectiles.IsCreated) projectiles.Dispose();
      if (raycastCommands.IsCreated) raycastCommands.Dispose();
      if (raycastResults.IsCreated) raycastResults.Dispose();
    }

    //actual function to make a projectile
    public static void SpawnProjectile(Vector3 positiion, Vector3 velocity, float damage, int tag, float drag = 0.001f, float lifeTime = 15f)
    {
      instance.projectiles.Add(new ProjectileData(positiion, velocity, instance.gravity, lifeTime, damage, drag, tag));
    }

    //Update. The big one.
    //For each projectile that exists, move based on velocity, then update the velocity, then raycast through it's movement vector.
    void Update()
    {
      if (projectiles.Length == 0) return;

      float dt = Time.deltaTime;
      //Get rid of the raycast data from previous frames and create new containers.
      if (raycastCommands.IsCreated) raycastCommands.Dispose();
      if (raycastResults.IsCreated) raycastCommands.Dispose();
      if (matricesArray.IsCreated) matricesArray.Dispose();
      raycastCommands = new NativeArray<RaycastCommand>(projectiles.Length, Allocator.TempJob);
      raycastResults = new NativeArray<RaycastHit>(projectiles.Length, Allocator.TempJob);
      matricesArray = new NativeArray<Matrix4x4>(projectiles.Length, Allocator.TempJob);

      matrices = new Matrix4x4[projectiles.Length];

      //create the job to be performed on other threads.
      AdvanceProjectilesJob job = new AdvanceProjectilesJob
      {
        //AsDefferedJobArry is used to give the job an up to date list since the length of the array isn't known.
        projectiles = projectiles.AsDeferredJobArray(),
        raycastCommands = raycastCommands,
        matricesArray = matricesArray,
        deltaTime = dt,
        mask = mask,
        camPos = Camera.main.transform.position,
        renderOnly = instance.pauseProjectilesAdvance
      };

      //the JobHandle in the RaycastCommand is the job that must be completed before a raycast can take place.
      //Schedule and complete the job.
      jobHandle = job.Schedule(projectiles.Length, 64);
      jobHandle = RaycastCommand.ScheduleBatch(raycastCommands, raycastResults, 64, jobHandle);
      jobHandle.Complete();

      //Debug lines per projectile. 
      if (drawDebug)
        for (int i = 0; i < projectiles.Length; i++)
        {
          Vector3 prevPos = projectiles[i].position - projectiles[i].velocity * Time.deltaTime;
          Debug.DrawLine(prevPos, projectiles[i].position, Color.yellow, 5f);
        }

      //Handle the raycast results
      for (int i = projectiles.Length - 1; i >= 0; i--)
      {
        if (raycastResults[i].collider != null)
        {
          IProjectileReceiver receiver = raycastResults[i].collider.GetComponent<IProjectileReceiver>();
          receiver?.ReceiveProjectile(projectiles[i],
          new ProjectileCollisionData
          {
            point = raycastResults[i].point,
            normal = raycastResults[i].normal
          });

          if (receiver == null)
          {
            OnProjectileCollision?.Invoke(
            new ProjectileCollisionData
            {
              point = raycastResults[i].point,
              normal = raycastResults[i].normal
            }
          );
          }

          projectiles.RemoveAtSwapBack(i);
        }
        else if (projectiles[i].lifeTime <= 0)
        {
          projectiles.RemoveAtSwapBack(i);
        }
      }

      matricesArray.CopyTo(matrices);

      int total = projectiles.Length;
      int batchSize = 1023;
      int drawn = 0;
      Vector3 camPos = Camera.main.transform.position;

      while (drawn < total)
      {
        int count = Mathf.Min(batchSize, total - drawn);

        // Fill one batch of matrices
        for (int i = 0; i < count; i++)
        {
          ProjectileData p = projectiles[drawn + i];
          float distFromCam = (p.position - camPos).magnitude;
          if (distFromCam > 5.5f)
          {
            matrices[i] = Matrix4x4.TRS(
              p.position,
              Quaternion.LookRotation(p.velocity, Vector3.up),
              Vector3.one * 0.005f * (distFromCam / 1.875f) + Vector3.forward * p.velocity.magnitude * 0.01f
          );
          }
          else
          {
            matrices[i] = Matrix4x4.zero;
          }
        }

        // Draw this batch
        Graphics.DrawMeshInstanced(
            tracerMesh,
            0,
            material,
            matrices,
            count
        );

        drawn += count;
      }

      raycastCommands.Dispose();
      raycastResults.Dispose();
      matricesArray.Dispose();
    }
  }


  [BurstCompile]
  public struct AdvanceProjectilesJob : IJobParallelFor
  {
    //Job data
    public NativeArray<ProjectileData> projectiles;
    public NativeArray<RaycastCommand> raycastCommands;
    public NativeArray<Matrix4x4> matricesArray;
    public float deltaTime;
    public LayerMask mask;
    public Vector3 camPos;
    public bool renderOnly;

    public void Execute(int index)
    {
      //extract data from the array of projectiles
      ProjectileData data = projectiles[index];

      //modify the data
      Vector3 start = data.position; //where it's starting from, used in raycast
      Vector3 move = data.velocity * deltaTime; //set the direction it's moving towards
      Vector3 dragForce = -data.drag * data.velocity * data.velocity.magnitude; //calculate the drag force applied to the projectile
      if (!renderOnly)
      {
        data.velocity += (data.gravity * deltaTime) + (dragForce * deltaTime); //apply gravity and drag to the velocity for the next frame
        data.position += move; //apply movement to current frame position
        data.lifeTime -= deltaTime; //reduce the projectile's lifetime 
      }

      //insert the modified data back to the array
      projectiles[index] = data;

      //parameters for the raycast
      QueryParameters para = new QueryParameters
      {
        hitBackfaces = false,
        hitMultipleFaces = false,
        hitTriggers = QueryTriggerInteraction.Ignore,
        layerMask = mask
      };

      //set up the raycast from where it was this frame to where it has moved to this frame
      if (!renderOnly)
        raycastCommands[index] = new RaycastCommand(start, move.normalized, para, move.magnitude * 1.1f);

      float distFromCam = (data.position - camPos).magnitude;
      float cullDistance = 0.15f;

      if (distFromCam > cullDistance && data.velocity.sqrMagnitude > 0.0001f)
      {
        matricesArray[index] = Matrix4x4.TRS(
          data.position,
          Quaternion.LookRotation(data.velocity, Vector3.up),
          Vector3.one
        );
      }
      else
      {
        matricesArray[index] = Matrix4x4.zero;
      }
    }
  }

  //projectile data struct
  public struct ProjectileData
  {
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 gravity;
    public float lifeTime;
    public float damage;
    public float drag;
    public int tag;

    public ProjectileData(Vector3 position, Vector3 velocity, Vector3 gravity, float lifeTime, float damage, float drag, int tag)
    {
      this.position = position;
      this.velocity = velocity;
      this.gravity = gravity;
      this.lifeTime = lifeTime;
      this.damage = damage;
      this.drag = drag;
      this.tag = tag;
    }
  }

  public struct ProjectileCollisionData
  {
    public Vector3 point;
    public Vector3 normal;
  }
}
