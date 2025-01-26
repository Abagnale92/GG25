using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public enum GameState
{
    MAIN_MENU,
    GAME_SETUP,
    GAME_LOOP,
    GAME_OVER
}

public enum SceneIndexs
{
    MANAGERS = 0,
    TEST_SCENE = 1
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] public GameState gameState;
    public int slipperyLevel = 0;
    public int score = 0;
    public float timeScore = 0;
    private int finalScore = 0;
    public GameObject bubblePrefab;

    public List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private bool showTutorial = false;

    public void LoseLife()
    {
        AudioManager.instance.SetPitch("music", 0.4f + Mathf.Clamp((slipperyLevel/20f), 0, 1));
        slipperyLevel += 1;
  


    }

   

    public void GameOverScreen()
    {
        gameState = GameState.GAME_OVER;
        // FindFirstObjectByType<Player>().GetComponent<Animator>().SetBool("Lost", true);
        UIManager.instance.SetupGameOverScreen();     

        
       
        UIManager.ShowElement("RestartBtn");
        Time.timeScale = 0;
    }

    public void ClearBubbles()
    {
        BubbleLogic[] bubbles = FindObjectsOfType<BubbleLogic>();
        foreach (BubbleLogic b in bubbles)
        {
            Destroy(b.gameObject);
        }


    }

    public static void RestartGame()
    {
        GameManager.instance.ClearBubbles();
        FindFirstObjectByType<Player>().GetComponent<Animator>().SetBool("Lost", false);
        UIManager.HideElement("RestartBtn");
        Time.timeScale = 1;
        UnloadScene(1);
        LoadScene(1);
        GameManager.instance.SetGameState(1);
        GameManager.instance.slipperyLevel = 0;
        GameManager.instance.score = 0;
        GameManager.instance.timeScore = 0;
        UIManager.instance.UpdateScoreUI(0);

    }

    public void SetGameState(int gameStateValue)
    {
        gameState = (GameState)gameStateValue;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Rest of your Awake code
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState) 
        {
            case GameState.MAIN_MENU:
                break;

            case GameState.GAME_SETUP:
                AudioManager.instance.SetPitch("music", 0.4f);

                if (!showTutorial)
                {
                    UIManager.ShowElement("Tutorial");
                    showTutorial = true;
                }
                
                gameState = GameState.GAME_LOOP;
                //State Code
                break;
            case GameState.GAME_LOOP:
                timeScore += Time.deltaTime;
                finalScore = Mathf.FloorToInt(timeScore + score);
                UIManager.instance.UpdateScoreUI(finalScore);
                //State Code
                break;
            case GameState.GAME_OVER:


                //State Code
                break;
        }
    }

    public static void LoadScene(int load)
    {
        GameManager.instance.scenesLoading.Add(SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive));
    }
    public static void UnloadScene(int unload)
    {
        GameManager.instance._UnloadScene(unload);
    }
    private void _UnloadScene(int unload)
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync(unload);
        GameManager.instance.scenesLoading.Add(op);
        StartCoroutine(RefreshAfterUnload(op));
    }
    public IEnumerator RefreshAfterUnload(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            yield return null;
        }
        if(operation != null)
            UIManager.instance.RefreshUIElemets();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        Debug.Log("Punteggio aggiornato: " + score);
        UIManager.instance.UpdateScoreUI(finalScore);
    }

    //LoadSceneWithCurtain allows you to load and unload one or multiple scenes
    //In addition it supports the visualization of the UI Element declared in the UI Manager
    #region LoadSceneWithCurtain
    public void LoadSceneWithDefaultCurtain(int load)
    {
        scenesLoading.Add(SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress(UIManager.instance.GetElementUIKey(0)));
    }

    public void LoadSceneWithCurtain(int[] load, string loadingScreen)
    {
        UIManager.ShowElement(loadingScreen);
        for (int i = 0; i < load.Length; i++)
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync(load[i], LoadSceneMode.Additive));
        }

        StartCoroutine(GetSceneLoadProgress(loadingScreen));
    }
    public void LoadSceneWithCurtain(int load, string loadingScreen)
    {
        scenesLoading.Add(SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress(loadingScreen));
    }
    public void LoadSceneWithCurtain(int[] unload, int[] load, string loadingScreen)
    {
        UIManager.ShowElement(loadingScreen);

        for(int i=0; i<unload.Length; i++)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(unload[i]));
        }
        for (int i = 0; i < load.Length; i++)
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync(load[i], LoadSceneMode.Additive));
        }

        StartCoroutine(GetSceneLoadProgress(loadingScreen));
    }
    public void LoadSceneWithCurtain(int unload, int load, string loadingScreen)
    {
        UIManager.ShowElement(loadingScreen);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(unload));
        scenesLoading.Add(SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress(loadingScreen));
    }
    public void LoadSceneWithCurtain(int[] unload, int load, string loadingScreen)
    {
        UIManager.ShowElement(loadingScreen);
        for (int i = 0; i < unload.Length; i++)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(unload[i]));
        }
        scenesLoading.Add(SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress(loadingScreen));
    }
    public void LoadSceneWithCurtain(int unload, int[] load, string loadingScreen)
    {
        UIManager.ShowElement(loadingScreen);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(unload));
        for (int i = 0; i < load.Length; i++)
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync(load[i], LoadSceneMode.Additive));
        }
        StartCoroutine(GetSceneLoadProgress(loadingScreen));
    }

    /*This method is meant to be called by a Coroutine. It will check the scenesLoading array and automatically disable
     *the UIElement with "loadingScreen" when the loading is complete.*/
    public IEnumerator GetSceneLoadProgress(string loadingScreen)
    {
        for(int i=0; i<scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }
        if(loadingScreen != null)
            UIManager.HideElement(loadingScreen);
    }

    #endregion
}
