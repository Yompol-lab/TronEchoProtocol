using UnityEngine;

public class LightCycleController : MonoBehaviour
{
    public float maxSpeed = 26.39f;
    public float acceleration = 50f;
    public float turnSpeed = 90f;

    public Transform forwardReference; // ← asigná el objeto hijo aquí

    private float currentSpeed = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Mover();
    }

    void Mover()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (vertical != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * vertical, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.fixedDeltaTime);
        }

        // Movimiento usando el transform de referencia
        Vector3 direction = forwardReference.forward;
        Vector3 movement = direction.normalized * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Rotar la moto (solo sobre eje Y)
        if (horizontal != 0)
        {
            float rotation = horizontal * turnSpeed * Time.fixedDeltaTime;
            Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
            rb.MoveRotation(rb.rotation * turn);
        }
    }
}
