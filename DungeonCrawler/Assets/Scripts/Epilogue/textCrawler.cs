using UnityEngine;

public class textCrawler : MonoBehaviour
{
    public float crawlSpeed = 0.05f;
    public bool scaleOnResolution = true;
    public UnityEngine.Events.UnityEvent onScreenExit;
    bool calledOnScreenExit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float trueCrawlSpeed = scaleOnResolution?
            crawlSpeed * Screen.currentResolution.height
                :
            crawlSpeed;

        this.transform.position += Vector3.up * trueCrawlSpeed * Time.deltaTime;

        bool exitedScreen = true;
        foreach (var child in this.GetComponentsInChildren<RectTransform>())
        {
            if (child.position.y < Screen.currentResolution.height) 
            {
                exitedScreen = false;
                break;
            }
        } 

        if (onScreenExit != null && exitedScreen && !calledOnScreenExit)
        {
            calledOnScreenExit = true;
            onScreenExit.Invoke();
        }
    }
}
