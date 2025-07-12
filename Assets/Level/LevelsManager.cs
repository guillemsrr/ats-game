// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
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

        [SerializeField] private List<LevelData> _levels;
        [SerializeField] private ResumeGenerator _resumeGenerator;
        [SerializeField] private RequirementsHandler _requirementsHandler;
        [SerializeField] private RequirementData _requirementData;
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
        }

        private void ReturnToMenu(ButtonHandler arg0)
        {
            GameManager.Instance.GoBackToMenu();
            _levelSelectorMenu.ActivateButtons();

            _returnToMenuHandler.Button.Deactivate();
            _proceedButton.Button.Deactivate();
            _restartButton.Button.Deactivate();
            _fitCanditateButton.Button.Deactivate();
            _unfitCanditateButton.Button.Deactivate();
        }

        public void StartLevel(int level)
        {
            _currentLevel = level;
            _scanManager.Reset();

            _progressionHandler.SetMaxProgression(5);

            _proceedButton.Show();
            _restartButton.Hide();
        }

        private void GenerateLevel()
        {
            _revealTransparencyHandler.ClearRenderTextureToBlack();

            float baseScale = 3.75f;
            float baseThreshold = 0.38f;
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

        private void GenerateResume()
        {
            var archetype = _resumeGenerator.GetRandomLevelArchetype(_currentLevel);
            ResumeData resumeData = _resumeGenerator.GenerateResume(archetype);

            bool allMet = Random.value > 0.5f;
            //allMet = true;
            float percentageMet = allMet ? 1f : Random.Range(0, 0.9f);
            List<RequirementPoco> requirements = _resumeGenerator.GetRandomRequirements(resumeData, archetype,
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
                _levelSelectorMenu.SetLevel(_currentLevel + 1);
                return;
            }

            HandleCorrectLevel();
        }

        private void HandleCorrectLevel()
        {
            _paperHandler.SetCorrect();
            _proceedButton.Show();

            _progressionHandler.GetLastProgressionLight().SetCorrect();
        }

        private void OnUnFitClick(ButtonHandler arg0)
        {
            HandleFitnessBase();

            //TODO: just go to the next candidate directly?
        }

        private void HandleIncorrectLevel()
        {
            _restartButton.Show();
            _paperHandler.SetIncorrect();

            _progressionHandler.GetLastProgressionLight().SetIncorrect();
        }

        private void HandleFitnessBase()
        {
            _fitCanditateButton.Hide();
            _unfitCanditateButton.Hide();
            _noiseRevealController.FullReveal();
        }

        private void OnProceedClick(ButtonHandler arg0)
        {
            _proceedButton.Hide();

            GenerateLevel();

            _fitCanditateButton.Show();
            _unfitCanditateButton.Show();

            _candidateAreaManager.Initialize();
        }

        private void OnRestartClick(ButtonHandler arg0)
        {
            _restartButton.Hide();

            StartLevel(_currentLevel);
        }
    }
}