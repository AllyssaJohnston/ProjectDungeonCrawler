using UnityEngine;

public class ArrowIndicatorBehavior : MonoBehaviour
{
    Vector3 startPos;
    Vector3 movementVect;
    float bufferTimer = 0f;
    [SerializeField] float movementTime;

    public void setUp(float rot)
    {
        startPos = transform.localPosition;
        float rad = (rot * Mathf.PI) / 180f;
        movementVect = 60 * new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    private void Update()
    {
        buffer();
    }

    private void FixedUpdate()
    {
        transform.localPosition += movementVect * Time.fixedDeltaTime;
    }

    private void buffer()
    {
        bufferTimer += Time.deltaTime;
        if (bufferTimer > movementTime)
        {
            bufferTimer = 0f;
            transform.localPosition = startPos;
        }
    }
}
