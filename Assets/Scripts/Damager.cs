using UnityEngine;

public class Damager : MonoBehaviour
{
    [Header("Modo")]
    public bool useTrigger = true;              
    public bool damageOnHit = true;             
    public bool damageOverTime = false;         
    public float damageAmount = 20f;            
    public float damagePerSecond = 5f;          
    public bool oneShot = false;                

    [Header("Opcional")]
    public string targetTag = "Player";         
    public bool destroyOnOneShot = false;       

    void Reset()
    {
        
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
        useTrigger = true;
    }

    
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
