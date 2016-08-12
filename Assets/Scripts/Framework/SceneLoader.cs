using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

namespace Framework
{
    public class SceneLoader : MonoBehaviour
    {
        public event Action LoadCompleted;
        public event Action<float> LoadProgress;

        public string LoadingScene { get; set; }

        public IEnumerator Load(string[] sceneNames, Action onLoadComplete)
        {
            SceneManager.LoadScene(LoadingScene, LoadSceneMode.Additive);

            int scenesLoaded = 0;
            float totalProgress = 0f;
            foreach (var scene in sceneNames)
            {
                var task = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

                while (!task.isDone)
                {
                    totalProgress = (scenesLoaded + task.progress) / sceneNames.Length;
                    LoadProgress.InvokeSafe(totalProgress);
                    yield return null;
                }

                scenesLoaded++;
            }

            SceneManager.UnloadScene(LoadingScene);

            onLoadComplete.InvokeSafe();
        }
    }
}
