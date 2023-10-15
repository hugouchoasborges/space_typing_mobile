using System;
using System.Collections.Generic;

namespace myproject.fsm
{
    public abstract class IFSMState : UnityEngine.MonoBehaviour
    {
        public bool IsActive => gameObject != null && gameObject.activeInHierarchy;

        // ----------------------------------------------------------------------------------
        // ========================== Infra - Actions ============================
        // ----------------------------------------------------------------------------------

        private Action<IFSMState> _actionGoToState;
        private Action<FSMStateType> _actionGoToStateByName;

        public List<FSMEventType> AllowedEventTypes { get; protected set; }

        public void RegisterActions(Action<IFSMState> goToState, Action<FSMStateType> goToStateByName)
        {
            _actionGoToState = goToState;
            _actionGoToStateByName = goToStateByName;
        }

        public void RegisterAllowedEvents(List<FSMEventType> allowedEvents)
        {
            AllowedEventTypes = new List<FSMEventType>(allowedEvents);
        }


        // ----------------------------------------------------------------------------------
        // ========================== State Transition ============================
        // ----------------------------------------------------------------------------------

        protected void GoToState(FSMStateType stateType) => _actionGoToStateByName.Invoke(stateType);
        protected void GoToState(IFSMState newState) => _actionGoToState.Invoke(newState);


        // ----------------------------------------------------------------------------------
        // ========================== State Callbacks ============================
        // ----------------------------------------------------------------------------------

        public virtual void OnStateEnter() { }
        public virtual void OnStateUpdate() { }
        public virtual void OnStateFixedUpdate() { }
        public virtual void OnStateExit() { }


        // ----------------------------------------------------------------------------------
        // ========================== State Events ============================
        // ----------------------------------------------------------------------------------

        public virtual void OnGameEventReceived(FSMEventType eventType, object data) { }
    }
}