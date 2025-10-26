using UnityEngine;

/// <summary>
/// Coloca este script en un objeto (cubo, trampa, proyectil) para que haga da�o
/// a objetos que tengan PlayerHealth (o componente compatible).
/// - Si "useTrigger" = true, usa OnTriggerEnter/Stay.
/// - Si "useTrigger" = false, usa OnCollisionEnter/Stay.
/// - Puedes hacer da�o puntual (damageOnHit) o da�o por segundo (damagePerSecond).
/// - Si oneShot = true, el damager se desactiva o destruye despu�s de aplicar el da�o puntual.
/// </summary>
public class Damager : MonoBehaviour
{
    [Header("Modo")]
    public bool useTrigger = true;              // usar trigger en lugar de colisi�n f�sica
    public bool damageOnHit = true;             // da�o instant�neo al tocar
    public bool damageOverTime = false;         // da�o continuo mientras est� en contacto
    public float damageAmount = 20f;            // da�o instant�neo
    public float damagePerSecond = 5f;          // da�o por segundo en contacto
    public bool oneShot = false;                // si es true y aplica da�oOnHit, se desactiva/destruye

    [Header("Opcional")]
    public string targetTag = "Player";         // si lo quer�s por tag en vez de GetComponent
    public bool destroyOnOneShot = false;       // si oneShot true, destruir en vez de desactivar

    void Reset()
    {
        // Por defecto, si el objeto tiene collider: lo hacemos trigger para pruebas r�pidas.
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
        useTrigger = true;
    }

    // Helper: intenta aplicar da�o si el objeto tiene PlayerHealth
    void TryApplyDamage(GameObject other, float amount)
    {
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)) return;

        var ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(amount);
            if (damageOnHit && oneShot)
            {
                if (destroyOnOneShot) Destroy(gameObject);
                else gameObject.SetActive(false);
            }
        }
    }

    // --- Trigger variants ---
    void OnTriggerEnter(Collider other)
    {
        if (!useTrigger) return;
        if (damageOnHit) TryApplyDamage(other.gameObject, damageAmount);
    }

    void OnTriggerStay(Collider other)
    {
        if (!useTrigger) return;
        if (damageOverTime)
        {
            float dmg = damagePerSecond * Time.deltaTime;
            TryApplyDamage(other.gameObject, dmg);
        }
    }

    // --- Collision variants ---
    void OnCollisionEnter(Collision collision)
    {
        if (useTrigger) return;
        if (damageOnHit) TryApplyDamage(collision.gameObject, damageAmount);
    }

    void OnCollisionStay(Collision collision)
    {
        if (useTrigger) return;
        if (damageOverTime)
        {
            float dmg = damagePerSecond * Time.deltaTime;
            TryApplyDamage(collision.gameObject, dmg);
        }
    }
}
