using UnityEngine;

public class Exit : MonoBehaviour
{
    int framesWaited = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (framesWaited > 1) 
        {
            Application.Quit();
        }

        framesWaited++;
    }
}
