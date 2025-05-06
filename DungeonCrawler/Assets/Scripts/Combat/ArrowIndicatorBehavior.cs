using UnityEngine;

public class ArrowIndicatorBehavior : MonoBehaviour
{
    bool move = true;
    Vector3 startPos;
    Vector3 movementVect;
    float bufferTimer = 0f;
    [SerializeField] float movementTime;

    public void setUp(float rot, bool move)
    {
        startPos = transform.localPosition;
        float rad = (rot * Mathf.PI) / 180f;
        movementVect = 60 * new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
        this.move = move;
    }

    private void Update()
    {
        if (move)
        {
            buffer();
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            transform.localPosition += movementVect * Time.fixedDeltaTime; ;
        }
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
