using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 직렬화: 유니티 내부에서 컴포넌트 필드에 연결할 수 있는 상태로 변환한다.
[System.Serializable]
public class Item
{
    public int id;          // 아이템을 구분하는 고유 번호.
    public Sprite sprite;   // 이미지.
    public string name;     // 이름.
    public string content;  // 내용.
    public int count;       // 개수.

    public Item()
    {
        id = -1;            // 기본 생성자의 id 값은 -1이다.
    }

    public Item GetCopy()
    {
        // 나와 동일한 값을 가지는 새로운 객체를 반환.
        Item copy = new Item();
        copy.id = id;
        copy.sprite = sprite;
        copy.name = name;
        copy.content = content;
        copy.count = 1;
        return copy;
    }
}
