using UnityEngine;
using UnityEngine.UI;

namespace menu
{
    public class MenuController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _playerSelector;

        // ========================== Main Menu ============================

        [Header("Main Menu")]
        [SerializeField] private Button _playbutton;

        public void OpenMainMenu()
        {
            _mainMenu.SetActive(true);

            // Add Listeners
            _playbutton.onClick.AddListener(Play);
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

        // ========================== Common ============================

        private void Play()
        {
            fsm.FSM.DispatchGameEvent(fsm.FSMControllerType.ALL, fsm.FSMStateType.ALL, fsm.FSMEventType.REQUEST_PLAY);
        }
    }
}
