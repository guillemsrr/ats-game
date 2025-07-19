// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using FogOfWar;
using GameJamBase.Audio;
using GameJamBase.UI.View;
using Menu;
using Questions;
using Scanner;
using UnityEngine;

namespace Level
{
    public class CandidateAreaManager : MonoBehaviour
    {
        [SerializeField] private ScanArea _scanArea;
        [SerializeField] private ScanManager _scanManager;
        [SerializeField] private ButtonTextHandler _fitCanditateButton;
        [SerializeField] private ButtonTextHandler _unfitCanditateButton;

        [SerializeField] private Transform _areaCenter;
        [SerializeField] private RequirementsHandler _requirementsHandler;
        [SerializeField] private RevealTransparencyHandler _revealTransparency;

        private void Awake()
        {
            _scanArea.OnMouseEnterAction += OnInRevealArea;
            _scanArea.OnMouseExitAction += OnOutOfRevealArea;
            _fitCanditateButton.SpatialButton.OnClick += OnFitClick;
            _unfitCanditateButton.SpatialButton.OnClick += OnUnFitClick;
        }

        private void Start()
        {
            _scanArea.gameObject.SetActive(false);

            DeactivateButtons();
        }

        private void OnUnFitClick(SpatialButtonView arg0)
        {
            DeactivateButtons();
            GameManager.Instance.GoToLevelArea();
        }

        private void OnFitClick(SpatialButtonView arg0)
        {
            DeactivateButtons();
            GameManager.Instance.GoToLevelArea();
        }

        public void Initialize()
        {
            GameManager.Instance.CameraController.SetTargetZoom(20f);
            GameManager.Instance.CameraController.Center = _areaCenter;
            GameManager.Instance.PointerPlayer.FindPointLocationsArround(_areaCenter.position);

            AudioManager.Instance.PlayInGameMusic();

            _scanArea.gameObject.SetActive(true);

            _fitCanditateButton.Show();
            _unfitCanditateButton.Show();
        }

        public void Reset()
        {
            _revealTransparency.ClearRenderTextureToBlack();
            _scanManager.DeActivate();
            _requirementsHandler.Clear();

            DeactivateButtons();
        }

        private void OnReturn(SpatialButtonView arg0)
        {
            GameManager.Instance.GoToLevelArea();

            Reset();
        }

        void DeactivateButtons()
        {
            _fitCanditateButton.Hide();
            _unfitCanditateButton.Hide();
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