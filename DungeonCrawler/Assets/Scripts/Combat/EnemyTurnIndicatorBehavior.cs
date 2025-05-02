using UnityEngine;

public class EnemyTurnIndicatorBehavior : MonoBehaviour
{
    private static EnemyTurnIndicatorBehavior instance;
    [SerializeField] public GameObject arrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        show(false);
    }

    private EnemyTurnIndicatorBehavior() { }

    public static void show(bool show)
    {
        instance.arrow.SetActive(show);
    }

    public static void Move(float x)
    {
        instance.arrow.transform.position = new Vector3(x, instance.arrow.transform.position.y, 0);
    }
}
