using UnityEditor;
using UnityEngine;

namespace myproject.fsm
{
    [CustomPreview(typeof(FSMStateController))]
    public class FSMStateControllerPreview : ObjectPreview
    {
        private FSMStateController _controller => target as FSMStateController;

        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override GUIContent GetPreviewTitle()
        {
            string state = _controller.CurrentStateName;
            return new GUIContent(base.GetPreviewTitle().text + (!string.IsNullOrEmpty(state) ? " - " + state : ""));
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            GUI.Label(r, _controller.PreviewGUI, background);
        }
    }
}
