using enemy;
using player;
using scenes;
using System;
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
            SceneHelper.LoadSceneAsync(_mainMenuScene, mode: LoadSceneMode.Additive, setAsActive: false, callback: OnMainMenuLoaded);
        }

        public void LoadPlayerSelector()
        {
            SceneHelper.LoadSceneAsync(_mainMenuScene, mode: LoadSceneMode.Additive, setAsActive: false, callback: OnPlayerSelectorLoaded);
        }

        public void LoadGame()
        {
            SceneHelper.LoadSceneAsync(_gameScene, mode: LoadSceneMode.Additive, setAsActive: true, callback: OnGameLoaded);
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


        // ========================== Editor Hacked Initialization ============================

#if UNITY_EDITOR
        // "Hacked Initialization (Editor Only)"
        public static fsm.FSMStateType HackedStartupState
        {
            get => (fsm.FSMStateType)Enum.Parse(typeof(fsm.FSMStateType), PlayerPrefs.GetInt(nameof(HackedStartupState)).ToString());
            set => PlayerPrefs.SetInt(nameof(HackedStartupState), (int)value);
        }
#endif


        // ----------------------------------------------------------------------------------
        // ========================== Menu ============================
        // ----------------------------------------------------------------------------------

        private void OnMainMenuLoaded()
        {
            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.ON_APPLICATION_MAIN_MENU);
        }


        // ----------------------------------------------------------------------------------
        // ========================== Player Selector ============================
        // ----------------------------------------------------------------------------------

        private void OnPlayerSelectorLoaded()
        {
            _playerSpawner.Initialize();
            _playerSpawner.Spawn();

            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.ON_APPLICATION_PLAYER_SELECTOR);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Game ============================
        // ----------------------------------------------------------------------------------

        public void OnGameLoaded()
        {
            _enemySpawner.Initialize();
            _enemySpawner.StartSpawningEnemies();

            _playerSpawner.Initialize();
            _playerSpawner.Spawn();


            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.ON_APPLICATION_GAME);
        }


        // ========================== Player Spawning ============================

        [Header("Player")]
        [SerializeField] private PlayerSpawner _playerSpawner;

        public void OnPlayerDestroyed(PlayerController player)
        {
            // Tells PlayerSpawner to destroy player (pool)
            _playerSpawner.Destroy(player);

            // MEDO: Update players points
            // MEDO: Update GUI
        }

        // ========================== Player Events ============================



        // ========================== Enemy Spawning ============================

        [Header("Enemy")]
        [SerializeField] private EnemySpawner _enemySpawner;

        public void StartSpawningEnemies()
        {
            _enemySpawner.StartSpawningEnemies();
        }

        public void StopSpawningEnemies()
        {
            _enemySpawner.StopSpawningEnemies();
        }


        // ========================== Enemy Events ============================


        public void OnEnemyDestroyed(EnemyController enemy)
        {
            // Tells EnemySpawn to destroy enemy (pool)
            _enemySpawner.DestroyEnemy(enemy);

            // MEDO: Update players points
            // MEDO: Update GUI
        }
    }
}