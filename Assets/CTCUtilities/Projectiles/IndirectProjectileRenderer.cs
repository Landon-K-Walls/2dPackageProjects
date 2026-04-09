using Unity.Collections;
using UnityEngine;

public class IndirectProjectileRenderer : MonoBehaviour
{
  [SerializeField] Mesh tracerMesh;
  [SerializeField] Material material;

  ComputeBuffer matrixBuffer;   // Holds all projectile transforms
  ComputeBuffer argsBuffer;     // Holds indirect draw arguments

  const int ARGS_SIZE = 5; // Unity requires [index count per inst, instance count, start index, base vertex, start instance]

  Matrix4x4[] matricesManaged;
  int lastCount;

  void InitBuffers(int projectileCount)
  {
    // Release old buffers if they exist
    ReleaseBuffers();

    // Each matrix is 64 bytes (16 floats)
    matrixBuffer = new ComputeBuffer(projectileCount, 64, ComputeBufferType.Structured);

    // Args buffer always length 1x5
    argsBuffer = new ComputeBuffer(1, ARGS_SIZE * sizeof(uint), ComputeBufferType.IndirectArguments);

    // Managed cache for copying from NativeArray
    matricesManaged = new Matrix4x4[projectileCount];
    lastCount = projectileCount;

    // Initialize args
    uint[] args = new uint[ARGS_SIZE];
    args[0] = (tracerMesh != null) ? tracerMesh.GetIndexCount(0) : 0; // index count
    args[1] = (uint)projectileCount;                                  // instance count
    args[2] = (tracerMesh != null) ? tracerMesh.GetIndexStart(0) : 0;
    args[3] = (tracerMesh != null) ? tracerMesh.GetBaseVertex(0) : 0;
    args[4] = 0; // start instance
    argsBuffer.SetData(args);
  }

  public void Render(NativeArray<Matrix4x4> matricesNA)
  {
    int projectileCount = matricesNA.Length;
    if (projectileCount == 0) return;

    // Reallocate if needed
    if (matrixBuffer == null || projectileCount != lastCount)
    {
      InitBuffers(projectileCount);
    }

    // Copy matrices into managed array → GPU buffer
    matricesNA.CopyTo(matricesManaged);
    matrixBuffer.SetData(matricesManaged);

    // Update args count
    uint[] args = new uint[ARGS_SIZE];
    argsBuffer.GetData(args);
    args[1] = (uint)projectileCount;
    argsBuffer.SetData(args);

    // Bind buffer to material (shader must use UNITY_ACCESS_INSTANCED_PROP for this)
    material.SetBuffer("_Matrices", matrixBuffer);

    // Single indirect draw call
    Graphics.DrawMeshInstancedIndirect(
        tracerMesh,
        0,
        material,
        new Bounds(Vector3.zero, new Vector3(1000, 1000, 1000)), // expand to cover world
        argsBuffer
    );
  }

  void ReleaseBuffers()
  {
    matrixBuffer?.Release();
    argsBuffer?.Release();
    matrixBuffer = null;
    argsBuffer = null;
  }

  void OnDestroy()
  {
    ReleaseBuffers();
  }
}