using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Item은 데이터이기 때문에 씬에 존재할 수 없다.
// 따라서 아이템 데이터를 가지고 있는 오브젝트의 역할을 한다.
public class ItemObject : MonoBehaviour, IInteraction
{
    [SerializeField] Item item;

    public string Name => (item == null) ? "이름 없음" : item.name;

    public void OnInteraction(GameObject order)
    {
        // 요청자가 인벤토리를 들고 있는지 확인한다.
        Inventory inven = order.GetComponent<Inventory>();

        // 들고 있다면 인벤토리에 아이템 대입을 시도하고
        // 대입에 성공했다면 오브젝트를 삭제한다.
        if(inven != null && inven.AddItem(item))
            Destroy(gameObject);
    }

    public void Setup(Item item)
    {
        this.item = item;
    }
}
