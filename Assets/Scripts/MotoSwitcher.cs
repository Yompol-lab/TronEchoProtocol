using UnityEngine;

public class MotoSwitcher : MonoBehaviour
{
    [Tooltip("Prefab o referencia al jugador original")]
    public GameObject playerPrefab;
    public float exitDistance = 2f;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            CambiarAJugador();
        }
    }

    void CambiarAJugador()
    {
        if (playerPrefab == null)
        {
            Debug.LogWarning("No hay jugador asignado en MotoSwitcher");
            return;
        }

        
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = transform.position + transform.right * exitDistance;
        player.transform.rotation = transform.rotation;

        
        GameManager.Instance.NotifyVehicleChange(false);

            
        gameObject.SetActive(false);
    }
}
