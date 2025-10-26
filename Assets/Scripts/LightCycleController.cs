using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LightCycleController : MonoBehaviour
{
    [Header("Movimiento")]
    public float maxSpeed = 26.39f;    
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float turnSpeed = 50f;

    [Header("Referencias")]
    public Transform forwardReference; 
    public Transform visualModel;     

    [Header("Estética de inclinación")]
    public float tiltAmount = 15f;
    public float tiltSmooth = 5f;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float inputVertical;
    private float inputHorizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        
        if (forwardReference == null)
            forwardReference = transform;

       
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        
        rb.linearDamping = 0.5f;
        rb.angularDamping = 2f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        inputVertical = Input.GetAxis("Vertical");   
        inputHorizontal = Input.GetAxis("Horizontal"); 
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
        dir.y = 0f;
        dir.Normalize();

       
        Vector3 move = dir * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

   
    void Girar()
    {
        if (Mathf.Abs(currentSpeed) > 1f && Mathf.Abs(inputHorizontal) > 0.1f)
        {
            float rotation = inputHorizontal * turnSpeed * Time.fixedDeltaTime;
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
