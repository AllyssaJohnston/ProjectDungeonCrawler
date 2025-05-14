using UnityEngine;
using System.Collections;


public class ExitManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(waitToExit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator waitToExit()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
