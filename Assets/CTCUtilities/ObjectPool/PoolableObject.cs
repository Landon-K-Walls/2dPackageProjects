using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    ObjectPool pool;

    public delegate void recycleSignature();
    public event recycleSignature onRecycle;

    public void SetPool(ObjectPool pool) => this.pool = pool;

    public void Recycle()
    {
        if (onRecycle != null)
            onRecycle.Invoke();

        onRecycle = null;

        pool.RecycleObject(this);
    }
}
