using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<float> OnHealthChange;
    [SerializeField] private float health = 100f;

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        OnHealthChange?.Invoke(health);
    }
}
