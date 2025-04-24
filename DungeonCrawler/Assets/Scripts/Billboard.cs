using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform whatToFace;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = whatToFace.rotation;
    }
}
