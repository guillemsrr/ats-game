// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using FogOfWar;
using Menu;
using Player;
using Questions;
using Resume;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform _menuCameraCenter;
        [SerializeField] private Transform _levelCameraCenter;
        [SerializeField] private CameraController _cameraController;

        [SerializeField] private ResumeGenerator _resumeGenerator;
        [SerializeField] private RevealTransparencyHandler _revealTransparencyHandler;
        [SerializeField] private LevelQuestions _levelQuestions;
        [SerializeField] private Pointer _pointerPlayer;
        [SerializeField] private LevelsManager _levelsManager;
        [SerializeField] private ScanManager _scanManager;

        [SerializeField] private ButtonHandler _buttonHandler;

        private void Awake()
        {
            _buttonHandler.OnClick += ReturnToMenu;
        }

        private void ReturnToMenu(ButtonHandler arg0)
        {
            GoBackToMenu();

            _scanManager.DeActivate();
        }

        private void Start()
        {
            GoBackToMenu();
        }

        public void GoBackToMenu()
        {
            _cameraController.Center = _menuCameraCenter;
            _cameraController.SetTargetZoom(10);
            _cameraController.SetCameraOffset(0.2f);

            _pointerPlayer.SetMoveToMouse(false);
            _pointerPlayer.FindPointLocationsArround(_menuCameraCenter.position);
        }

        public void StartLevel(int level)
        {
            if (level <= 1)
            {
                _pointerPlayer.SetMoveToMouse(true);
                _scanManager.SetScanType(false);
            }
            else
            {
                _scanManager.SetScanType(true);
            }

            _scanManager.Activate();
            _levelsManager.StartLevel(level);
            _resumeGenerator.GenerateResume();
            _revealTransparencyHandler.Initialize();

            _pointerPlayer.FindPointLocationsArround(_levelCameraCenter.position);
            _cameraController.Center = _levelCameraCenter;
            _cameraController.SetTargetZoom(30);
            _cameraController.SetCameraOffset(0.5f);
        }
    }
}