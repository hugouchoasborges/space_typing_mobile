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

        // ========================== Common ============================

        private void GoToPlayerSelector()
        {
            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.REQUEST_PLAYER_SELECTOR);
        }

        private void GotoMenu()
        {
            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.REQUEST_MAIN_MENU);
        }

        private void Play()
        {
            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.REQUEST_PLAY);
        }
    }
}
