using UnityEngine;

public class PlayerControllerRB : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 7f;

    [Header("Moto")]
    public GameObject motoConPersonaje;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //  Congela rotaciones para evitar caídas
        rb.freezeRotation = true;

        if (motoConPersonaje != null)
            motoConPersonaje.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CambiarAMoto();
            return;
        }

        Mover();
        Saltar();
    }

    void Mover()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        Vector3 newVelocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);

        rb.velocity = newVelocity;
    }

    void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset vertical
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // Detecta si tocamos el suelo para permitir saltar
        if (other.contacts.Length > 0)
        {
            ContactPoint contact = other.contacts[0];
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                isGrounded = true;
        }
    }

    void CambiarAMoto()
    {
        if (motoConPersonaje == null) return;
        StartCoroutine(ActivarMotoConDelay());
    }

    System.Collections.IEnumerator ActivarMotoConDelay()
    {
        Rigidbody rb = motoConPersonaje.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Separación para evitar colisiones
        Vector3 offset = transform.forward * 1.5f + Vector3.down * 0.5f;
        motoConPersonaje.transform.position = transform.position + offset;

        // ROTACIÓN FIJA PARA MODELOS QUE MIRAN AL SUELO
        Quaternion fixedRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Quaternion.Euler(-90f, 0f, 0f);
        motoConPersonaje.transform.rotation = fixedRotation;

        motoConPersonaje.SetActive(true);

        yield return null;

        if (rb != null)
            rb.isKinematic = false;

        gameObject.SetActive(false);
    }






}