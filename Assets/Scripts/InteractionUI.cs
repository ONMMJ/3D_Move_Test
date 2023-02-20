using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI instance { get; private set; }

    // 상호작용 가능한 오브젝트와 만났을 때 내용을 출력.
    [SerializeField] Image blindImage;
    [SerializeField] Text hotkeyText;
    [SerializeField] Text contentText;

    RectTransform contentRect;
    Transform target;

    private void Awake()
    {
        instance = this; 
    }
    private void Start()
    {
        contentRect = contentText.GetComponent<RectTransform>();
        SwitchPanel(false);
    }
    private void Update()
    {
        // 타겟이 있다면 해당 타겟의 월드 좌표를 스크린 좌표로 변환시켜 대입.
        if (target != null)
            transform.position = Camera.main.WorldToScreenPoint(target.position);
    }

    public void OpenPanel(string context, Transform target)
    {

        // 매개변수 문자열을 텍스트에 대입.
        // 해당 문자열의 너비만큼 사이즈 조정.
        contentText.text = context;
        contentRect.sizeDelta = new Vector2(contentText.preferredWidth, contentRect.sizeDelta.y);

        this.target = target;

        SwitchPanel(true);
    }
    public void ClosePanel()
    {
        SwitchPanel(false);
    }

    private void SwitchPanel(bool isOn)
    {
        blindImage.enabled = isOn;
        hotkeyText.enabled = isOn;
        contentText.enabled = isOn;
    }
}
