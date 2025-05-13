using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{ 
    public float moveSpeed;
    public float rotationSpeed;
    public float rotation {get; private set;} = 0f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rotationSpeed = GameManagerBehavior.sensSlider * 60f;
    }

    // Update is called once per frame
    void Update()
    { 
        if (GameManagerBehavior.modernControls)
        {
            float mouseXDelta = Input.GetAxis("Mouse X");
            rotation += mouseXDelta * rotationSpeed * Time.deltaTime;

            float strafe = Input.GetAxis("Horizontal");
            float move = Input.GetAxis("Vertical");

            moveDirection = Quaternion.AngleAxis(rotation, Vector3.back) * new Vector2(move, -strafe).normalized;

        } else { 
            rotation += Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime;
            
            moveDirection = Quaternion.AngleAxis(rotation, Vector3.back) * new Vector2(Input.GetAxisRaw("Vertical"), 0f).normalized;
    
        }
    }
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rotationSpeed = GameManagerBehavior.sensSlider * 60f;
    }

    private void FixedUpdate()
    {
        rotationSpeed = GameManagerBehavior.sensSlider * 60f;
        rb.linearVelocity = moveSpeed * Time.fixedDeltaTime * moveDirection;
    }

}
