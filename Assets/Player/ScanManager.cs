// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using FogOfWar;
using UnityEngine;

namespace Player
{
    public class ScanManager : MonoBehaviour
    {
        [SerializeField] private RevealTransparencyHandler _revealTransparencyHandler;
        [SerializeField] private Scanner _scanner;

        [SerializeField] private float _scanLength = 5f;
        [SerializeField] private float _scanOffset = 1f;

        private bool _linearScan = true;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Activate()
        {
            _scanner.ScanStart += OnScanStart;
            _scanner.ScanEnd += OnScanEnd;
        }

        public void DeActivate()
        {
            _scanner.ScanStart -= OnScanStart;
            _scanner.ScanEnd -= OnScanEnd;
        }

        public void SetScanType(bool isLinear)
        {
            _linearScan = isLinear;
        }

        private void OnScanEnd()
        {
        }

        private void OnScanStart()
        {
            if (_linearScan)
            {
                Vector3 startPosition = _scanner.transform.position;
                Vector3 position = startPosition;
                float distance = 0;
                while (distance < _scanLength)
                {
                    _revealTransparencyHandler.RevealAtPoint(position);
                    position += _scanner.transform.forward * _scanOffset;
                    distance = Vector3.Distance(startPosition, position);
                }
            }
            else
            {
                _revealTransparencyHandler.RevealAtPoint(_scanner.transform.position);
            }
        }
    }
}