using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{ 
    public float moveSpeed = 10;
    public float rotationSpeed = 200;
    public float rotation {get; private set;} = 0f;
    float currentPitch = 0f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentRotationAngle = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        // Possible TODO? Mouselook vs. tank controls?
        //rotation += Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime;
        float mouseXDelta = Input.GetAxis("Mouse X");
        rotation += mouseXDelta * rotationSpeed * Time.deltaTime;

        currentPitch = Mathf.Clamp(currentPitch, -180, 180);
        transform.localEulerAngles = new Vector3(currentPitch, 0f, 0f);

        float strafe = Input.GetAxis("Horizontal");
        float move = Input.GetAxis("Vertical");

        moveInput = Quaternion.AngleAxis(rotation, Vector3.back) * new Vector2(move, -strafe).normalized;

        //moveDirection = Quaternion.AngleAxis(rotation, Vector3.back) * new Vector2(Input.GetAxisRaw("Vertical"), 0f).normalized;

    }
     
    private void FixedUpdate()
    {
        rb.linearVelocity = moveSpeed * Time.fixedDeltaTime * moveInput;

    }


}
