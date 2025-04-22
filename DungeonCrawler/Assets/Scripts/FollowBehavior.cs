using UnityEngine;

public class FollowBehavior : MonoBehaviour
{
    public GameObject followObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (followObject != null)
        {
            Vector3 position = new Vector3(followObject.transform.position.x, followObject.transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = position;
        }
    }
}
