using fsm;
using log;
using player.settings;
using UnityEngine;

namespace player
{
    public class PlayerSelectorState : AbstractPlayerState
    {
        private PlayerSettingsSO _settings;
        private int _currentSkin = -1;
        private Sprite[] _availableSkins => _settings?.Skins;
        private bool _hasSkins => _availableSkins != null && _availableSkins.Length > 0;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _settings = PlayerSettingsSO.Instance;
            if (_availableSkins != null)
            {
                _currentSkin = 0;
                ApplyCurrentSkin();
            }
        }

        public override void OnGameEventReceived(FSMEventType eventType, object data)
        {
            base.OnGameEventReceived(eventType, data);

            switch (eventType)
            {
                case FSMEventType.REQUEST_PLAYER_NEXT_SKIN:
                    OnNextSkin();
                    break;

                case FSMEventType.REQUEST_PLAYER_PREVIOUS_SKIN:
                    OnPreviousSkin();
                    break;

                case FSMEventType.ON_APPLICATION_GAME:
                    GoToState(FSMStateType.GAME);
                    break;
            }
        }

        private void OnNextSkin()
        {
            if (!_hasSkins) return;

            _currentSkin = (_currentSkin + 1) % _availableSkins.Length;
            ApplyCurrentSkin();
        }

        private void OnPreviousSkin()
        {
            if (!_hasSkins) return;

            _currentSkin = _currentSkin - 1;
            if (_currentSkin < 0) _currentSkin = _availableSkins.Length - 1;
            ApplyCurrentSkin();
        }

        private void ApplyCurrentSkin() => ApplySkin(_currentSkin);

        private void ApplySkin(int skin)
        {
            if (_currentSkin < 0)
                throw new System.Exception("Skins not available");

            ApplySkin(_availableSkins[skin]);
        }

        private void ApplySkin(Sprite skin)
        {
            controller.ApplySkin(skin);
        }
    }
}
