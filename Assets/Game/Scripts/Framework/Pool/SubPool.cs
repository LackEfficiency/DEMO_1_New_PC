using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SubPool
{
    Transform m_parent;

    //预设
    GameObject m_prefab;

    //集合
    List<GameObject> m_objects = new List<GameObject>();

    //名字标识
    public string Name
    { get { return m_prefab.name; } }

    //构造
    public SubPool(Transform parent, GameObject prefab)
    {
        this.m_parent = parent;
        this.m_prefab = prefab;
    }

    //取对象 
    public GameObject Spawn()
    {
        GameObject go = null;

        foreach (GameObject obj in m_objects)
        {
            if (!obj.activeSelf)
            {
                go = obj;
                break;
            }
        }

        if (go == null)
        {
            go = GameObject.Instantiate<GameObject>(m_prefab);
            go.transform.parent = m_parent;
            m_objects.Add(go);
        }

        //如果不需要淡入 直接显示 否则不显示等待淡入
        // 尝试获取 FadeController 组件
        try
        {
            FadeController fadeController = go.GetComponent<FadeController>();
            if (fadeController != null)
            {
                // 如果 FadeController 存在，执行渐进效果
                fadeController.StartCoroutine(fadeController.FadeIn(go));
            }
            else
            {
                // 如果 FadeController 不存在，直接激活对象
                go.SetActive(true);
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"Failed to get FadeController component: {e.Message}");
            // 处理异常，直接激活对象
            go.SetActive(true);
        }

        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }

    //回收对象
    public void Unspawn(GameObject go)
    {
        if (Contains(go))
        {
            go.SendMessage("OnUnspawn", SendMessageOptions.DontRequireReceiver);

            // 如果不需要淡出 直接隐藏 否则不显示等待淡出
            // 尝试获取 FadeController 组件
            try
            {
                FadeController fadeController = go.GetComponent<FadeController>();
                if (fadeController != null)
                {
                    // 如果 FadeController 存在，执行渐进效果
                    fadeController.StartCoroutine(fadeController.FadeOut(go));
                }
                else
                {
                    // 如果 FadeController 不存在，直接隐藏对象
                    go.SetActive(false);
                }
            }
            catch (NullReferenceException e)
            {
                Debug.LogError($"Failed to get FadeController component: {e.Message}");
                // 处理异常，直接隐藏
                go.SetActive(false);
            }
        }
    }

    //回收所有对象
    public void UnSpawnAll()
    {
        foreach (GameObject obj in m_objects)
        {
            if (obj.activeSelf)
            {
                Unspawn(obj);
            }
        }
    }

    //销毁所有对象
    
    public void DestroyAll()
    {
        foreach (GameObject go in m_objects)
        {
            GameObject.Destroy(go);
        }
        m_objects.Clear();
    }

    //是否包含对象
    public bool Contains(GameObject go)
    {
        return m_objects.Contains(go);
    }

}
