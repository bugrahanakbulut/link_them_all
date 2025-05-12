using System;
using UnityEngine;

namespace LinkThemAll.Game
{
    public class InputController : MonoBehaviour
    {
        private bool _locked = false;
        
        public Action<Vector3> OnFingerDown { get; set; }
        public Action OnFingerUp { get; set; }
        public Action<Vector3> OnInputMove { get; set; }

        private Vector3 _fingerPos;

        private const float MIN_INPUT_MAGNITUTDE = 5f; 

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                FingerDown();
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                FingerUp();
                return;
            }

            if (Input.GetMouseButton(0))
            {
                FingerMove();
            }
        }

        private void FingerDown()
        {
            if (_locked)
            {
                return;
            }
            
            _fingerPos = Input.mousePosition;
            
            OnFingerDown?.Invoke(_fingerPos);
        }

        private void FingerUp()
        {
            if (_locked)
            {
                return;
            }
            
            OnFingerUp?.Invoke();
        }

        private void FingerMove()
        {
            if (_locked)
            {
                return;
            }

            Vector3 deltaInput = Input.mousePosition - _fingerPos;

            if (deltaInput.magnitude < MIN_INPUT_MAGNITUTDE)
            {
                return;
            }
            
            _fingerPos = Input.mousePosition;
            
            OnInputMove?.Invoke(Input.mousePosition);
        }

        public void Lock()
        {
            _locked = true;
        }

        public void Unlock()
        {
            _locked = false;
        }
    }
}