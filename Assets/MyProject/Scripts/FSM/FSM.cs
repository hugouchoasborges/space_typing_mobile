using log;
using fsm.settings;
using Sirenix.OdinInspector;
using tools;
using UnityEngine;

namespace fsm
{
    public class FSM : Singleton<FSM>
    {
        [SerializeField, ReadOnly]
        private FSMStateController[] _stateControllers;
        public FSMStateController[] StateControllers
        {
            get
            {
                //if (_stateControllers != null)
                //    foreach (var controller in _stateControllers)
                //        if (controller == null)
                //            UpdateStateControllersList();

                return _stateControllers;
            }
            set
            {
                _stateControllers = value;
            }
        }

#if UNITY_EDITOR

        public string PreviewGUI
        {
            get
            {
                string previews = "";

                foreach (var controller in StateControllers)
                {
                    previews += string.Format("{0}\n", controller.PreviewGUI);
                }

                return previews;
            }
        }

#endif

        private void Start()
        {
            UpdateStateControllersList();
        }

        public static void UpdateStateControllersList() => Instance?.Internal_UpdateStateControllersList();

        [Button("Update State Controllers")]
        private void Internal_UpdateStateControllersList()
        {
            StateControllers = FindObjectsOfType<FSMStateController>();
        }


        // ----------------------------------------------------------------------------------
        // ========================== Event Dispatch ============================
        // ----------------------------------------------------------------------------------

        // Static options
        public static void DispatchGameEvent(FSMControllerType controllerType, FSMStateType stateType, FSMEventType eventType, object data = null) => Instance?.Internal_DispatchGameEvent(controllerType, stateType, eventType, data);

        [Button("Test -- Dispatch Event")]
        private void Internal_DispatchGameEvent(FSMControllerType controllerType, FSMStateType stateType, FSMEventType eventType, object data = null)
        {
            if (StateControllers == null || StateControllers.Length == 0) return;

            ELog.Log(ELogType.FSM_DISPATCH_EVENT, "Dispatching Event: {0}[{1}][{2}]", eventType, stateType, controllerType);

            foreach (var controller in StateControllers)
            {
                if (controllerType != FSMControllerType.ALL && controller.ControllerType != controllerType) continue;
                if (stateType != FSMStateType.ALL && controller.CurrentStateType != stateType) continue;

                DoDispatchGameEvent(controller, eventType, data);
            }
        }

        private void DoDispatchGameEvent(FSMStateController controller, FSMEventType eventType, object data = null)
        {
            controller.ReceiveGameEvent(eventType, data);
        }

#if UNITY_EDITOR

        [PropertySpace(SpaceBefore = 30, SpaceAfter = 0)]
        [Button("FSM Settings", ButtonSizes.Large)]
        private void GoToFSMSettings()
        {
            FSMSettingsSO.MenuItem_FSMSettings();
        }
#endif
    }
}