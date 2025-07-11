// Copyright (c) Guillem Serra. All Rights Reserved.

using Audio;
using FogOfWar;
using Menu;
using Player;
using Scanner;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform _menuCameraCenter;
        [SerializeField] private Transform _levelCameraCenter;
        [SerializeField] private CameraController _cameraController;

        [SerializeField] private NoiseRevealController _noiseRevealController;
        [SerializeField] private Pointer _pointerPlayer;
        [SerializeField] private LevelsManager _levelsManager;

        public static GameManager Instance;

        public Pointer PointerPlayer => _pointerPlayer;
        public CameraController CameraController => _cameraController;

        [SerializeField] private int _seed;
        [SerializeField] private bool _randomSeed = true;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (_randomSeed)
            {
                _seed = Random.Range(0, int.MaxValue);
            }

            Random.InitState(_seed);

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            GoBackToMenu();
        }

        public void GoBackToMenu()
        {
            _cameraController.Center = _menuCameraCenter;
            _cameraController.SetTargetZoom(15f);
            _pointerPlayer.SetMoveToMouse(false);
            _pointerPlayer.FindPointLocationsArround(_menuCameraCenter.position);

            AudioManager.Instance.PlayMenuMusic();
        }

        public void PrepareLevel(int level)
        {
            _levelsManager.StartLevel(level);

            GoToLevelArea();
        }

        public void GoToLevelArea()
        {
            _cameraController.Center = _levelCameraCenter;
            _cameraController.SetTargetZoom(20f);
            _pointerPlayer.FindPointLocationsArround(_levelCameraCenter.position);

            AudioManager.Instance.PlayMenuMusic();
        }
    }
}