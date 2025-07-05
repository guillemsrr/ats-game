// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;

namespace Resume.Base
{
    public class VerticalLayout3D : MonoBehaviour
    {
        public float spacing = 0.3f;
        public bool alignTopToBottom = true;

        void Start()
        {
            //ApplyLayout();
        }

        public void ApplyLayout()
        {
            float y = 0f;
            int direction = alignTopToBottom ? -1 : 1;

            foreach (Transform child in transform)
            {
                child.localPosition = new Vector3(0, y, 0);
                y += spacing * direction;
            }
        }
    }
}