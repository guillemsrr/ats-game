// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections;
using FogOfWar;
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
        [SerializeField] private float _depletionRate = 0.1f;
        [SerializeField] private float _scanRateSeconds = 0.01f;

        [SerializeField] private float _pixelSizeIncrease = 0.1f;

        private bool _linearScan = false;
        private bool _canScan = false;
        private Coroutine _scanCoroutine;

        public void Awake()
        {
            _scanner.ScanStart += OnScanStart;
            _scanner.ScanEnd += OnScanEnd;
        }

        public void Reset()
        {
            _scanBattery.RechargeBattery();
        }

        public void Activate()
        {
            _canScan = true;
        }

        public void DeActivate()
        {
            _canScan = false;
        }

        public void SetLinearScan(bool isLinear)
        {
            _linearScan = isLinear;
        }

        private void OnScanEnd()
        {
            if (_scanCoroutine != null)
            {
                StopCoroutine(_scanCoroutine);
                _scanCoroutine = null;

                _revealTransparencyHandler.PaintSquareSizeMultiplier = 1f;
            }
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
                    _revealTransparencyHandler.RevealAtPoint(position);
                    yield return new WaitForSeconds(_scanRateSeconds / (iterations * 10f));
                }

                _scanBattery.Decrease(_depletionRate * _revealTransparencyHandler.PaintSquareSizeMultiplier *
                                      Time.fixedDeltaTime);
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

                _scanBattery.Decrease(_depletionRate * Time.fixedDeltaTime);
                _revealTransparencyHandler.RevealAtPoint(_scanner.transform.position);
                yield return new WaitForSeconds(_scanRateSeconds);
                IncreaseRevealPixelArea();
            }
        }

        private void IncreaseRevealPixelArea()
        {
            _revealTransparencyHandler.PaintSquareSizeMultiplier += _pixelSizeIncrease * Time.fixedDeltaTime;
        }
    }
}