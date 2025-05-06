using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
    private static GameManagerBehavior instance;
    public List<CombatEncounterBehavior> combatEncounters = new List<CombatEncounterBehavior>();
    static bool loadingEncounter = false;
    static CombatEncounterBehavior encounter = null;
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

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (SceneManager.GetActiveScene().name == "Combat")
        {
            Debug.Log("starting in combat");
            OnLoadCombat(null); // combat already loaded, don't have to load it
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("quit");
            Application.Quit();
        }
        // call load combat to see if combat scene has loaded
        if (loadingEncounter)
        {
            instance.StartCoroutine(instance.LoadCombat());
        }
    }

    // load combat
    public static void enterCombat(CombatEncounterBehavior enc)
    {
        encounter = enc;
        instance.StartCoroutine(instance.LoadCombat());
    }

    // async load combat, and call OnLoadCombat when done
    private IEnumerator LoadCombat()
    {
        if (!loadingEncounter)
        {
            loadingEncounter = true;
            asyncLoad = SceneManager.LoadSceneAsync("Combat");
            asyncLoad.allowSceneActivation = false;
        }

        // Wait until the asynchronous scene fully loads
        while (asyncLoad.progress < .90f) // async laod will never progress above 90 apparently with allowSceneActivation = false
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        loadingEncounter = false;
        asyncLoad = null;
        Debug.Log("Combat loaded");
        OnLoadCombat(encounter);
    }

    // what to do after combat has loaded
    private static void OnLoadCombat(CombatEncounterBehavior encounter)
    {
        CombatManagerBehavior.startBattle(encounter);
    }
    
    // what to do when entering the level
    public static void enterLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
