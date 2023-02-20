using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] TMP_Text damageText;

    Vector3 worldPosition;

    public void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(worldPosition);
    }

    public void Setup(float damage, Vector3 worldPosition)
    {
        damageText.text = Mathf.RoundToInt(damage).ToString();
        this.worldPosition = worldPosition;
    }

    public void OnEndAnimation()
    {
        Destroy(gameObject);
    }
}
