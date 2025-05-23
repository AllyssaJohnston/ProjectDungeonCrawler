using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public enum E_GameMode
{
    LEVEL,
    COMBAT
}

public class GameManagerBehavior : MonoBehaviour
{
    private static GameManagerBehavior instance;
    public static E_GameMode gameMode = E_GameMode.LEVEL;
    public static bool inMenu = false;
    static bool loading = false;
    static List<string> scenesToLoad;
    static int curSceneToLoad;
    AsyncOperation asyncLoad;
    static GameObject combatData;
    static GameObject levelData;
    public static GameObject menuData;
    static string curScene;
    public static bool combatOnlyMode = false;
    public static float sensSlider;
    public static bool modernControls;
    private static AudioSource[] audioSources;
	private static AudioSource menuMusic;
	private static AudioSource ambience;
	private static AudioSource popSound;
    private static AudioSource levelTheme;
    private static AudioSource combatTheme;

	private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
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
        getAudio();

        if (curScene == "Combat")
        {
            ambience.Pause();
            levelTheme.Pause();
            menuMusic.Pause();
            combatTheme.Play();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("starting in combat");
            gameMode = E_GameMode.COMBAT;
            combatData = GameObject.FindWithTag("CombatData");
            combatOnlyMode = true;
            enterCombat(null, CombatManagerBehavior.getSimulateTutorial());
        }
        else if (curScene.Contains("Level") || curScene == "DesignPlayground")
        {
            ambience.Play();
            levelTheme.Play();
            menuMusic.Pause();
            combatTheme.Pause();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("starting in level");
            gameMode = E_GameMode.LEVEL;
            levelData = GameObject.FindWithTag("LevelData");
            scenesToLoad = new List<string> { "Combat", "Menu" };
            GameObject.FindWithTag("FirstPerson").GetComponent<FirstPerson>().setUp();
            // load in scenes async so they're ready when we need them
            instance.StartCoroutine(instance.StartLoad());
        }
        else if (curScene == "Menu")
        {
            inMenu = true;
            ambience.Pause();
            menuMusic.Play();
            levelTheme.Pause();
            combatTheme.Pause();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("starting in menu");
            gameMode = E_GameMode.LEVEL;
            menuData = GameObject.FindWithTag("MenuData");
            scenesToLoad = new List<string> { "Level1", "CombatTutorial" };
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

    // start combat
    public static void enterCombat(CombatEncounterBehavior encounter, bool enterTutorial = false)
    {
        ambience.Pause();
		menuMusic.Pause();
		levelTheme.Pause();
		combatTheme.Play();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Debug.Log("entering combat");
        if (!combatOnlyMode) //combat only mode is used to just test combat, other datas aren't set, so don't call set active on them
        {
            levelData.SetActive(false);
            menuData.SetActive(false);
            combatData.SetActive(true);
        }
        gameMode = E_GameMode.COMBAT;
        if (enterTutorial)
        {
            CombatManagerBehavior.inTutorial = true;
            CombatManagerBehavior.startTutorial();

        } 
        else
        {
            CombatManagerBehavior.inTutorial = false;
            CombatManagerBehavior.startBattle(encounter);
        }
    }

    // what to do when entering the level
    public static void enterLevel()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		if (combatOnlyMode)
        {
            DebugBehavior.updateLog("COMBAT ENDED");
            exit();
            return;
        }
        else
        {
            combatTheme.Pause();
			menuMusic.Pause();
			ambience.Play();
            levelTheme.Play();
            combatData.SetActive(false);
            menuData.SetActive(false);
            levelData.SetActive(true);
            gameMode = E_GameMode.LEVEL;
        }
    }
    
    private static void enterMenu()
    {
        inMenu = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Debug.Log("Enter pause menu");
        ambience.Pause();
		menuMusic.Play();
        levelTheme.Pause();
        combatTheme.Pause();
		levelData.SetActive(false);
        combatData.SetActive(false);
        menuData.SetActive(true);

    }

    // what to do when leaving menu
    public static void leaveMenu()
    {
        inMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		if (gameMode == E_GameMode.LEVEL)
        {
            DebugBehavior.updateLog("RE-ENTER LEVEL");
			menuMusic.Pause();
            if (!levelTheme.isPlaying)
                levelTheme.Play();
			if (!ambience.isPlaying)
				ambience.Play();
			menuData.SetActive(false);
            levelData.SetActive(true);

        } 
        else // COMBAT
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ambience.Pause();
            levelTheme.Pause();
			menuMusic.Pause();
			combatTheme.Play();
			DebugBehavior.updateLog("RE-ENTER COMBAT");
            menuData.SetActive(false);
            combatData.SetActive(true);
        }
    }

    public static void changeLevels(string levelName)
    {
        Scene curScene = levelData.scene;
        levelData.SetActive(false);
        levelData = null;
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));
        SceneManager.UnloadSceneAsync(curScene);
    }        

    private static void getAudio()
    {
        audioSources = instance.GetComponents<AudioSource>();
        ambience = audioSources[0];
        menuMusic = audioSources[1];
        popSound = audioSources[2];
        levelTheme = audioSources[3];
        combatTheme = audioSources[4];
    }

    public static void pop() 
    {
        popSound.Play();
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
                Debug.Log("updated combat data");
                combatData.SetActive(false);
            }
        }
        if (levelData == null)
        {
            levelData = GameObject.FindWithTag("LevelData");
            if (levelData != null)
            {
                Debug.Log("updated level data");
                levelData.SetActive(gameMode == E_GameMode.LEVEL && !inMenu);
                GameObject[] fps = GameObject.FindGameObjectsWithTag("FirstPerson");
                foreach(GameObject fp in fps)
                {
                    if (fp.scene == SceneManager.GetSceneByName(curScene))
                    {
                        fp.GetComponent<FirstPerson>().setUp();
                    }
                }
            }
        }
        if (menuData == null)
        {
            menuData = GameObject.FindWithTag("MenuData");
            if (menuData != null)
            {
                Debug.Log("updated menu data");
                menuData.SetActive(false);
                modernControls = menuData.GetComponentInChildren<Toggle>(true).isOn;
                sensSlider = menuData.GetComponentInChildren<Slider>(true).value;
            }
        }
    }
}
