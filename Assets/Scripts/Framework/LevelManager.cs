using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Framework
{
    public static class LevelManager
    {
        public static event Action<LevelLoadEvent> LevelUnloaded;
        public static event Action<LevelLoadEvent> LevelLoaded;

        public class LevelLoadEvent : EventArgs
        {
            public string SceneName { get; private set; }

            public LevelLoadEvent(string sceneName)
            {
                SceneName = sceneName;
            }
        }

        public static void LoadLevel(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);

            if (!scene.IsValid())
            {
                Debug.LogErrorFormat("Scene '{0}' does not exist and will not be loaded.", sceneName);
                //return;
            }

            if (LevelUnloaded != null)
                LevelUnloaded(new LevelLoadEvent(SceneManager.GetActiveScene().name));

            var levelLoader = GameObject.FindObjectOfType<LevelLoader>();

            levelLoader.LoadLevel(sceneName, () =>
            {
                if (LevelLoaded != null)
                    LevelLoaded(new LevelLoadEvent(sceneName));
            });
        }
    }
}
