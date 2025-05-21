using UnityEngine;

public class ArrowIndicatorBehavior : MonoBehaviour
{
    protected bool move = true;
    protected Vector3 startPos;
    protected Vector3 movementVect;
    protected float bufferTimer = 0f;
    [SerializeField] protected float movementTime;

    public void setUp(float rot, float moveDist)
    {
        startPos = transform.localPosition;
        float rad = (rot * Mathf.PI) / 180f;
        movementVect = moveDist * new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
        move = (moveDist != 0f);
    }

    private void Update()
    {
        Tick();
    }

    virtual protected void Tick() 
    {
        if (move)
        {
            buffer();
        }
    }

    public void UpdateMove(bool move)
    {
        bufferTimer = 0f;
        transform.localPosition = startPos;
        this.move = move;
    }

    private void FixedUpdate()
    {
        if (move)
        {
            transform.localPosition += movementVect * Time.fixedDeltaTime;
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
