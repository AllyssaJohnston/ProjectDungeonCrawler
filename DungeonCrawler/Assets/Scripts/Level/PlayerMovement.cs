using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    public float moveSpeed = 10;
    public float rotationSpeed = 64f;

    public float rotation {get; private set;} = 0f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Possible TODO? Mouselook vs. tank controls?
        rotation += Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime;

        moveDirection = Quaternion.AngleAxis(rotation, Vector3.back) * new Vector2(Input.GetAxisRaw("Vertical"), 0f).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveSpeed * Time.fixedDeltaTime * moveDirection;
    }



}
