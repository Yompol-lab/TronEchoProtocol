using UnityEngine;

/// <summary>
/// Coloca este script en un objeto (cubo, trampa, proyectil) para que haga daño
/// a objetos que tengan PlayerHealth (o componente compatible).
/// - Si "useTrigger" = true, usa OnTriggerEnter/Stay.
/// - Si "useTrigger" = false, usa OnCollisionEnter/Stay.
/// - Puedes hacer daño puntual (damageOnHit) o daño por segundo (damagePerSecond).
/// - Si oneShot = true, el damager se desactiva o destruye después de aplicar el daño puntual.
/// </summary>
public class Damager : MonoBehaviour
{
    [Header("Modo")]
    public bool useTrigger = true;              // usar trigger en lugar de colisión física
    public bool damageOnHit = true;             // daño instantáneo al tocar
    public bool damageOverTime = false;         // daño continuo mientras está en contacto
    public float damageAmount = 20f;            // daño instantáneo
    public float damagePerSecond = 5f;          // daño por segundo en contacto
    public bool oneShot = false;                // si es true y aplica dañoOnHit, se desactiva/destruye

    [Header("Opcional")]
    public string targetTag = "Player";         // si lo querés por tag en vez de GetComponent
    public bool destroyOnOneShot = false;       // si oneShot true, destruir en vez de desactivar

    void Reset()
    {
        // Por defecto, si el objeto tiene collider: lo hacemos trigger para pruebas rápidas.
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
        useTrigger = true;
    }

    // Helper: intenta aplicar daño si el objeto tiene PlayerHealth
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
