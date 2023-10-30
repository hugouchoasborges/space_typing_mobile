using scenes;
using tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace application
{
    public class ApplicationController : MonoBehaviour
    {

        // ----------------------------------------------------------------------------------
        // ========================== Scene Handling ============================
        // ----------------------------------------------------------------------------------

        [Header("Scenes")]
        [SerializeField]
        private SceneType _mainMenuScene;

        [SerializeField]
        private SceneType _gameScene;

        [SerializeField]
        private SceneType _initializationScene;


        public void LoadMainMenu()
        {
            SceneHelper.LoadSceneAsync(_mainMenuScene, mode: LoadSceneMode.Additive, setAsActive: true);
        }

        public void LoadGame()
        {
            SceneHelper.LoadSceneAsync(_gameScene, mode: LoadSceneMode.Additive, setAsActive: true);
        }

        private void LoadInitializationScene()
        {
            SceneHelper.LoadSceneAsync(_initializationScene, setAsActive: true);
        }

        public void RestartSystem()
        {
            UnloadAdditionalScenes();
            LoadInitializationScene();
        }

        private void UnloadAdditionalScenes()
        {
            if (SceneHelper.IsSceneLoaded(_mainMenuScene))
                SceneHelper.UnloadSceneAsync(_mainMenuScene);

            if (SceneHelper.IsSceneLoaded(_gameScene))
                SceneHelper.UnloadSceneAsync(_gameScene);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Enemy Spawning ============================
        // ----------------------------------------------------------------------------------

        private void Start()
        {
            // MEDO: Remove testing code;
            StartSpawningEnemies();
        }

        [Header("Components")]
        [SerializeField] private EnemySpawner _enemySpawner;

        public void StartSpawningEnemies()
        {
            _enemySpawner.StartSpawningEnemies();
        }

        public void StopSpawningEnemies()
        {
            _enemySpawner.StopSpawningEnemies();
        }
    }
}