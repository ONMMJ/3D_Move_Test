using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropTable : MonoBehaviour
{
    [System.Serializable]
    public struct TableElement
    {
        public int id;              // 아이템 코드(ID).
        public float weight;        // 가중치.
    }
    // 내가 어떤 아이템을 드랍할 수 있는가를 정의하고
    // 확률을 통해 특정 아이템을 생성한다.
    [SerializeField] Transform createPosition;
    [SerializeField] TableElement[] elements;

    public void OnCreateItem()
    {
        // 모든 테이블의 가중치를 더해 가중치를 구하고
        // Random.value를 곱해 특정 위치를 잡는다.
        float totalWeight = elements.Select(element => element.weight).Sum();
        float pick = Random.value * totalWeight;

        int itemID = 0;

        // 반복문을 순회하며 값이 어떤 테이블에 포함되는지 계산한다.
        float current = 0f;
        for(int i = 0; i < elements.Length; i++)
        {
            current += elements[i].weight;
            if (pick < current)
            {
                itemID = elements[i].id;
                break;
            }
        }
        // 확률적으로 선택된 ID를 가진 아이템 오브젝트를 DB에게서 가져온다.
        // 이후 생성 위치로 이동시킨다.
        ItemObject itemObject = ItemDB.Instance.GetItemObject(itemID);
        itemObject.transform.position = createPosition.position;
    }

}
