using log;
using myproject.core;
using myproject.fsm;
using System.Collections.Generic;
using tools;
using UnityEngine;

namespace myproject.input
{
    public class KeyboardInputController : MonoBehaviour
    {
        public Camera MainCamera => Locator.MainCamera;

        [Header("Fine Control")]
        [SerializeField][Range(5, 50)] private float _movementMultiplier = 20f;

        [Header("Inputs")]
        [SerializeField] private KeyCode _left = KeyCode.LeftArrow;
        [SerializeField] private KeyCode _right = KeyCode.RightArrow;
        [SerializeField] private KeyCode _up = KeyCode.UpArrow;
        [SerializeField] private KeyCode _down = KeyCode.DownArrow;

        [SerializeField] private KeyCode _shoot;


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

        private void CheckMovementInput()
        {
            float x = 0;
            float y = 0;

            if (Input.GetKey(_left))
                x -= 1;
            if (Input.GetKey(_right))
                x += 1;
            if (Input.GetKey(_up))
                y += 1;
            if (Input.GetKey(_down))
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
