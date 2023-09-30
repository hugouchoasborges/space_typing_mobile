using UnityEngine;

namespace myproject.input
{
    public struct TouchInputModel
    {
        public GameObject[] Targets;

        public Vector2 StartPosition;
        public Vector2 CurrentPosition;
        public Vector2 Delta;

        public TouchInputModel(GameObject[] targets,
            Vector2 startPosition,
            Vector2 currentPosition = default,
            Vector2 delta = default)
        {
            Targets = targets;

            StartPosition = startPosition;
            CurrentPosition = currentPosition;
            Delta = delta;
        }
    }
}
