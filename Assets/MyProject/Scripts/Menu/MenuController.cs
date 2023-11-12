using core;
using UnityEngine;
using UnityEngine.UI;

namespace menu
{
    public class MenuController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _playerSelector;
        [SerializeField] private GameObject _gameOver;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _gameUI;

        // ========================== Main Menu ============================

        [Header("Main Menu")]
        [SerializeField] private Button _playbutton;

        public void OpenMainMenu()
        {
            _mainMenu.SetActive(true);

            // Add Listeners
            _playbutton.onClick.AddListener(GoToPlayerSelector);
        }

        public void CloseMainMenu()
        {
            _mainMenu.SetActive(false);

            // Remove listeners
            _playbutton.onClick.RemoveAllListeners();
        }


        // ========================== Player Selector ============================

        [Header("Player Selector")]
        [SerializeField] private Button _playerSelectorNext;
        [SerializeField] private Button _playerSelectorPrevious;
        [SerializeField] private Button _playerSelectorPlay;

        public void OpenPlayerSelector()
        {
            _playerSelector.SetActive(true);

            // Add Listeners
            _playerSelectorPlay.onClick.AddListener(Play);
            _playerSelectorNext.onClick.AddListener(PlayerSelectorNext);
            _playerSelectorPrevious.onClick.AddListener(PlayerSelectorPrevious);
        }

        public void ClosePlayerSelector()
        {
            _playerSelector.SetActive(false);

            // Remove listeners
            _playerSelectorPlay.onClick.RemoveAllListeners();
            _playerSelectorNext.onClick.RemoveAllListeners();
            _playerSelectorPrevious.onClick.RemoveAllListeners();
        }

        private void PlayerSelectorNext()
        {
            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.PLAYER, fsm.FSMStateType.PLAYER_SELECTOR, fsm.FSMEventType.REQUEST_PLAYER_NEXT_SKIN);
        }

        private void PlayerSelectorPrevious()
        {
            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.PLAYER, fsm.FSMStateType.PLAYER_SELECTOR, fsm.FSMEventType.REQUEST_PLAYER_PREVIOUS_SKIN);
        }


        // ========================== Pause Menu ============================

        [Header("Pause")]
        [SerializeField] private Button _pauseResumeButton;
        [SerializeField] private Button _pauseMenuButton;

        public void OpenPause()
        {
            _pauseMenu.SetActive(true);

            // Add event listeners
            _pauseResumeButton.onClick.AddListener(Resume);
            _pauseMenuButton.onClick.AddListener(GotoMenu);
        }


        public void ClosePause()
        {
            _pauseMenu.SetActive(false);

            // Remove Listeners
            _pauseResumeButton.onClick.RemoveAllListeners();
            _pauseMenuButton.onClick.RemoveAllListeners();
        }

        // ========================== Game Over Menu ============================

        [Header("GameOver")]
        [SerializeField] private Button _gameOverPlayButton;
        [SerializeField] private Button _gameOverMenuButton;

        public void OpenGameOver()
        {
            _gameOver.SetActive(true);

            // Add event listeners
            _gameOverPlayButton.onClick.AddListener(GoToPlayerSelector);
            _gameOverMenuButton.onClick.AddListener(GotoMenu);
        }

        public void CloseGameOver()
        {
            _gameOver.SetActive(false);

            // Remvoe event listeners
            _gameOverPlayButton.onClick.RemoveAllListeners();
            _gameOverMenuButton.onClick.RemoveAllListeners();
        }


        // ========================== GAME UI ============================

        [Header("GAME")]
        [SerializeField] private Button _gamePauseButton;
        [SerializeField] private Button _powerUpButton;
        [SerializeField] private Image _powerUpButtonImage;

        public void OpenGameUI()
        {
            _gameUI.gameObject.SetActive(true);

            // Add event listeners
            _gamePauseButton.onClick.AddListener(GoToPause);
            _powerUpButton.onClick.AddListener(ActivatePowerUp);

            UpdatePowerUpButtonLoadPercentage();
        }

        public void CloseGameUI()
        {
            _gameUI.gameObject.SetActive(false);

            // Remove event listeners
            _gamePauseButton.onClick.RemoveAllListeners();
            _powerUpButton.onClick.RemoveAllListeners();
        }


        // =========== PowerUp ===========

        private void ActivatePowerUp()
        {
            SetPowerUpButtonInteractable(false);
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.REQUEST_POWER_UP);
        }

        private void SetPowerUpButtonInteractable(bool interactable)
        {
            _powerUpButton.interactable = interactable;
        }

        public void AddPowerUpLoadPercentage(int points)
        {
            float percFromPoints = _powerUpButtonImage.fillAmount +
                (points / (float)Locator.ApplicationController.PowerUpMultiShoot.PointsRequired);

            percFromPoints = Mathf.Clamp(percFromPoints, 0, 1);
            SetPowerUpButtonLoadPercentage(percFromPoints);
        }

        public void UpdatePowerUpButtonLoadPercentage()
        {
            SetPowerUpButtonLoadPercentage(Locator.ApplicationController.PlayerPowerUpMultiShoot.CurrentPercentage);
        }

        private void SetPowerUpButtonLoadPercentage(int points)
        {
            float percFromPoints = points / (float)Locator.ApplicationController.PowerUpMultiShoot.PointsRequired;
            SetPowerUpButtonLoadPercentage(percFromPoints);
        }

        public void SetPowerUpButtonLoadPercentage(float fillAmount)
        {
            _powerUpButtonImage.fillAmount = fillAmount;
            SetPowerUpButtonInteractable(fillAmount >= 1);
        }


        // ========================== Common ============================

        private void GoToPlayerSelector()
        {
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.REQUEST_PLAYER_SELECTOR);
        }

        private void Resume()
        {
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.REQUEST_RESUME);
        }

        private void GoToPause()
        {
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.REQUEST_PAUSE);
        }

        private void GotoMenu()
        {
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.REQUEST_MAIN_MENU);
        }

        private void Play()
        {
            fsm.FSM.DispatchGameEventAll(fsm.FSMEventType.REQUEST_PLAY);
        }
    }
}
