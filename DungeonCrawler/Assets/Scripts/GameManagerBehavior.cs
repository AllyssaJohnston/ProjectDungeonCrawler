using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public enum E_GameMode
{
    LEVEL,
    COMBAT
}

public class GameManagerBehavior : MonoBehaviour
{
    private static GameManagerBehavior instance;
    public static E_GameMode gameMode = E_GameMode.LEVEL;
    static bool loading = false;
    static List<string> scenesToLoad;
    static int curSceneToLoad;
    AsyncOperation asyncLoad;
    static GameObject combatData;
    static GameObject combatTutorialData;
    static GameObject levelData;
    public static GameObject menuData;
    static string curScene;
    public static bool combatOnlyMode = false;
    public static float sensSlider;
    public static bool modernControls;
    private static AudioSource ambience;
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
        curSceneToLoad = 0;
        curScene = SceneManager.GetActiveScene().name;
        ambience = GetComponent<AudioSource>();
        if (curScene == "Combat")
        {
            ambience.Pause();
            Debug.Log("starting in combat");
            gameMode = E_GameMode.COMBAT;
            combatData = GameObject.FindWithTag("CombatData");
            combatOnlyMode = true;
            // combat already loaded, don't have to load it
            enterCombat(null);
        }
        if (curScene == "CombatTutorial")
        {
            ambience.Pause();
            Debug.Log("starting in combat tutorial");
            gameMode = E_GameMode.COMBAT;
            combatTutorialData = GameObject.FindWithTag("CombatTutorialData");
            combatOnlyMode = true;
            // combat already loaded, don't have to load it
            enterCombatTutorial(null);
        }
        else if (curScene.Contains("Level") || curScene == "DesignPlayground")
        {
            ambience.Play();
            Debug.Log("starting in level");
            gameMode = E_GameMode.LEVEL;
            levelData = GameObject.FindWithTag("LevelData");
            scenesToLoad = new List<string> { "CombatTutorial", "Combat", "Menu" };
            // load in scenes async so they're ready when we need them
            instance.StartCoroutine(instance.StartLoad());
        }
        else if (curScene == "Menu")
        {
            ambience.Pause();
            Debug.Log("starting in menu");
            gameMode = E_GameMode.LEVEL;
            menuData = GameObject.FindWithTag("MenuData");
            scenesToLoad = new List<string> { "Level1", "CombatTutorial", "Combat" };
            // load in scenes async so they're ready when we need them
            instance.StartCoroutine(instance.StartLoad());
        }
    }

    public void updateSettings()
    {
        modernControls = menuData.GetComponentInChildren<Toggle>(true).isOn;
        sensSlider = menuData.GetComponentInChildren<Slider>(true).value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enterMenu();
        }

        // load scenes async
        if (loading && instance != null)
        {
            instance.StartCoroutine(instance.ContinueLoad());
        }

        getRootData();
    }

    // load combat
    public static void enterCombat(CombatEncounterBehavior encounter)
    {
        ambience.Pause();
        Debug.Log("entering combat");
        if (!combatOnlyMode) //combat only mode is used to just test combat, so don't go back to the level
        {
            levelData.SetActive(false);
            menuData.SetActive(false);
            combatTutorialData.SetActive(false);
            combatData.SetActive(true);
        }
        gameMode = E_GameMode.COMBAT;
        CombatManagerBehavior.inTutorial = false;
        CombatManagerBehavior.startBattle(encounter);
    }

    // load combat
    public static void enterCombatTutorial(CombatEncounterBehavior encounter)
    {
        ambience.Pause();
        Debug.Log("entering combat tutorial ");
        if (!combatOnlyMode) //combat only mode is used to just test combat, so don't go back to the level
        {
            levelData.SetActive(false);
            menuData.SetActive(false);
            combatData.SetActive(false);
            combatTutorialData.SetActive(true);
        }
        gameMode = E_GameMode.COMBAT;
        CombatManagerBehavior.inTutorial = true;
        CombatManagerBehavior.startBattle(encounter);
    }

    // what to do when entering the level
    public static void enterLevel()
    {
        if (combatOnlyMode)
        {
            DebugBehavior.updateLog("COMBAT ENDED");
            exit();
        }
        else
        {
            ambience.Play();
            combatData.SetActive(false);
            menuData.SetActive(false);
            combatTutorialData.SetActive(false);
            levelData.SetActive(true);
            gameMode = E_GameMode.LEVEL;
        }
    }
    
    private static void enterMenu()
    {
        Debug.Log("Enter pause menu");
        ambience.Pause();
        levelData.SetActive(false);
        combatData.SetActive(false);
        combatTutorialData.SetActive(false);
        menuData.SetActive(true);

    }

    // what to do when leaving menu
    public static void leaveMenu()
    {
     
        if (gameMode == E_GameMode.LEVEL)
        {
            DebugBehavior.updateLog("RE-ENTER LEVEL");
            ambience.Play();
            menuData.SetActive(false);
            levelData.SetActive(true);

        } 
        else // COMBAT
        {
            ambience.Pause();
            DebugBehavior.updateLog("RE-ENTER COMBAT");
            menuData.SetActive(false);
            if (CombatManagerBehavior.inTutorial)
            {
                combatTutorialData.SetActive(true);
            }
            else
            {
                combatData.SetActive(true);
            }
        }
    }

    public static void exit()
    {
        SceneManager.LoadScene("Exit");
        Application.Quit();
    }

    private IEnumerator StartLoad()
    {
        loading = true;
        Debug.Log("Loading " + scenesToLoad[curSceneToLoad]);
        asyncLoad = SceneManager.LoadSceneAsync(scenesToLoad[curSceneToLoad], LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;
        return ContinueLoad();
    }

    // async load scenes
    private IEnumerator ContinueLoad()
    {
        // Wait until the asynchronous scene fully loads
        while (asyncLoad == null || asyncLoad.progress < .90f) // async load will never progress above 90 apparently with allowSceneActivation = false
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        asyncLoad = null;
        loading = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(curScene));

        Debug.Log("Loaded " + scenesToLoad[curSceneToLoad]);
        curSceneToLoad++;
        if (curSceneToLoad < scenesToLoad.Count)
        {
            // start next load
            StartLoad();
        }
        else
        {
            // all scenes loaded
            Debug.Log("Finished loading");
        }
    }

    private static void getRootData()
    {
        // get scene roots
        if (combatData == null)
        {
            combatData = GameObject.FindWithTag("CombatData");
            if (combatData != null)
            {
                combatData.SetActive(false);
            }
        }
        if (combatTutorialData == null)
        {
            combatTutorialData = GameObject.FindWithTag("CombatTutorialData");
            if (combatTutorialData != null)
            {
                combatTutorialData.SetActive(false);
            }
        }
        if (levelData == null)
        {
            levelData = GameObject.FindWithTag("LevelData");
            if (levelData != null)
            {
                levelData.SetActive(false);
            }
        }
        if (menuData == null)
        {
            menuData = GameObject.FindWithTag("MenuData");
            if (menuData != null)
            {
                menuData.SetActive(false);
                modernControls = menuData.GetComponentInChildren<Toggle>(true).isOn;
                sensSlider = menuData.GetComponentInChildren<Slider>(true).value;
            }
        }
    }
}
