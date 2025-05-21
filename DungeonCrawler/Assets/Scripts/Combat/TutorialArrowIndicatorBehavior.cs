using UnityEngine;

public class TutorialArrowIndicatorBehavior : ArrowIndicatorBehavior
{
    [SerializeField] float moveDist;
    [SerializeField] float degrees;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float rad = degrees * (Mathf.PI / 180.0f);
        startPos = transform.localPosition;
        movementVect = moveDist * new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    // Update is called once per frame
    void Update()
    {
        Tick();
    }

}
