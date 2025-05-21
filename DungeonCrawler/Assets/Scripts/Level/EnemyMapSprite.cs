using UnityEngine;

public class EnemyMapSprite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Why the hell do I have to go to the grandparent? that's not the hierarchy here, Unity.
        this.GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponentInParent<SpriteRenderer>().sprite;

        GameObject root = GameObject.Find("EnemyMapSprites");
        if (root == null) {
            root = new GameObject();
            root.name = "EnemyMapSprites";
        }
        this.transform.SetParent(root.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
