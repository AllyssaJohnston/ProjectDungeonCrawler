using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void startLevel() 
    {
        GameManagerBehavior.leaveMenu();
    }
}
