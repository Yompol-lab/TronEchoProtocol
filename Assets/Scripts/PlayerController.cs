using System.Windows.Input;
using UnityEngine;
using UnityEngine.Playables;
[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerRB : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    public bool IsGrounded { get; private set; } = true;

    private PlayerState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ChangeState(new WalkingState());
    }

    void Update()
    {
        currentState.Update(this);

        if (Input.GetKeyDown(KeyCode.F))
            CambiarAMoto();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
        currentState.Enter(this);
    }

    public void HandleMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ICommand move = new MoveCommand(moveInput);
        move.Execute(this);
    }

    public void Move(Vector2 input)
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 move = transform.forward * input.y + transform.right * input.x;
        Vector3 newVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);
        rb.linearVelocity = newVelocity;
    }

    public void PerformJump()
    {
        if (!IsGrounded) return;
        ICommand jump = new JumpCommand();
        jump.Execute(this);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        IsGrounded = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.contacts.Length > 0 && Vector3.Dot(other.contacts[0].normal, Vector3.up) > 0.5f)
            IsGrounded = true;
    }

    void CambiarAMoto()
    {
        GameObject moto = ObjectPool.Instance.GetMoto();
        moto.transform.position = transform.position + transform.forward * 2f;
        GameManager.Instance.NotifyVehicleChange(true);
        gameObject.SetActive(false);
    }
}
