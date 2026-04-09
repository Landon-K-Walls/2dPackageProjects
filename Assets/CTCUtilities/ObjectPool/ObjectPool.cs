using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCUtil;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] [Range(1, 1000)] int PoolSize;

    [SerializeField] GameObject _templateObject;

    [SerializeField] Queue<PoolableObject> objects = new Queue<PoolableObject>();

    private void Awake()
    {
        if(_templateObject != null)
        {
            for(int i = 0; i < PoolSize; i++)
            {
                GameObject O = Instantiate(_templateObject);
                O.transform.parent = gameObject.transform;
                O.transform.position = Vector3.zero;
                Component C = O.AddComponent(typeof(PoolableObject));
                PoolableObject P = (PoolableObject) C;
                objects.Enqueue(P);
                P.SetPool(this);
                P.gameObject.SetActive(false);
            }
        }

        _templateObject.SetActive(false);
    }

    public GameObject SpawnNew(Vector3 position)
    {
        PoolableObject P = objects.Dequeue();
        P.gameObject.SetActive(true);
        P.transform.position = position;
        return P.gameObject;
    }

    public void RecycleObject(PoolableObject P)
    {
        P.transform.position = Vector3.zero;
        P.gameObject.SetActive(false);
        objects.Enqueue(P);
    }

}
