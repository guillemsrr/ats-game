// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Audio;
using FogOfWar;
using Level.Progression;
using Menu;
using Questions;
using Resume;
using Resume.Data.Requirements;
using Scanner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class LevelsManager : MonoBehaviour
    {
        [System.Serializable]
        private class LevelData
        {
            public int Level;
            public GameObject LevelObject;
        }

        [SerializeField] private Transform _levelCameraCenter;

        [SerializeField] private List<LevelData> _levels;
        [SerializeField] private ResumeGenerator _resumeGenerator;
        [SerializeField] private RequirementsHandler _requirementsHandler;
        [SerializeField] private ProgressionHandler _progressionHandler;
        [SerializeField] private NoiseRevealController _noiseRevealController;
        [SerializeField] private RevealTransparencyHandler _revealTransparencyHandler;

        [SerializeField] private ButtonTextHandler _returnToMenuHandler;
        [SerializeField] private ButtonTextHandler _proceedButton;
        [SerializeField] private ButtonTextHandler _restartButton;

        [SerializeField] private ButtonTextHandler _fitCanditateButton;
        [SerializeField] private ButtonTextHandler _unfitCanditateButton;
        [SerializeField] private ScanManager _scanManager;
        [SerializeField] private PaperHandler _paperHandler;

        [SerializeField] private CandidateAreaManager _candidateAreaManager;
        [SerializeField] private LevelSelectorMenu _levelSelectorMenu;

        [SerializeField] private ScanBattery _scanBattery;

        [SerializeField] private float _levelRelationBatteryDecrease = 0.01f;

        private int _currentLevel;

        private void Awake()
        {
            _fitCanditateButton.Button.OnClick += OnFitClick;
            _unfitCanditateButton.Button.OnClick += OnUnFitClick;
            _proceedButton.Button.OnClick += OnProceedClick;
            _restartButton.Button.OnClick += OnRestartClick;

            _returnToMenuHandler.Button.OnClick += ReturnToMenu;
        }

        private void Start()
        {
            //TEST
            //_currentLevel = 5;
            //GenerateResume();

            HideButtons();
        }

        public void Focus()
        {
            GameManager.Instance.CameraController.Center = _levelCameraCenter;
            GameManager.Instance.CameraController.SetTargetZoom(15f);
            GameManager.Instance.PointerPlayer.FindPointLocationsArround(_levelCameraCenter.position);

            AudioManager.Instance.PlayMenuMusic();
        }

        private void ReturnToMenu(ButtonHandler arg0)
        {
            GameManager.Instance.GoBackToMenu();
            _levelSelectorMenu.ActivateButtons();

            HideButtons();
        }

        private void HideButtons()
        {
            _returnToMenuHandler.Hide();
            _proceedButton.Hide();
            _restartButton.Hide();
            _fitCanditateButton.Hide();
            _unfitCanditateButton.Hide();
        }

        public void StartLevel(int level)
        {
            _returnToMenuHandler.Show();
            _proceedButton.Show();
            _restartButton.Hide();

            _currentLevel = level;
            _scanManager.Reset();
            _candidateAreaManager.Reset();

            _progressionHandler.SetMaxProgression(5);
            _progressionHandler.UnlockLevel(level);
        }

        private void GenerateLevel()
        {
            _revealTransparencyHandler.ClearRenderTextureToBlack();

            float baseScale = 3.5f;
            float baseThreshold = 0.4f;
            if (_currentLevel <= 1)
            {
                baseScale = 5f;
                baseThreshold = 0.2f;
            }

            float noiseScale = baseScale + Mathf.Log(1 + _currentLevel) * 1.2f;
            float threshold = baseThreshold + _currentLevel * 0.05f;
            threshold = Mathf.Clamp(threshold, 0.4f, 0.9f);
            _noiseRevealController.StartPartialReveal(noiseScale, threshold);
            //Debug.Log("noiseScale " + noiseScale);

            GenerateResume();

            /*var levelObject = _levels.Find(x => x.Level == _currentLevel)?.LevelObject;
            if (levelObject == null)
            {
                return;
            }

            levelObject.SetActive(true);*/
        }

        private async void GenerateResume()
        {
            var archetype = _resumeGenerator.GetRandomLevelArchetype(_currentLevel);
            ResumeData resumeData = await _resumeGenerator.GenerateResume(archetype);

            bool allMet = Random.value > Mathf.Clamp01(0.25f + _currentLevel * 0.1f);
            //allMet = true;

            float percentageMet = allMet ? 1f : Random.Range(0, 0.9f);
            List<RequirementPoco> requirements = await _resumeGenerator.GetRandomRequirements(resumeData, archetype,
                percentageMet);
            _requirementsHandler.SetRequirements(requirements, allMet);
            _paperHandler.SetDefault();

            _resumeGenerator.GenerateVisualResume(resumeData);
        }

        private void OnFitClick(ButtonHandler arg0)
        {
            HandleFitnessBase();

            if (!_requirementsHandler.IsFit())
            {
                HandleIncorrectLevel();
                return;
            }

            if (_progressionHandler.IsMaxProgression)
            {
                //TODO: unlock new level
                _levelSelectorMenu.SetLevel(_currentLevel);
                return;
            }

            HandleCorrectLevel();
        }

        private void HandleCorrectLevel()
        {
            _paperHandler.SetCorrect();
            _proceedButton.Show();
            _restartButton.Hide();

            _progressionHandler.NextProgression();
            _progressionHandler.NextCandidate();
        }

        private void OnUnFitClick(ButtonHandler arg0)
        {
            _progressionHandler.NextCandidate();

            _proceedButton.Show();

            _scanBattery.SetDecreaseRatio(0f);

            //if i wanted to show the full paper
            //HandleFitnessBase();
            
            //if i want it directly:
            //ProceedNextCandidate();
        }

        private void HandleIncorrectLevel()
        {
            _restartButton.Show();
            _proceedButton.Hide();

            _paperHandler.SetIncorrect();

            _progressionHandler.GetLastProgressionLight().SetIncorrect();
        }

        private void HandleFitnessBase()
        {
            _fitCanditateButton.Hide();
            _unfitCanditateButton.Hide();
            _noiseRevealController.FullReveal();

            _scanBattery.SetDecreaseRatio(0f);
        }

        private void OnProceedClick(ButtonHandler arg0)
        {
            _proceedButton.Hide();

            ProceedNextCandidate();
        }

        private void ProceedNextCandidate()
        {
            GenerateLevel();

            _fitCanditateButton.Show();
            _unfitCanditateButton.Show();

            _candidateAreaManager.Initialize();

            float ratio = _levelRelationBatteryDecrease * (_currentLevel - 1);
            ratio = Mathf.Min(ratio, 0.1f);
            _scanBattery.SetDecreaseRatio(ratio);
        }

        private void OnRestartClick(ButtonHandler arg0)
        {
            _restartButton.Hide();

            StartLevel(_currentLevel);
        }
    }
}