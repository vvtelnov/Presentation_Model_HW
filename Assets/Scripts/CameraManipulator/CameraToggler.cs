using UnityEngine;
using Zenject;

namespace CameraManipulator
{
    public class CameraToggler : ICameraToggler
    {
        [Inject(Id = "GameCamera")]
        private Camera _gameCamera;
        
        [Inject(Id = "UICamera")]
        private Camera _uiCamera;


        public void TurnOnUICamera()
        {
            _uiCamera.enabled = true;
            _gameCamera.enabled = false;
        }

        public void TurnOnGameCamera()
        {
            _uiCamera.enabled = false;
            _gameCamera.enabled = true;
        }
    }
}