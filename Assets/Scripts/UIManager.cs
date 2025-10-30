using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;

    void OnEnable() => PlayerHealth.OnHealthChange += UpdateHealthText;
    void OnDisable() => PlayerHealth.OnHealthChange -= UpdateHealthText;

    void UpdateHealthText(float value)
    {
        
        if (value < 0f) value = 0f;

       
        healthText.text = "Vida: " + value.ToString("0");
    }
}
