// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using Audio;
using Menu;
using Scanner;
using UnityEngine;

namespace Manager
{
    public class CandidateAreaManager : MonoBehaviour
    {
        [SerializeField] private ScanArea _scanArea;
        [SerializeField] private ScanManager _scanManager;
        [SerializeField] private ButtonTextHandler _returnButton;
        [SerializeField] private ButtonTextHandler _fitCanditateButton;
        [SerializeField] private ButtonTextHandler _unfitCanditateButton;

        [SerializeField] private Transform _areaCenter;

        private void Awake()
        {
            _scanArea.OnMouseEnterAction += OnInRevealArea;
            _scanArea.OnMouseExitAction += OnOutOfRevealArea;
            _fitCanditateButton.Button.OnClick += OnFitClick;
            _unfitCanditateButton.Button.OnClick += OnUnFitClick;

            _returnButton.Button.OnClick += Return;
        }

        private void Start()
        {
            _returnButton.Hide();
        }

        private void OnUnFitClick(ButtonHandler arg0)
        {
            _returnButton.Show();
        }

        private void OnFitClick(ButtonHandler arg0)
        {
            _returnButton.Show();
        }

        public void Initialize()
        {
            GameManager.Instance.CameraController.Center = _areaCenter;
            AudioManager.Instance.PlayInGameMusic();
        }

        private void Return(ButtonHandler arg0)
        {
            GameManager.Instance.GoToLevelArea();
            _returnButton.Hide();
        }

        private void OnOutOfRevealArea()
        {
            GameManager.Instance.CameraController.AttachToCenter();
            GameManager.Instance.PointerPlayer.SetMoveToMouse(false);

            _scanManager.DeActivate();
            //_scanManager.SetLinearScan(true);
        }

        private void OnInRevealArea()
        {
            GameManager.Instance.CameraController.DettachFromCenter();
            GameManager.Instance.PointerPlayer.SetMoveToMouse(true);
            
            _scanManager.Activate();
            //_scanManager.SetLinearScan(false);
        }
    }
}