using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image selectedImage;

    Item item;
    RectTransform rectTransform;

    // 오브젝트가 활성활 될 때마다 호출.
    private void OnEnable()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        OnDeselectedSlot();
    }

    public void Setup(Item item)
    {
        // 슬롯의 초기화 함수.
        this.item = item;
        bool isEmpty = (item == null) || (item.id == -1);

        if (isEmpty)
        {
            itemImage.enabled = false;
        }
        else
        {
            itemImage.sprite = item.sprite;
            itemImage.enabled = item.sprite != null;
        }
    }
    public void OnSelectedSlot()
    {
        // 선택 이미지 활성화.
        selectedImage.enabled = true;
        if(item != null)
        {
            Vector3 position = rectTransform.position;
            position.y -= rectTransform.sizeDelta.y / 2f;
            InventoryUI.Instance.ShowContext(item.content, position);
        }
    }
    public void OnDeselectedSlot()
    {
        // 선택 이미지 비활성화.
        if (selectedImage.enabled)
        {
            selectedImage.enabled = false;

            if (item != null)
                InventoryUI.Instance.Close();
        }
    }
}
