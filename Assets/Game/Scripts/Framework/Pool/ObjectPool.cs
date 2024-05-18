using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public string ResourceDir = "";

    Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();

    //取对象
    public GameObject Spawn(string name, string resourceDir = null)
    {
        if (!m_pools.ContainsKey(name))
        {
            RegisterNew(name, resourceDir);
        }
        SubPool pool = m_pools[name];
        return pool.Spawn();
    }
    //回收对象
    public void Unspawn(GameObject go)
    {
        SubPool pool = null;
        foreach (SubPool p in m_pools.Values)
        {
            if (p.Contains(go))
            {
                pool = p;
                break;
            }
        }
        pool.Unspawn(go);
    }

    //回收所有对象
    public void UnspawnAll()
    {
        foreach (SubPool p in m_pools.Values)
        {
            p.UnSpawnAll();
        }
    }

    //创建新子池子
    private void RegisterNew(string name, string resourceDir = null)
    {
        //预设的路径
        string path = "";
        ResourceDir = resourceDir;
        if (string.IsNullOrEmpty(ResourceDir.Trim()))
        {
            path = name;
        }
        else
        {
            path = ResourceDir + "/" + name;
        }
        //加载预设
        GameObject prefab = Resources.Load<GameObject>(path);

        //创建子对象池
        SubPool pool = new SubPool(transform, prefab);
        m_pools.Add(pool.Name, pool);
    }

    public void ClearAll() //特殊情况调用
    {
        m_pools = new Dictionary<string, SubPool>();
    }

}