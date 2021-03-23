using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.ChromaTower.View
{
    public class DragRotator : MonoBehaviour
    {
        [SerializeField] float dampening = 0.8f;

        private float delta;
        private ChromaTowerRenderer tower;
        private Vector2[] touchPos = new Vector2[2];
        private bool[] touchStates = new bool[2];
        private bool DragBegan { get => touchStates[0] && !touchStates[1]; }
        private bool Dragging { get => touchStates[0] && touchStates[1]; }
        private bool DragEnded { get => !touchStates[0] && touchStates[1]; }

        public void AttachTower(ChromaTowerRenderer tower)
        {
            this.tower = tower;
        }

        private Vector2 InputPosition
        {
            get
            {
#if UNITY_STANDALONE || UNITY_EDITOR
                return Input.mousePosition;
#else
                return Input.GetTouch(0).position;
#endif
            }
        }

        private bool InputDown
        {
            get
            {
#if UNITY_STANDALONE || UNITY_EDITOR
                return Input.GetMouseButton(0);
#else
                return Input.touchCount > 0;
#endif
            }
        }

        public float DragDelta()
        {
            touchStates[1] = touchStates[0];
            touchStates[0] = InputDown;

            if (DragBegan)
            {
                touchPos[0] = InputPosition;

                return 0;
            }
            else if (Dragging)
            {
                touchPos[1] = touchPos[0];
                touchPos[0] = InputPosition;

                return touchPos[1].x - touchPos[0].x;
            }

            return 0;
        }

        public void SmoothDelta()
        {
            delta = DragDelta();

            if(Dragging)
                delta = delta * (1 - dampening) + DragDelta() * dampening;
        }

        private void Update()
        {
            SmoothDelta();

            if(delta != 0)
                tower.transform.Rotate(Vector3.up, delta);
        }
    }
}
