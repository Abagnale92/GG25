using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public enum GameState
{
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
    public int lives = 3;
    private int score = 0;

    public List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoseLife()
    {
        lives -= 1;
        UIManager.UpdateLifeCounter();
        BubbleLogic[] bubbles = FindObjectsOfType<BubbleLogic>();
        foreach(BubbleLogic b in bubbles)
        {
            Destroy(b.gameObject);
        }

        if(lives <= 0)
        {
            Time.timeScale = 0;
            gameState = GameState.GAME_OVER;
            UIManager.ShowElement("RestartBtn");
        }
    }

    public static void RestartGame()
    {
        UIManager.HideElement("RestartBtn");
        Time.timeScale = 1;
        UnloadScene(1);
        LoadScene(1);
        GameManager.instance.lives = 3;
        GameManager.instance.score = 0;
        UIManager.UpdateLifeCounter();
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
            case GameState.GAME_SETUP:
                //State Code
                break;
            case GameState.GAME_LOOP:
                AudioManager.instance.SetPitch("music", 0.3f + Mathf.Clamp((Time.time / 300), 0, 1));
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
        UIManager.instance.UpdateScoreUI(score);
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
