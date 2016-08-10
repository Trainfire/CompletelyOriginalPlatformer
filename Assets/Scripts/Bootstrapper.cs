using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Bootstraps the game. 
/// When placed in a level, it will automatically initialize the game then return to the current level.
/// </summary>
class Bootstrapper : MonoBehaviour
{
    private string _sceneName;

    public void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;

        var game = FindObjectOfType<Game>();
        if (game == null)
        {
            StartCoroutine(Initialize());
        }
        else if (!game.Initialized)
        {
            game.Initialize();
        }
    }

    IEnumerator Initialize()
    {
        yield return SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);

        var existingBootstrappers = FindObjectsOfType<Bootstrapper>();
        foreach (var bootstrapper in existingBootstrappers)
        {
            if (bootstrapper != this)
            {
                Debug.Log("Found an existing bootstrapper in 'Main' during initialization. Removing it...");
                Destroy(bootstrapper);
            }
        }

        // Move Bootstrapper to Main.
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        // Unload the scene the bootstrapper is in.
        SceneManager.UnloadScene(_sceneName);

        // Initialize game passing in the level name as an argument.
        var game = FindObjectOfType<Game>();
        if (game == null)
        {
            Debug.LogError("Failed to find a Game component. The scene 'Main' should contain a Game component.");
        }
        else
        {
            game.Initialize(_sceneName);
        }
    }
}