using System;
using UnityEngine;

namespace LyonicDevelopment.PlayerAnimationLibrary.Mono
{
    public class CustomPlayerCameraController : MonoBehaviour
    {

        public GameObject CamParent;

        private PlayerController playerController;
        private Transform originalCamParent;

        private void Awake()
        {
            playerController = GetComponentInParent<PlayerController>();
            originalCamParent = playerController.forwardReference.parent;
        }

        public void SwitchCameraParent()
        {
            playerController.forwardReference.parent = playerController.forwardReference.parent == originalCamParent ? CamParent.transform : originalCamParent;
        }
    }
}