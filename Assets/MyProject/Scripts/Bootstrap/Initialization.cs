using core;
using scenes;
using UnityEngine;

namespace bootstrap
{
    public class Initialization : MonoBehaviour
    {
        [SerializeField]
        private SceneType _sceneToLoad;

        public void Init()
        {
            // Initialize Stuff
            LoadMainScene();
        }

        private void LoadMainScene()
        {
            SceneHelper.LoadSceneAsync(_sceneToLoad);
        }
    }
}
