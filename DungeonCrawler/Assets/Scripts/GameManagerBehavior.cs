using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum E_GameMode
{
    LEVEL,
    COMBAT
}

public class GameManagerBehavior : MonoBehaviour
{
    private static GameManagerBehavior instance;
    public static E_GameMode gameMode = E_GameMode.LEVEL;
    static bool loadCombat = false;
    static bool loading = false;
    AsyncOperation asyncLoad;
    static GameObject combatData;
    static GameObject levelData;
    static bool getData = false;
    public static bool combatOnlyMode = true;

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
            gameMode = E_GameMode.COMBAT;
            combatData = GameObject.FindWithTag("CombatData");
            combatOnlyMode = true;
            OnLoadCombat(null); // combat already loaded, don't have to load it
        }
        else
        {
            Debug.Log("starting in level");
            gameMode = E_GameMode.LEVEL;
            levelData = GameObject.FindWithTag("LevelData");
            loadCombat = true;
            combatOnlyMode = false;
            instance.StartCoroutine(instance.Load());
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
        if (loading)
        {
            instance.StartCoroutine(instance.Load());
        }
        if (getData)
        {
            if (loadCombat)
            {
                Debug.Log("Combat loaded");
                combatData = GameObject.FindWithTag("CombatData");
                if (combatData != null)
                {
                    combatData.SetActive(false);
                    getData = false;
                }
            }
            else
            {
                Debug.Log("Level loaded");
                levelData = GameObject.FindWithTag("LevelData");
                if (levelData != null)
                {
                    levelData.SetActive(false);
                    getData = false;
                }
            }        
        }
    }

    // load combat
    public static void enterCombat(CombatEncounterBehavior enc)
    {
        OnLoadCombat(enc);
    }

    // async load scenes
    private IEnumerator Load()
    {
        Debug.Log("load");
        if (!loading)
        {
            loading = true;
            if (loadCombat)
            {
                Debug.Log("loading combat");
                asyncLoad = SceneManager.LoadSceneAsync("Combat", LoadSceneMode.Additive);
            }
            else
            {
                Debug.Log("loading level");
                asyncLoad = SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);
            }
            asyncLoad.allowSceneActivation = false;
        }

        // Wait until the asynchronous scene fully loads
        while (asyncLoad == null || asyncLoad.progress < .90f) // async laod will never progress above 90 apparently with allowSceneActivation = false
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        loading = false;
        asyncLoad = null;
        getData = true;
        
           
    }

    // what to do after combat has loaded
    private static void OnLoadCombat(CombatEncounterBehavior encounter)
    {
        Debug.Log("on load combat");
        if (combatOnlyMode)
        {

        }
        else
        {
            levelData.SetActive(false);
            combatData.SetActive(true);
        }
        gameMode = E_GameMode.COMBAT;
        CombatManagerBehavior.startBattle(encounter);
    }
    
    // what to do when entering the level
    public static void enterLevel()
    {
        if (combatOnlyMode)
        {

        }
        else
        {
            levelData.SetActive(true);
            combatData.SetActive(false);
            gameMode = E_GameMode.LEVEL;
        }
    }
}
