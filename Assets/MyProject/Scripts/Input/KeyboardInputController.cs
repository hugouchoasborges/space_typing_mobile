using log;
using core;
using fsm;
using System.Collections.Generic;
using tools;
using UnityEngine;

namespace input
{
    public class KeyboardInputController : MonoBehaviour
    {
        public Camera MainCamera => Locator.MainCamera;

        [Header("Fine Control")]
        [SerializeField][Range(5, 50)] private float _movementMultiplier = 40f;

        [Header("Input - Movement")]
        [SerializeField] private KeyCode[] _leftKey = new KeyCode[] { KeyCode.LeftArrow, KeyCode.A };
        [SerializeField] private KeyCode[] _rightKey = new KeyCode[] { KeyCode.RightArrow, KeyCode.D };
        [SerializeField] private KeyCode[] _upKey = new KeyCode[] { KeyCode.UpArrow, KeyCode.W };
        [SerializeField] private KeyCode[] _downKey = new KeyCode[] { KeyCode.DownArrow, KeyCode.S };

        [Header("Input - Action")]
        [SerializeField] private KeyCode[] _shootKey = new KeyCode[] { KeyCode.Space };


        private void Awake()
        {
            SetPaused(true);
        }

        public void SetPaused(bool paused)
        {
            enabled = !paused;
        }


        // ----------------------------------------------------------------------------------
        // ========================== Movement ============================
        // ----------------------------------------------------------------------------------

        private bool CheckKeyCodeArray(KeyCode[] keyArray, System.Func<KeyCode, bool> myMethodName)
        {
            foreach (var key in keyArray)
                if (myMethodName(key))
                    return true;

            return false;
        }

        private void CheckMovementInput()
        {
            float x = 0;
            float y = 0;

            if (CheckKeyCodeArray(_leftKey, Input.GetKey))
                x -= 1;
            if (CheckKeyCodeArray(_rightKey, Input.GetKey))
                x += 1;
            if (CheckKeyCodeArray(_upKey, Input.GetKey))
                y += 1;
            if (CheckKeyCodeArray(_downKey, Input.GetKey))
                y -= 1;

            if (x != 0 || y != 0)
            {
                FSM.DispatchGameEvent(FSMControllerType.ALL, FSMStateType.ALL, FSMEventType.KEYBOARD_MOVED,
                    new KeyboardInputModel(x, y, _movementMultiplier * Time.deltaTime));
            }

        }


        // ----------------------------------------------------------------------------------
        // ========================== Action ============================
        // ----------------------------------------------------------------------------------

        private void CheckActionInput()
        {
            // The shooting action will be triggered by holding the Shoot Key
            // It's is up for the Player\Gun to define the fire rate
            if (CheckKeyCodeArray(_shootKey, Input.GetKey))
                ActionShoot();
        }

        private void ActionShoot()
        {
            FSM.DispatchGameEvent(FSMControllerType.ALL, FSMStateType.ALL, FSMEventType.PLAYER_SHOOT);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Update ============================
        // ----------------------------------------------------------------------------------


        void Update()
        {
            CheckMovementInput();
            CheckActionInput();
        }
    }
}
