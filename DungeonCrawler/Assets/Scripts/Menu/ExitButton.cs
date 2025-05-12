using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ExitScene()
    {
        GameManagerBehavior.exit();
    }
}
