using collectable;
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
        private SceneType _menuScene;

        [SerializeField]
        private SceneType _gameScene;

        [SerializeField]
        private SceneType _initializationScene;


        public void LoadMainMenu()
        {
            SceneHelper.LoadSceneAsync(_menuScene, mode: LoadSceneMode.Additive, setAsActive: false, callback: OnMainMenuLoaded);
        }

        public void LoadPlayerSelector()
        {
            SceneHelper.LoadSceneAsync(_menuScene, mode: LoadSceneMode.Additive, setAsActive: false, callback: OnPlayerSelectorLoaded);
        }

        public void LoadGame()
        {
            SceneHelper.LoadSceneAsync(_gameScene, mode: LoadSceneMode.Additive, setAsActive: true, callback: OnGameLoaded);
        }

        public void LoadPause()
        {
            SceneHelper.LoadSceneAsync(_menuScene, mode: LoadSceneMode.Additive, setAsActive: true, callback: OnPauseLoaded);
        }

        public void LoadGameOver()
        {
            SceneHelper.LoadSceneAsync(_menuScene, mode: LoadSceneMode.Additive, setAsActive: true, callback: OnGameOverLoaded);
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
            if (SceneHelper.IsSceneLoaded(_menuScene))
                SceneHelper.UnloadSceneAsync(_menuScene);

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
            ResetGameScene();

            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.ON_APPLICATION_MAIN_MENU);
        }

        private void OnPauseLoaded()
        {
            // Pause enemy spawning
            _enemySpawner.StopSpawningEnemies();
            _enemySpawner.SetMovementActive(false);

            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.ON_APPLICATION_PAUSED);
        }

        private void OnGameOverLoaded()
        {
            ResetGameScene();

            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.ON_APPLICATION_GAME_OVER);
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

        public void ResetGameScene()
        {
            // Ensure players aren't instantiated
            _playerSpawner.DestroyAll();

            // Ensure enemies aren't instantiated
            _enemySpawner.StopSpawningEnemies();
            _enemySpawner.DestroyAll();

            // Remove all collectables
            _collectableSpawner.DestroyAll();
        }

        public void OnGameLoaded()
        {
            _enemySpawner.Initialize();
            _enemySpawner.StartSpawningEnemies();
            _enemySpawner.SetMovementActive(true);

            _playerSpawner.Initialize();
            if (_playerSpawner.PlayersActive == 0)
                _playerSpawner.Spawn();

            _collectableSpawner.Initialize();

            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.ON_APPLICATION_GAME);
        }


        // ========================== Player Spawning ============================

        [Header("Player")]
        [SerializeField] private PlayerSpawner _playerSpawner;

        public void OnPlayerDestroyed(PlayerController player)
        {
            // Tells PlayerSpawner to destroy player (pool)
            _playerSpawner.Destroy(player);

            // MEDO: Handle player lives
            // MEDO: Update players points
            // MEDO: Update GUI

            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.APPLICATION, fsm.FSMStateType.ALL, fsm.FSMEventType.REQUEST_GAME_OVER);
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
            _enemySpawner.Destroy(enemy);

            // MEDOING
            SpawnCollectables(enemy.transform.position);

            // MEDO: Update players points
            // MEDO: Update GUI
        }



        // ----------------------------------------------------------------------------------
        // ========================== Collectables ============================
        // ----------------------------------------------------------------------------------

        [Header("Collectables")]
        [SerializeField] private CollectableSpawner _collectableSpawner;

        // ========================== Spawning ============================


        private void SpawnCollectables(Vector2 position)
        {
            _collectableSpawner.Spawn(position);
        }


        // ========================== Collision ============================

        public void OnCollectableCollected(GameObject gObj)
        {
            Collectable collectable = gObj.GetComponent<Collectable>();

            if (collectable == null)
                throw new Exception(string.Format("'{0}' doesn't have a '{1}' component", gObj.name, nameof(Collectable)));

            _collectableSpawner.Destroy(collectable);

            // MEDO: Add points to player
            // MEDO: Add points to the screen
        }

    }
}