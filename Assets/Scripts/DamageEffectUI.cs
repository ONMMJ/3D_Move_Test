using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectUI : MonoBehaviour
{
    public static DamageEffectUI Instance { get; private set; }

    [SerializeField] DamageEffect prefab;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(Vector3 worldPosition, float damage)
    {
        DamageEffect newEffect = Instantiate(prefab, transform);
        newEffect.Setup(damage, worldPosition);
    }
}
