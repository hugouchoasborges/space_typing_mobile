using UnityEditor;
using UnityEngine;

namespace tools.attributes
{
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneDrawer : PropertyDrawer
    {
        private SceneAttribute _attribute => (SceneAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (property.propertyType == SerializedPropertyType.String)
            {
                var sceneObject = GetSceneObject(property.stringValue);
                var scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
                if (scene == null)
                {
                    property.stringValue = "";
                }
                else if (GetScenePath(scene) != property.stringValue)
                {
                    var sceneObj = GetSceneObject(GetScenePath(scene));
                    if (sceneObj == null)
                    {
                        Debug.LogWarningFormat("The scene '{0}' couldn't be loaded.", scene.name);
                    }
                    else
                    {
                        property.stringValue = GetScenePath(scene);
                    }
                }
            }
            else
                EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
        }

        protected string GetScenePath(Object scene)
        {
            return AssetDatabase.GetAssetPath(scene);
        }

        protected SceneAsset GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
            {
                return null;
            }

            if (_attribute.AllowOutsideBuildReference)
                return AssetDatabase.LoadAssetAtPath(sceneObjectName, typeof(SceneAsset)) as SceneAsset;

            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.path.IndexOf(sceneObjectName) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }
            Debug.LogWarning("Scene [" + sceneObjectName + "] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.");
            return null;
        }
    }
}
