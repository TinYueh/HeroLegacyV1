using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem.Scene
{
    public class SceneManager : Singleton<SceneManager>
    {
        #region Property

        BackgroundController _backgroundController;
        CoverController _coverController;

        // 所有場景
        Dictionary<SystemCore.eGameScene, string> _dicScene = new Dictionary<SystemCore.eGameScene, string>();

        #endregion  // Property

        #region Init

        public override bool Init()
        {
            _backgroundController = GameObject.Find("Background").transform.GetComponent<BackgroundController>();
            _backgroundController.Init();
            GameObject.DontDestroyOnLoad(_backgroundController);

            _coverController = GameObject.Find("Cover").transform.GetComponent<CoverController>();
            _coverController.Init();
            GameObject.DontDestroyOnLoad(_coverController);

            RegistAllScene();

            Debug.Log("SceneManager Init OK");

            return true;
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

        #endregion  // Init

        #region Get Set

        internal bool GetSceneName(SystemCore.eGameScene gameScene, out string outSceneName)
        {
            if (_dicScene.TryGetValue(gameScene, out outSceneName) == false)
            {
                Debug.LogError("GetSceneName failed, Scene: " + gameScene);
                return false;
            }

            return true;
        }

        #endregion  // Get Set

        #region Method

        internal IEnumerator LoadScene(SystemCore.eGameScene gameScene)
        {
            string sceneName;
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

        #endregion  // Method
    }
}