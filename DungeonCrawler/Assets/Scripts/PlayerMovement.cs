using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    public float moveSpeed = 10;
    private Rigidbody2D rb;
    private Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveSpeed * Time.fixedDeltaTime * direction;
        Debug.Log(rb.linearVelocity.ToString());
    }



}
