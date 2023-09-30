using UnityEditor;
using UnityEngine;

namespace myproject.fsm
{
    [CustomPreview(typeof(FSM))]
    public class FSMPreview : ObjectPreview
    {
        private FSM _controller => target as FSM;
        private Vector2 _scrollPos;

        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            //_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,
            //    GUILayout.Width(r.width),
            //    GUILayout.Height(r.height));

            GUI.Label(r, _controller.PreviewGUI, background);

            //EditorGUILayout.EndScrollView();
        }
    }
}
