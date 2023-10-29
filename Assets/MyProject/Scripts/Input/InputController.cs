#if UNITY_EDITOR || !UNITY_ANDROID
#define HAS_KEYBOARD
#endif

using log;
using core;
using fsm;
using System.Collections.Generic;
using tools;
using UnityEngine;

namespace input
{
    public class InputController : MonoBehaviour
    {
        public Camera MainCamera => Locator.MainCamera;

        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;

        [Header("Raycast")]
        [Range(0, 100f)]
        [SerializeField]
        private float _rayCastMaxDist = 100f;

        [Header("Events")]
        [Range(0, .5f)]
        private float _maxTapDistance = .1f;

        private KeyboardInputController _keyboardInput;

        private void Awake()
        {
#if HAS_KEYBOARD
            // Make sure to add Keyboard support
            if (_keyboardInput == null)
                _keyboardInput = gameObject.GetComponent<KeyboardInputController>()
                    ?? gameObject.AddComponent<KeyboardInputController>();
#else
            // Make sure to remove Keyboard support
            _keyboardInput = gameObject.GetComponent<KeyboardInputController>();
            if (_keyboardInput != null)
                Destroy(_keyboardInput);
#endif

            SetPaused(true);
        }

        public void SetPaused(bool paused)
        {
            enabled = !paused;
#if HAS_KEYBOARD
            _keyboardInput.SetPaused(paused);
#endif
        }


        // ----------------------------------------------------------------------------------
        // ========================== Movement Input ============================
        // ----------------------------------------------------------------------------------

        private void OnTouchStarted(Vector2 screenPosition)
        {
            Vector2 position = MainCamera.ScreenToWorldPoint(screenPosition);
            ELog.Log(ELogType.TOUCH, "OnTouchStarted: {0}", position);

            SetStartObjectPosition(position);
            _startTouchPosition = position;

            TouchInputModel touchInput = new TouchInputModel(GetTouchTargets(position), _startTouchPosition);
            _currentTouchPosition = _startTouchPosition;

            fsm.FSM.DispatchGameEvent(FSMControllerType.ALL, FSMStateType.ALL, FSMEventType.TOUCH_STARTED, touchInput);
        }


        private void OnTouchMoved(Vector2 screenPosition)
        {
            Vector2 position = MainCamera.ScreenToWorldPoint(screenPosition);
            ELog.Log(ELogType.TOUCH, "OnTouchMoved: {0}", position);

            SetEndObjectPosition(position);
            Vector2 delta = _currentTouchPosition != default ? position - _currentTouchPosition : default;
            TouchInputModel touchInput = new TouchInputModel(GetTouchTargets(position), _startTouchPosition, currentPosition: position, delta: delta);

            _currentTouchPosition = position;
            FSM.DispatchGameEvent(FSMControllerType.ALL, FSMStateType.ALL, FSMEventType.TOUCH_MOVED, touchInput);
        }

        private void OnTouchCanceled(Vector2 screenPosition) => OnTouchEnded(screenPosition);
        private void OnTouchEnded(Vector2 screenPosition)
        {
            Vector2 position = MainCamera.ScreenToWorldPoint(screenPosition);
            ELog.Log(ELogType.TOUCH, "OnTouchEnded: {0}", position);

            SetEndObjectPosition(position);
            TouchInputModel touchInput = new TouchInputModel(GetTouchTargets(position), _startTouchPosition, position);

            FSM.DispatchGameEvent(FSMControllerType.ALL, FSMStateType.ALL, FSMEventType.TOUCH_ENDED, touchInput);
            _currentTouchPosition = _startTouchPosition;

            if ((_startTouchPosition - position).magnitude < _maxTapDistance)
            {
                ELog.Log(ELogType.TOUCH, "OnTouchTap: {0}", position);
                FSM.DispatchGameEvent(FSMControllerType.ALL, FSMStateType.ALL, FSMEventType.TOUCH_TAP, touchInput);
            }
        }

        private GameObject[] GetTouchTargets(Vector2 position)
        {
            List<GameObject> goList = new List<GameObject>();
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, transform.forward, _rayCastMaxDist);
            for (int i = 0; i < hits.Length; i++)
                goList.Add(hits[i].collider.gameObject);

            return goList.ToArray();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            // Mouse Touch
            if (Input.GetMouseButtonDown(0)) OnTouchStarted(Input.mousePosition);
            else if (Input.GetMouseButton(0)) OnTouchMoved(Input.mousePosition);
            else if (Input.GetMouseButtonUp(0)) OnTouchEnded(Input.mousePosition);
#else
            // TouchScreen
            //if (!Input.GetMouseButtonUp(0)) return;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) OnTouchStarted(touch.position);
            else if (touch.phase == TouchPhase.Moved) OnTouchMoved(touch.position);
            else if (touch.phase == TouchPhase.Ended) OnTouchEnded(touch.position);
            else if (touch.phase == TouchPhase.Canceled) OnTouchCanceled(touch.position);
#endif
        }

        // ----------------------------------------------------------------------------------
        // ========================== Editor input objects logic ============================
        // ----------------------------------------------------------------------------------

        [Header("Editor input objects")]
#if UNITY_EDITOR
        [SerializeField] private GameObject _startTouchObj;
        [SerializeField] private GameObject _endTouchObj;
#endif

        private DelayedCall _hideInputsDelayedCall;

        private void SetStartObjectPosition(Vector2 position)
        {
#if UNITY_EDITOR
            _startTouchObj.gameObject.SetActive(true);
            _startTouchObj.transform.position = position;

            _hideInputsDelayedCall?.Kill();
#endif
        }

        private void SetEndObjectPosition(Vector2 position)
        {
#if UNITY_EDITOR
            _endTouchObj.gameObject.SetActive(true);
            _endTouchObj.transform.position = position;

            _hideInputsDelayedCall?.Kill();
            _hideInputsDelayedCall = DOTweenDelayedCall.DelayedCall(HideInputObjects, 2f);
#endif
        }

        private void HideInputObjects()
        {
#if UNITY_EDITOR
            _startTouchObj.gameObject.SetActive(false);
            _endTouchObj.gameObject.SetActive(false);
#endif
        }
    }
}
