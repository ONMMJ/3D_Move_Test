using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] ItemSlotUI[] slots;

    [Header("Context")]
    [SerializeField] RectTransform contextPanel;
    [SerializeField] TMP_Text contextText;

    private void Awake()
    {
        Instance = this; 
    }
    private void Start()
    {
        panel.SetActive(false);
        Close();
    }

    public void UpdateItem(Inventory inventory)
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Setup(inventory.items[i]);
    }

    public bool SwitchInventory()
    {
        panel.SetActive(!panel.activeSelf);     // 현재 상태의 반대로 적용한다.
        return panel.activeSelf;                // 현재 상태를 반환한다.
    }

    public void ShowContext(string context, Vector3 position)
    {
        // 텍스트 대입.
        contextText.text = context;

        // 패널의 크기 및 위치 조정.
        contextPanel.gameObject.SetActive(true);
        contextPanel.position = position;
        contextPanel.sizeDelta = new Vector2(contextText.preferredWidth, contextPanel.sizeDelta.y);
    }
    public void Close()
    {
        contextPanel.gameObject.SetActive(false);
    }
}
