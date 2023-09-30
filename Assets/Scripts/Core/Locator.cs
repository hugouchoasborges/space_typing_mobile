using application;
using UnityEngine;

namespace myproject.core
{
    public class Locator
    {
        private static Camera _mainCamera;
        public static Camera MainCamera
        {
            get => _mainCamera ?? Camera.main;
            set => _mainCamera = value;
        }

        public static ApplicationController ApplicationController { get; set; }
    }
}

