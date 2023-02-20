using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 아이템의 개수에 제한이 있다.
    public Item[] items { get; private set; }

    private void Start()
    {
        items = new Item[16];
    }


    public bool AddItem(Item item)
    {
        // 현재는 비어있는 칸에 아이템을 대입한다.
        // true: 아이템을 성공적으로 획득한다.
        // false: 인벤토리가 꽉 차 있어 획득하지 못했다.
        for(int i = 0; i < items.Length; i++)
        {
            // items의 내부 초기값은 null이 맞지만
            // 직렬화 속성을 통해 인스펙터에 노출상태가 되면 null이 아니게된다.
            // 따라서 값이 없음을 표현할때는 id를 -1로 표기한다.
            if(items[i] == null || items[i].id == -1)
            {
                items[i] = item;
                return true;
            }
        }
        return false;
    }
}
