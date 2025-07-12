// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections;
using FogOfWar;
using Level;
using UnityEngine;

namespace Scanner
{
    public class ScanManager : MonoBehaviour
    {
        [SerializeField] private RevealTransparencyHandler _revealTransparencyHandler;
        [SerializeField] private Scanner _scanner;
        [SerializeField] private ScanBattery _scanBattery;

        [SerializeField] private float _scanLength = 5f;
        [SerializeField] private float _scanOffset = 1f;
        [SerializeField] private float _depletionRate = 0.001f;
        [SerializeField] private float _scanRateSeconds = 0.01f;

        [SerializeField] private float _pixelSizeIncrease = 0.1f;
        [SerializeField] private float _zoomPixelRelation = 1f;

        private bool _linearScan = false;
        private bool _canScan = false;
        private Coroutine _scanCoroutine;

        float _scanPixelSize = 15f;
        private float _scanMultiplier = 1f;

        private float PixelSize => _scanPixelSize * _scanMultiplier;

        public void Awake()
        {
            _scanner.ScanStart += OnScanStart;
            _scanner.ScanEnd += OnScanEnd;
        }

        private void LateUpdate()
        {
            float zoom = GameManager.Instance.CameraController.GetCameraZoom();
            _scanPixelSize = zoom * _zoomPixelRelation;

            if (_canScan)
            {
                GameManager.Instance.PointerPlayer.Scale(zoom);
            }
        }

        public void Reset()
        {
            _scanBattery.RechargeBattery();
        }

        public void Activate()
        {
            _canScan = true;

            GameManager.Instance.PointerPlayer.SetUnscanningMaterial();
        }

        public void DeActivate()
        {
            _canScan = false;
            GameManager.Instance.PointerPlayer.SetDefaultMaterial();
            GameManager.Instance.PointerPlayer.Scale(0f);
        }

        public void SetLinearScan(bool isLinear)
        {
            _linearScan = isLinear;
        }

        private void OnScanEnd()
        {
            if (_scanCoroutine == null)
            {
                return;
            }

            StopCoroutine(_scanCoroutine);
            _scanCoroutine = null;

            _scanMultiplier = 1f;
            GameManager.Instance.PointerPlayer.SetUnscanningMaterial();
        }

        private void OnScanStart()
        {
            if (!_canScan)
            {
                return;
            }

            if (_scanBattery.IsDepleted())
            {
                return;
            }

            if (_scanCoroutine != null)
            {
                StopCoroutine(_scanCoroutine);
            }

            GameManager.Instance.PointerPlayer.SetScanningMaterial();

            if (_linearScan)
            {
                _scanCoroutine = StartCoroutine(LinearScan());
            }
            else
            {
                _scanCoroutine = StartCoroutine(AreaScan());
            }
        }

        private IEnumerator LinearScan()
        {
            int iterations = 1;
            while (true)
            {
                if (_scanBattery.IsDepleted())
                {
                    yield break;
                }

                for (int i = 0; i < iterations; i++)
                {
                    Vector3 position = _scanner.transform.position;
                    position += _scanner.transform.forward * i * _scanOffset;
                    _revealTransparencyHandler.RevealAtPoint(position, _scanPixelSize);
                    yield return new WaitForSeconds(_scanRateSeconds / (iterations * 10f));
                }

                DecreaseBattery();
                yield return new WaitForSeconds(_scanRateSeconds);
                IncreaseRevealPixelArea();
                iterations++;
            }
        }

        private IEnumerator AreaScan()
        {
            while (true)
            {
                if (_scanBattery.IsDepleted())
                {
                    yield break;
                }

                DecreaseBattery();
                _revealTransparencyHandler.RevealAtPoint(_scanner.transform.position, PixelSize);
                yield return new WaitForSeconds(_scanRateSeconds);
                IncreaseRevealPixelArea();
            }
        }

        private void DecreaseBattery()
        {
            _scanBattery.Decrease(_depletionRate * PixelSize * Time.fixedDeltaTime);
        }

        private void IncreaseRevealPixelArea()
        {
            //testing without scaling
            //_scanMultiplier += _pixelSizeIncrease * Time.fixedDeltaTime;
        }
    }
}