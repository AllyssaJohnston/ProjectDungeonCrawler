using UnityEngine;

public class TrackerScript : MonoBehaviour
{
    public static TrackerScript Instance { get; private set; }

    public bool started;
    public string sceneToLoad;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            started = false;
            sceneToLoad = "Level1";
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
