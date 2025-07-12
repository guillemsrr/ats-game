// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;

namespace Scanner
{
    public class ScanBattery : MonoBehaviour
    {
        [SerializeField] private float _charge;
        [SerializeField] private Scanner _scanner;
        [SerializeField] private float _defaultDecreaseRatio = 0.1f;

        [Header("Line Segments")] public Transform _topLine;
        public Transform _rightLine;
        public Transform _bottomLine;
        public Transform _leftLine;

        private const float MAX_BATTERY = 1f;
        private float _currentBattery;

        private float SegmentPercent => _currentBattery / MAX_BATTERY;

        private Vector3 _topScale, _rightScale, _bottomScale, _leftScale;

        private float _decreaseRatioMultiplier = 0f;

        public void RechargeBattery()
        {
            _currentBattery = MAX_BATTERY;
            UpdateFrameVisual();
        }

        public bool IsDepleted() => _currentBattery <= 0;

        private void Start()
        {
            _currentBattery = MAX_BATTERY;

            // Store initial scales
            _topScale = _topLine.localScale;
            _rightScale = _rightLine.localScale;
            _bottomScale = _bottomLine.localScale;
            _leftScale = _leftLine.localScale;
        }

        private void Update()
        {
            Decrease(_defaultDecreaseRatio * _decreaseRatioMultiplier * Time.deltaTime);
        }

        private void LateUpdate()
        {
            UpdateFrameVisual();
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void SetDecreaseRatio(float ratioMultiplier)
        {
            _decreaseRatioMultiplier = ratioMultiplier;
        }

        public void Decrease(float rate)
        {
            _currentBattery -= rate;
            _currentBattery = Mathf.Max(_currentBattery, 0);
        }

        private void UpdateFrameVisual()
        {
            float fill = SegmentPercent * 4f; // Let it go from 0..4

            _topLine.localScale = new Vector3(_topScale.x * Mathf.Clamp01(fill - 0f), _topScale.y, _topScale.z);
            _rightLine.localScale = new Vector3(_rightScale.x * Mathf.Clamp01(fill - 1f), _rightScale.y, _rightScale.z);
            _bottomLine.localScale =
                new Vector3(_bottomScale.x * Mathf.Clamp01(fill - 2f), _bottomScale.y, _bottomScale.z);
            _leftLine.localScale = new Vector3(_leftScale.x * Mathf.Clamp01(fill - 3f), _leftScale.y, _leftScale.z);
        }
    }
}