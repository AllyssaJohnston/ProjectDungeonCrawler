using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform whatToFace;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (whatToFace == null) 
        {
            whatToFace = FindFirstObjectByType<FirstPerson>().gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update() {
        if (whatToFace != null) { 
            this.transform.rotation = whatToFace.rotation;
        }
    }
}
