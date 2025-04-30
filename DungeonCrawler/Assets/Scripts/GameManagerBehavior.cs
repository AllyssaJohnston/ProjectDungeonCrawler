using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    public static GameManagerBehavior instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("quit");
            Application.Quit();
        }
    }
}
