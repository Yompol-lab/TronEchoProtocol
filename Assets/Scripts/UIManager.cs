using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    void OnEnable() => PlayerHealth.OnHealthChange += UpdateHealthBar;
    void OnDisable() => PlayerHealth.OnHealthChange -= UpdateHealthBar;

    void UpdateHealthBar(float value) => healthBar.value = value;
}
