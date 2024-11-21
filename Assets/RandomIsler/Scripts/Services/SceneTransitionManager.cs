using System.Collections.Generic;
using System.Threading.Tasks;
using RandomIsleser;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransitionManager
{
    private const string k_menuScene = "MenuScene";
    private const string k_OpeningScene = "TestOpenWorld";
    private const string k_OpenWorldScene = "TestOpenWorld";
    private const string k_LoadingSceneName = "LoadingScene";

    private static List<string> _loadedChunks = new List<string>();
    
    public static string CurrentScene => SceneManager.GetActiveScene().name;

    public static bool CurrentSceneIsSaveable()
    {
        return !CurrentSceneIsLoading();
    }

    public static bool CurrentSceneIsOpenWorld()
    {
        return SceneManager.GetActiveScene().name == k_OpenWorldScene;
    }

    public static bool CurrentSceneIsLoading()
    {
        return SceneManager.GetActiveScene().name == k_LoadingSceneName;
    }

    public static async void LoadScene(string sceneName)
    {
        await LoadSceneAsync(sceneName, SceneManager.GetActiveScene().name);
    }

    public static void LoadMenuScene()
    {
        LoadScene(k_menuScene);
    }

    public static void LoadOpeningScene()
    {
        LoadScene(k_OpeningScene);
    }

    public static async void LoadOpenWorld()
    {
        _loadedChunks.Clear();
        await LoadSceneAdditiveAsync(k_OpenWorldScene);
        
        //LoadChunks(PlayerController.Instance.GetChunks());
    }

    public static async void LoadChunks(List<string> worldChunkScenes)
    {
        foreach (var chunkName in worldChunkScenes)
        {
            _loadedChunks.Add(chunkName);
            await LoadSceneAdditiveAsync(chunkName);
        }
    }

    private static async Task LoadSceneAsync(string newScene, string currentScene)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(k_LoadingSceneName, LoadSceneMode.Additive);

        if (loadOperation == null)
        {
            LogNullScene(k_LoadingSceneName);
            return;
        }

        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }

        if (CurrentSceneIsOpenWorld())
        {
            foreach (var loadedChunk in _loadedChunks)
            {
                loadOperation = SceneManager.UnloadSceneAsync(loadedChunk);
                if (loadOperation == null)
                {
                    LogNullScene(loadedChunk);
                    return;
                }
                while (!loadOperation.isDone)
                {
                    await Task.Yield();
                }
            }
            _loadedChunks.Clear();
        }
        
        loadOperation = SceneManager.UnloadSceneAsync(currentScene);
        
        if (loadOperation == null)
        {
            LogNullScene(currentScene);
            return;
        }

        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
        loadOperation = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        
        if (loadOperation == null)
        {
            LogNullScene(newScene);
            return;
        }

        await Task.Delay(1000); // Makes it take a second minimum to load, can be removed once there's actually stuff to load

        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));

        AsyncOperation unLoadOperation = SceneManager.UnloadSceneAsync(k_LoadingSceneName);
        
        if (unLoadOperation == null)
        {
            Debug.LogError("Failed to remove loading scene");
            return;
        }

        while (!unLoadOperation.isDone)
        {
            await Task.Yield();
        }
        await Task.Yield();
    }

    private static async Task LoadSceneAdditiveAsync(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        if (loadOperation == null)
        {
            LogNullScene(k_LoadingSceneName);
            return;
        }

        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }

    private static void LogNullScene(string sceneName)
    {
        Debug.LogError($"Failed to load scene async: {sceneName} not found");
    }
}
