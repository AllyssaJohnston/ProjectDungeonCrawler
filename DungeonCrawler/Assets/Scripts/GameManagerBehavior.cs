using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
    private static GameManagerBehavior instance;
    static bool loadingCombat = false;
    AsyncOperation asyncLoad;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("quit");
            Application.Quit();
        }
        if (loadingCombat)
        {
            instance.StartCoroutine(instance.LoadCombat());
        }
    }

    public static void enterCombat()
    {
        instance.StartCoroutine(instance.LoadCombat());
    }

    private IEnumerator LoadCombat()
    {
        if (!loadingCombat)
        {
            loadingCombat = true;
            asyncLoad = SceneManager.LoadSceneAsync("Combat");
            asyncLoad.allowSceneActivation = false;
        }

        // Wait until the asynchronous scene fully loads
        while (asyncLoad.progress < .90f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        loadingCombat = false;
        asyncLoad = null;
        Debug.Log("Combat loaded");
        CombatManagerBehavior.startBattle();
    }

    public static void enterLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
