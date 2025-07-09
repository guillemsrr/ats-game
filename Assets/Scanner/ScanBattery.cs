// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;

namespace Scanner
{
    public class ScanBattery : MonoBehaviour
    {
        [SerializeField] private float _charge;
        [SerializeField] private Scanner _scanner;

        [Header("Frame Segments")] public Transform topLine;
        public Transform rightLine;
        public Transform bottomLine;
        public Transform leftLine;

        [Header("Settings")] public float maxBattery = 5f;
        private float currentBattery;

        private float segmentPercent => currentBattery / maxBattery;

        private Vector3 topScale, rightScale, bottomScale, leftScale;

        public void RechargeBattery()
        {
            currentBattery = maxBattery;
            UpdateFrameVisual();
        }

        public bool IsDepleted() => currentBattery <= 0;

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void Decrease(float rate)
        {
            currentBattery -= rate;
            currentBattery = Mathf.Max(currentBattery, 0);
            UpdateFrameVisual();
        }

        void Start()
        {
            currentBattery = maxBattery;

            // Store initial scales
            topScale = topLine.localScale;
            rightScale = rightLine.localScale;
            bottomScale = bottomLine.localScale;
            leftScale = leftLine.localScale;
        }

        void UpdateFrameVisual()
        {
            float fill = segmentPercent * 4f; // Let it go from 0..4

            topLine.localScale = new Vector3(topScale.x * Mathf.Clamp01(fill - 0f), topScale.y, topScale.z);
            rightLine.localScale = new Vector3(rightScale.x * Mathf.Clamp01(fill - 1f), rightScale.y, rightScale.z);
            bottomLine.localScale = new Vector3(bottomScale.x * Mathf.Clamp01(fill - 2f), bottomScale.y, bottomScale.z);
            leftLine.localScale = new Vector3(leftScale.x * Mathf.Clamp01(fill - 3f), leftScale.y, leftScale.z);
        }
    }
}