using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void LoadScene() 
    {
        GameManagerBehavior.leaveMenu();
    }
}
