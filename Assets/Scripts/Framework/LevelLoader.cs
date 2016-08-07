using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string sceneName, Action onDone)
    {
        StartCoroutine(Load(sceneName, onDone));
    }

    IEnumerator Load(string sceneName, Action onDone)
    {
        var task = SceneManager.LoadSceneAsync(sceneName);
        while (!task.isDone)
        {
            yield return null;
        }
        onDone();
    }
}
