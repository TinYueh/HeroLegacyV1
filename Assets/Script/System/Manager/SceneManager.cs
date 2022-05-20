using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem.Scene
{
    public class SceneManager : Singleton<SceneManager>
    {
        BackgroundController _backgroundController = null;
        CoverController _coverController = null;

        // 所有場景
        Dictionary<SystemCore.eGameScene, string> _dicScene = new Dictionary<SystemCore.eGameScene, string>();

        public override void Init()
        {
            _backgroundController = GameObject.Find("Background").transform.GetComponent<BackgroundController>();
            GameObject.DontDestroyOnLoad(_backgroundController);

            _coverController = GameObject.Find("Cover").transform.GetComponent<CoverController>();
            GameObject.DontDestroyOnLoad(_coverController);

            RegistAllScene();

            Debug.Log("SceneManager Init OK");
        }

        private void RegistAllScene()
        {
            RegistScene(SystemCore.eGameScene.E_GAME_SCENE_START, "StartScene");
            RegistScene(SystemCore.eGameScene.E_GAME_SCENE_COMBAT, "CombatScene");
        }

        private bool RegistScene(SystemCore.eGameScene gameScene, string sceneName)
        {
            if (_dicScene.TryAdd(gameScene, sceneName) == false)
            {
                Debug.LogError("RegistScene failed, Scene: " + gameScene);
                return false;
            }

            return true;
        }

        internal bool GetSceneName(SystemCore.eGameScene gameScene, out string outSceneName)
        {
            if (_dicScene.TryGetValue(gameScene, out outSceneName) == false)
            {
                Debug.LogError("GetSceneName failed, Scene: " + gameScene);
                return false;
            }

            return true;
        }

        internal IEnumerator LoadScene(SystemCore.eGameScene gameScene)
        {
            string sceneName = null;
            if (GetSceneName(gameScene, out sceneName) == false)
            {
                yield break;
            }

            _coverController.ShowPicture();

            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }

            _coverController.HidePicture();

            asyncOperation.allowSceneActivation = true;
        }
    }
}
