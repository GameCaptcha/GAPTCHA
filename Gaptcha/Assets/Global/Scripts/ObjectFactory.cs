using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory<T> : MonoBehaviour where T: MonoBehaviour
{
    [SerializeField] int poolingCount = 10;
    [SerializeField] T prefabObject;

    List<T> poolingList = new List<T>();
    List<T> usingList = new List<T>();


    private void Awake()
    {
        AppendPoolingList();
    }

    public T UseObject()
    {
        if (poolingList.Count <= 0)
        {
            AppendPoolingList();
        }

        T obj = poolingList[0];
        poolingList.RemoveAt(0);
        usingList.Add(obj);
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void Restore(T obj)
    {
        obj.transform.parent = transform;
        obj.gameObject.SetActive(false);
        usingList.Remove(obj);
        poolingList.Add(obj);
    }

    void AppendPoolingList()
    {
        for(int i = 0; i < poolingCount; ++i)
        {
            T obj = Instantiate(prefabObject, transform);
            obj.gameObject.SetActive(false);
            poolingList.Add(obj);
        }
    }

    public void Refresh()
    {
        for (int i = usingList.Count; i > 0; i--)
        {
            Restore(usingList[i - 1]);
        }
        usingList.Clear();
    }
}
