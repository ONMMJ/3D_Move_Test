using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Status : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] float maxHp;

    [Header("Event")]
    [SerializeField] UnityEvent<float> onTakeDamage;    // 피격 시 호출되는 이벤트 함수.
    [SerializeField] UnityEvent onDead;                 // 사망 시 호출되는 이벤트 함수.

    public float Hp => hp;
    public float MaxHp => maxHp;

    public float TakeDamage(float power)
    {
        if (hp <= 0)
            return 0;

        onTakeDamage?.Invoke(power);                // 데미지 이벤트 호출.
        hp = Mathf.Clamp(hp - power, 0, maxHp);     // 체력 감소.
        if (hp <= 0)
        {
            onDead?.Invoke();                       // 사망 이벤트 호출.
        }

        return power;
    }
}
