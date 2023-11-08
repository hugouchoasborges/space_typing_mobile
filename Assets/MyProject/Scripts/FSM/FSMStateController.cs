using fsm.settings;
using log;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using tools.attributes;
using UnityEngine;

namespace fsm
{
    public class FSMStateController : MonoBehaviour
    {
        [SerializeField, RuntimeReadOnly]
        private FSMControllerType _controllerType = FSMControllerType.NONE;
        public FSMControllerType ControllerType => _controllerType;

        [SerializeField]
        private bool _allowAllEvents = true;

        [SerializeField]
        private bool _resetOnDisable = true;

#if UNITY_EDITOR

        public string PreviewGUI
        {
            get
            {
                return string.Format(
                "--- {0} ---\n" +
                "* Current State: {1}\n" +
                "* Previous State: {2}\n" +
                "* Last Received Events: \n{3}\n"
                , name, CurrentStateName, PreviousStateName, LastEventsReceivedStr);
            }
        }

#endif


        // ========================== States ============================

        // Initial State
        [SerializeField, RuntimeReadOnly]
        private FSMStateType _initialState = FSMStateType.IDLE;

        // Current State
        private IFSMState _currentState = null;
        public string CurrentStateName => _currentState == null ? "" : _statesCachedReversed[_currentState].ToString();
        public FSMStateType CurrentStateType => _currentState == null ? FSMStateType.NONE : _statesCachedReversed[_currentState];

        // Previous State
        private IFSMState _previousState = null;
        public string PreviousStateName => _previousState == null ? "" : _statesCachedReversed[_previousState].ToString();
        public FSMStateType PreviousStateType => _previousState == null ? FSMStateType.NONE : _statesCachedReversed[_previousState];

        // States Structure
        [LabelText("Serialized States")]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = false, HideRemoveButton = false, HideAddButton = false)]
        [SerializeField]
        private List<SerializableState> _statesSerialized = new List<SerializableState>();
        private Dictionary<FSMStateType, IFSMState> _statesCached = new Dictionary<FSMStateType, IFSMState>();
        private Dictionary<IFSMState, FSMStateType> _statesCachedReversed = new Dictionary<IFSMState, FSMStateType>();


        // ========================== Events ============================

        [SerializeField]
        protected List<FSMEventType> allowedEventTypes = new List<FSMEventType>();

        private void OnDisable()
        {
            if (_resetOnDisable && _initialized)
                GoToState(FSMStateType.NONE);

            FSM.UpdateStateControllersList();
        }

        private void OnEnable()
        {
            FSM.UpdateStateControllersList();

            // If it is already initialized and hasn't an active state
            if (_initialized && _currentState == null)
                GoToState(_initialState);
        }

        public const int MAX_EVENTS_RECEIVED_VERBOSE = 10;

        [LabelText("Ignored Cache Events")]
        [ListDrawerSettings(Expanded = false, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = true, HideRemoveButton = false, HideAddButton = false, NumberOfItemsPerPage = 3)]
        [SerializeField]
        [Tooltip("These entries will be ignored and not displayed at the 'Last Events Received' list")]
        private List<FSMEventType> _lastEventsReceivedIgnoreList = new List<FSMEventType>();
        private List<FSMEventType> _lastEventsReceived = new List<FSMEventType>();
        public string LastEventsReceivedStr
        {
            get
            {
                string events = "";
                for (int i = 0; i < _lastEventsReceived.Count; i++)
                    events += string.Format("** {0}. {1}\n", i + 1, _lastEventsReceived[i].ToString());

                return events;
            }
        }

#if UNITY_EDITOR
        [Button("FSM Settings")]
        private void GoToFSMSettings()
        {
            FSMSettingsSO.MenuItem_FSMSettings();
        }
#endif

        private void Start()
        {
            Initialize();
            GoToState(_initialState);
        }

        private bool _initialized = false;

        private void Initialize()
        {
            if (_initialized) return;

            _initialized = true;

            // Remove null states
            for (int i = _statesSerialized.Count - 1; i >= 0; i--)
                if (_statesSerialized[i] == null || _statesSerialized[i].State == null)
                    _statesSerialized.RemoveAt(i);

            // Fill cache
            for (int i = _statesSerialized.Count - 1; i >= 0; i--)
            {
                var serializedState = _statesSerialized[i];
                if (_statesCached.ContainsKey(serializedState.EventType))
                {
                    Debug.LogWarning("Removing duplicated state: " + serializedState.Name);
                    _statesSerialized.RemoveAt(i);
                    continue;
                }

                _statesCached[serializedState.EventType] = serializedState.State;
                _statesCached[serializedState.EventType].RegisterAllowedEvents(serializedState.AllowedEventTypes);
                _statesCached[serializedState.EventType].RegisterActions(GoToState, GoToState);

                _statesCachedReversed[_statesCached[serializedState.EventType]] = serializedState.EventType;
            }
        }

        // ----------------------------------------------------------------------------------
        // ========================== State Transition ============================
        // ----------------------------------------------------------------------------------

        private void GoToState(FSMStateType stateType)
        {
            if (stateType == FSMStateType.NONE)
            {
                ELog.Log(ELogType.FSM_STATE_TRANSITION, "{0}: Reseting FSM", name);
                _previousState = _currentState;
                _currentState = null;
                return;
            }

            if (!_statesCached.ContainsKey(stateType))
            {
                throw new KeyNotFoundException(string.Format("{0} state not found", stateType));
            }

            GoToState(_statesCached[stateType]);
        }

        private void GoToState(IFSMState newState)
        {
            if (_currentState != null && !_currentState.IsActive) return;

            if (!_statesCached.ContainsValue(newState))
            {
                throw new KeyNotFoundException(string.Format("{0} state not found", newState.GetType()));
            }

            // Exist current state
            if (_currentState != null)
            {
                ELog.Log(ELogType.FSM_STATE_TRANSITION, "{0}: Exiting State {1}", name, CurrentStateName);
                _currentState.OnStateExit();
                _previousState = _currentState;
                _currentState = null;
            }

            // Enter new state
            _currentState = newState;
            ELog.Log(ELogType.FSM_STATE_TRANSITION, "{0}: Entering State {1}", name, CurrentStateName);
            _currentState.OnStateEnter();
        }

        private void Update()
        {
            if (_currentState != null && _currentState.IsActive)
            {
                _currentState.OnStateUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (_currentState != null && _currentState.IsActive)
            {
                _currentState.OnStateFixedUpdate();
            }
        }


        // ----------------------------------------------------------------------------------
        // ========================== STATE EVENTS ============================
        // ----------------------------------------------------------------------------------

        public void ReceiveGameEvent(FSMEventType eventType, object data)
        {
            if (eventType == FSMEventType.NONE) return;
            if (_currentState == null) return;
            if (!_allowAllEvents && !_currentState.AllowedEventTypes.Contains(eventType) && !allowedEventTypes.Contains(eventType)) return;

            ELog.Log(ELogType.FSM_RECEIVE_EVENT, "{0}: Event Received: {1}{2}", name, eventType, data == null ? "" : " [contains data]");

#if UNITY_EDITOR
            CacheReceivedEvent(eventType);
#endif
            _currentState.OnGameEventReceived(eventType, data);
        }

        private void CacheReceivedEvent(FSMEventType eventType)
        {
            // Ignore event
            if (_lastEventsReceivedIgnoreList.Contains(eventType)) return;

            _lastEventsReceived.Insert(0, eventType);

            if (_lastEventsReceived.Count > MAX_EVENTS_RECEIVED_VERBOSE)
                _lastEventsReceived.RemoveAt(_lastEventsReceived.Count - 1);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            RemoveUnusedStates();
        }

        [ContextMenu("Remove Unused States")]
        private void RemoveUnusedStates()
        {
            bool HasState(System.Type type)
            {
                if (_statesSerialized == null) return false;
                foreach (var state in _statesSerialized)
                {
                    if (state.State != null && state.State.GetType() == type)
                        return true;
                }
                return false;
            }

            var components = gameObject.GetComponents<IFSMState>();
            foreach (var c in components)
            {
                if (HasState(c.GetType())) continue;

                Debug.Log("Removing Component: " + c.GetType().Name);
                UnityEditor.EditorApplication.delayCall += () => Component.DestroyImmediate(c);
            }
        }
#endif
    }

    [System.Serializable]
    internal class SerializableState
    {
        public string Name => EventType.ToString();

        [HorizontalGroup("Group")]
        [HideLabel]
        public FSMStateType EventType;

        [SerializeReference, AddFilteredComponent(buttonLabel: "Select State")]
        [HorizontalGroup("Group")]
        [HideLabel]
        public IFSMState State;

        [LabelText("Allowed Events")]
        [ListDrawerSettings(Expanded = false, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = true, HideRemoveButton = false, HideAddButton = false)]
        [SerializeField]
        public List<FSMEventType> AllowedEventTypes = new List<FSMEventType>();

        public SerializableState(FSMStateType eventType, IFSMState state)
        {
            EventType = eventType;
            State = state;
        }
    }
}