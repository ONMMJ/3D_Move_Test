using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public static ItemDB Instance { get; private set; }

    [SerializeField] ItemObject prefab;
    [SerializeField] Item[] items;

    private void Awake()
    {
        Instance = this;
    }

    // 특정 아이디를 가진 아이템을 반환한다.
    public Item GetItem(int id)
    {
        for(int i = 0; i<items.Length; i++)
        {
            if (items[i].id == id)
                return items[i].GetCopy();
        }
        return null;
    }

    public ItemObject GetItemObject(int id)
    {
        ItemObject newItemObject = Instantiate(prefab);
        newItemObject.Setup(GetItem(id));
        return newItemObject;
    }
}
