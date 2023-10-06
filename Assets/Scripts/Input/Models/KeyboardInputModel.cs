using System;
using UnityEngine;

namespace myproject.input
{
    public struct KeyboardInputModel
    {
        public float X;
        public float Y;
        public float Multiplier;

        public Vector2 Delta;

        public KeyboardInputModel(float x, float y, float multiplier)
        {
            X = x;
            Y = y;
            Multiplier = multiplier;
            Delta = Vector2.ClampMagnitude(new Vector2(X, Y) * Multiplier, Multiplier);
        }
    }
}
