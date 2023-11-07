using fsm;
using scenes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace application
{
    public class ApplicationPrepareState : AbstractApplicationState
    {
        [Header("Quality Settings")]
        [Range(1, 120), SerializeField] private int _targetFPS = 60;

        public override void OnStateEnter()
        {
            ApplyQualitySettings();
            LoadBaseScenes(() => GoToState(FSMStateType.IDLE));
        }

        private void LoadBaseScenes(Action callback)
        {
            SceneHelper.LoadSceneAsync(SceneType.GAME, LoadSceneMode.Additive, setAsActive: true);
            SceneHelper.LoadSceneAsync(SceneType.MAIN_MENU, LoadSceneMode.Additive, setAsActive: false, callback);
        }

        private void ApplyQualitySettings()
        {
            Application.targetFrameRate = _targetFPS;
        }
    }
}