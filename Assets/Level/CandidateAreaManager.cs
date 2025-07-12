// Copyright (c) Guillem Serra. All Rights Reserved.

using Audio;
using Menu;
using Scanner;
using UnityEngine;

namespace Level
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

            DeactivateButtons();
        }

        private void OnUnFitClick(ButtonHandler arg0)
        {
            
        }

        private void OnFitClick(ButtonHandler arg0)
        {
            _returnButton.Show();
        }

        public void Initialize()
        {
            GameManager.Instance.CameraController.SetTargetZoom(20f);
            GameManager.Instance.CameraController.Center = _areaCenter;
            GameManager.Instance.PointerPlayer.FindPointLocationsArround(_areaCenter.position);

            AudioManager.Instance.PlayInGameMusic();

            _returnButton.Show();
            _fitCanditateButton.Show();
            _unfitCanditateButton.Show();

            _scanManager.Activate();
        }

        private void Return(ButtonHandler arg0)
        {
            GameManager.Instance.GoToLevelArea();
            _returnButton.Hide();

            DeactivateButtons();
        }

        void DeactivateButtons()
        {
            _returnButton.Button.Deactivate();
            _fitCanditateButton.Button.Deactivate();
            _unfitCanditateButton.Button.Deactivate();
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