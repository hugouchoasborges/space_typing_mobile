using log;
using scenes.settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace scenes
{
    public class SceneHelper
    {

        private static Dictionary<AsyncOperation, (string Scene, bool setAsActive, Action Callback)> _scenesToLoadCache = new Dictionary<AsyncOperation, (string Scene, bool setAsActive, Action Callback)>();
        private static Dictionary<AsyncOperation, (string Scene, Action Callback)> _scenesToUnloadCache = new Dictionary<AsyncOperation, (string Scene, Action Callback)>();

        public static bool IsSceneLoaded(SceneType scene)
        {
            string scenePath = SceneHelperTypeSettingsSO.Instance.GetScenePath(scene);
            return SceneManager.GetSceneByPath(scenePath).isLoaded;
        }

        public static int SceneCount()
        {
            return SceneManager.loadedSceneCount;
        }

        // ----------------------------------------------------------------------------------
        // ========================== Load Scenes ============================
        // ----------------------------------------------------------------------------------

        public static void LoadSceneAsync(SceneType scene, LoadSceneMode mode = LoadSceneMode.Single, bool setAsActive = false, Action callback = null)
        {

            string scenePath = SceneHelperTypeSettingsSO.Instance.GetScenePath(scene);

            if (IsSceneLoaded(scene))
            {
                ELog.LogWarning(ELogType.SCENE, "Cannot load the same scene twice: {0}", scene);

                if (setAsActive) SetSceneAsActive(scenePath);
                callback?.Invoke();

                return;
            }

            try
            {
                ELog.Log(ELogType.SCENE, "Loading scene '{0}'", scenePath);
                var asyncOp = SceneManager.LoadSceneAsync(scenePath, mode);
                _scenesToLoadCache.Add(asyncOp, (scenePath, setAsActive, callback));
                asyncOp.completed += OnSceneLoaded;
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Error Loading Scene", "Make sure all your scenes are set properly!", "Ok", "");
                SceneHelperSettingsSO.MenuItem_SceneTypeSettings();
#endif
                throw e;
            }
        }

        private static void OnSceneLoaded(UnityEngine.AsyncOperation obj)
        {
            obj.completed -= OnSceneLoaded;
            ELog.Log(ELogType.SCENE, "Loaded scene '{0}'", _scenesToLoadCache[obj].Scene);

            Action callback = _scenesToLoadCache[obj].Callback;
            if (_scenesToLoadCache[obj].setAsActive)
                SetSceneAsActive(_scenesToLoadCache[obj].Scene);
            _scenesToLoadCache.Remove(obj);
            callback?.Invoke();
        }

        public static void SetSceneAsActive(SceneType scene)
            => SetSceneAsActive(SceneHelperTypeSettingsSO.Instance.GetScenePath(scene));

        private static void SetSceneAsActive(string scenePath)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scenePath));
        }

        // ----------------------------------------------------------------------------------
        // ========================== Unload Scenes ============================
        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Unload all loaded scenes, except the parameters ones
        /// </summary>
        /// <param name="exceptions">List of scenes to keep loaded</param>
        public static void UnloadAllScenes(params SceneType[] exceptions)
        {
            foreach (SceneType sceneType in Enum.GetValues(typeof(SceneType)))
            {
                if (!IsSceneLoaded(sceneType)) continue;
                if (exceptions != null && exceptions.Contains(sceneType)) continue;

                UnloadSceneAsync(sceneType);
            }
        }

        public static void UnloadSceneAsync(SceneType scene, UnloadSceneOptions options = UnloadSceneOptions.None, Action callback = null)
        {
            if (!IsSceneLoaded(scene))
            {
                ELog.LogWarning(ELogType.SCENE, "Cannot unload an already unloaded scene: {0}", scene);
                return;
            }

            string scenePath = SceneHelperTypeSettingsSO.Instance.GetScenePath(scene);

            ELog.Log(ELogType.SCENE, "Unloading scene '{0}'", scenePath);
            var asyncOp = SceneManager.UnloadSceneAsync(scenePath, options);
            _scenesToUnloadCache.Add(asyncOp, (scenePath, callback));
            asyncOp.completed += OnSceneUnloaded;
        }

        private static void OnSceneUnloaded(AsyncOperation obj)
        {
            obj.completed -= OnSceneUnloaded;
            ELog.Log(ELogType.SCENE, "Unloaded scene '{0}'", _scenesToUnloadCache[obj].Scene);

            Action callback = _scenesToUnloadCache[obj].Callback;
            _scenesToUnloadCache.Remove(obj);
            callback?.Invoke();
        }

#if UNITY_EDITOR


        // ----------------------------------------------------------------------------------
        // ========================== Menu Items ============================
        // ----------------------------------------------------------------------------------

        public static void LoadEditorScene(SceneType scene, OpenSceneMode mode = OpenSceneMode.Single)
        {
            string scenePath = SceneHelperTypeSettingsSO.Instance.GetScenePath(scene);
            EditorSceneManager.OpenScene(scenePath, mode);
        }

        [MenuItem("MyProject/SCENES/Application")]
        private static void LoadScene_Application() => LoadEditorScene(SceneType.APPLICATION);

        [MenuItem("MyProject/SCENES/Initialization")]
        private static void LoadScene_Initialization() => LoadEditorScene(SceneType.INITIALIZATION);

        private static void LoadScene_Base()
        {
            LoadEditorScene(SceneType.APPLICATION);
            LoadEditorScene(SceneType.GAME, OpenSceneMode.Additive);
            LoadEditorScene(SceneType.MAIN_MENU, OpenSceneMode.Additive);
        }

        [MenuItem("MyProject/SCENES/Game")]
        private static void LoadScene_Game()
        {
            LoadScene_Base();
            SetSceneAsActive(SceneType.GAME);
            application.ApplicationController.HackedStartupState = fsm.FSMStateType.GAME;
        }

        [MenuItem("MyProject/SCENES/MainMenu")]
        private static void LoadScene_MainMenu()
        {
            LoadScene_Base();
            SetSceneAsActive(SceneType.MAIN_MENU);
            application.ApplicationController.HackedStartupState = fsm.FSMStateType.MENU;
        }

        [MenuItem("MyProject/SCENES/PlayerSelector")]
        private static void LoadScene_PlayerSelector()
        {
            LoadScene_Base();
            SetSceneAsActive(SceneType.GAME);
            application.ApplicationController.HackedStartupState = fsm.FSMStateType.PLAYER_SELECTOR;
        }

        [MenuItem("MyProject/SCENES/PauseMenu")]
        private static void LoadScene_PauseMenu()
        {
            LoadScene_Base();
            SetSceneAsActive(SceneType.MAIN_MENU);
            application.ApplicationController.HackedStartupState = fsm.FSMStateType.PAUSED;
        }

        [MenuItem("MyProject/SCENES/GameOver")]
        private static void LoadScene_GameOver()
        {
            LoadScene_Base();
            SetSceneAsActive(SceneType.MAIN_MENU);
            application.ApplicationController.HackedStartupState = fsm.FSMStateType.GAME_OVER;
        }


#endif
    }
}
