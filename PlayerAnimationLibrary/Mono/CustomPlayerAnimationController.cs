using System;
using UnityEngine;

namespace LyonicDevelopment.PlayerAnimationLibrary.Mono
{
    public class CustomPlayerAnimationController : MonoBehaviour
    {
        
        public static CustomPlayerAnimationController Instance { get; private set; }

        private CustomPlayerCameraController customCamController;

        private string[] animationClipNames;

        private GameObject playerViewObject;
        private GameObject customPlayerViewObject;
        
        private Animator customPlayerAnimator;

        public void Awake()
        {
            var customPlayerView = 
        }
    }
}