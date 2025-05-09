using System;
using UnityEngine;

namespace LinkThemAll.Game
{
    public class InputController : MonoBehaviour
    {
        public Action<Vector3> OnFingerDown { get; set; }
        public Action OnFingerUp { get; set; }
        public Action<Vector3> OnInputMove { get; set; }

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
            OnFingerDown?.Invoke(Input.mousePosition);
        }

        private void FingerUp()
        {
            OnFingerUp?.Invoke();
        }

        private void FingerMove()
        {
            OnInputMove?.Invoke(Input.mousePosition);
        }
    }
}