using UnityEngine;

public class LightCycleController : MonoBehaviour
{
    [Header("Movimiento")]
    public float maxSpeed = 26.39f;        // 95 km/h
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float turnSpeed = 50f;

    [Header("Referencias")]
    public Transform forwardReference;  // Objeto hijo que apunta hacia Z+
    public Transform visualModel;       // Mesh visual de la moto

    [Header("Estética de inclinación")]
    public float tiltAmount = 15f;      // Cuánto inclina al doblar (visual)
    public float tiltSmooth = 5f;       // Suavidad de la inclinación

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float inputVertical;
    private float inputHorizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputVertical = Input.GetAxis("Vertical");     // W/S
        inputHorizontal = Input.GetAxis("Horizontal"); // A/D
    }

    void FixedUpdate()
    {
        Mover();
        Girar();
        InclinarVisual();
    }

    void Mover()
    {
        if (Mathf.Abs(inputVertical) > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * Mathf.Sign(inputVertical), acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }

        Vector3 dir = forwardReference.forward;
        dir = new Vector3(dir.x, 0f, dir.z).normalized; // Plano XZ

        Vector3 velocity = dir * currentSpeed;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void Girar()
    {
        // Solo girar si hay velocidad
        if (Mathf.Abs(currentSpeed) > 1f && Mathf.Abs(inputHorizontal) > 0.1f)
        {
            float turnFactor = Mathf.Lerp(0.3f, 1f, Mathf.Abs(currentSpeed) / maxSpeed);
            float rotation = inputHorizontal * turnSpeed * turnFactor * Time.fixedDeltaTime;
            Quaternion deltaRot = Quaternion.Euler(0f, rotation, 0f);
            rb.MoveRotation(rb.rotation * deltaRot);
        }
    }

    void InclinarVisual()
    {
        if (visualModel == null) return;

        float targetTilt = -inputHorizontal * tiltAmount;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetTilt);
        visualModel.localRotation = Quaternion.Slerp(visualModel.localRotation, targetRotation, Time.deltaTime * tiltSmooth);
    }
}
