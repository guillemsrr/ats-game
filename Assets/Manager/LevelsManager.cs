// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using FogOfWar;
using Menu;
using Player;
using Progression;
using Questions;
using Resume;
using Resume.Base;
using Scanner;
using UnityEngine;

namespace Manager
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

        [SerializeField] private ButtonTextHandler _fitCanditateButton;
        [SerializeField] private ButtonTextHandler _unfitCanditateButton;

        [SerializeField] private ButtonTextHandler _nextCanditateButton;
        [SerializeField] private ButtonTextHandler _restartButton;
        [SerializeField] private TextHandler _levelText;

        [SerializeField] private Pointer _pointerPlayer;
        [SerializeField] private ScanManager _scanManager;
        [SerializeField] private PaperHandler _paperHandler;

        private int _currentLevel;

        private void Awake()
        {
            _fitCanditateButton.Button.OnClick += OnFitClick;
            _unfitCanditateButton.Button.OnClick += OnUnFitClick;
            _nextCanditateButton.Button.OnClick += OnNextClick;
            _restartButton.Button.OnClick += OnRestartClick;

            _revealTransparencyHandler.OnMouseEnterAction += OnInRevealArea;
            _revealTransparencyHandler.OnMouseExitAction += OnOutOfRevealArea;
        }

        private void Start()
        {
            _nextCanditateButton.Hide();
            _restartButton.Hide();
        }

        public void StartLevel(int level)
        {
            _currentLevel = level;

            /*if (_currentLevel <= 1)
            {
                _revealTransparencyHandler.PaintSquareSize = 60;
            }
            else
            {
                _revealTransparencyHandler.PaintSquareSize = 15;
            }*/

            _scanManager.Reset();
            _levelText.SetTextKey("Level " + _currentLevel);

            _progressionHandler.SetMaxProgression(5);

            GenerateLevel();
        }

        private void GenerateLevel()
        {
            _revealTransparencyHandler.ClearRenderTextureToBlack();
            _revealTransparencyHandler.PaintSquareSize = 7.5f;

            float baseScale = 3.5f;
            float baseThreshold = 0.6f;
            float noiseScale = baseScale + Mathf.Log(1 + _currentLevel) * 1.2f;
            float threshold = baseThreshold + _currentLevel * 0.05f;
            threshold = Mathf.Clamp(threshold, 0.4f, 0.9f);
            _noiseRevealController.StartReveal(noiseScale, threshold);
            //Debug.Log("noiseScale " + noiseScale);

            _resumeGenerator.GenerateResume();

            _requirementsHandler.Initialize();
            _requirementsHandler.AddRequirement(_requirementData);

            _paperHandler.SetDefault();

            if (_currentLevel != 0)
            {
                _levels[0].LevelObject.SetActive(true);
                return;
            }

            var levelObject = _levels.Find(x => x.Level == _currentLevel)?.LevelObject;
            if (levelObject == null)
            {
                return;
            }

            levelObject.SetActive(true);
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
                //TODO: WIN, Next Level
                return;
            }

            StartCoroutine(ShowNextButtonAfterTime());
        }

        private void OnUnFitClick(ButtonHandler arg0)
        {
            HandleFitnessBase();

            if (_requirementsHandler.IsFit())
            {
                HandleIncorrectLevel();
                return;
            }

            StartCoroutine(ShowNextButtonAfterTime());
        }

        private void HandleIncorrectLevel()
        {
            _restartButton.Show();
            _paperHandler.SetIncorrect();
        }

        private void HandleFitnessBase()
        {
            _fitCanditateButton.Hide();
            _unfitCanditateButton.Hide();
            _noiseRevealController.FullReveal();
        }

        private System.Collections.IEnumerator ShowNextButtonAfterTime()
        {
            _paperHandler.SetCorrect();
            yield return new WaitForSeconds(0.5f);
            _nextCanditateButton.Show();
        }

        private void OnNextClick(ButtonHandler arg0)
        {
            _nextCanditateButton.Hide();
            _progressionHandler.NextCandidate();

            StartCoroutine(NextAfterTime());
        }

        private System.Collections.IEnumerator NextAfterTime()
        {
            yield return new WaitForSeconds(0.5f);

            GenerateLevel();

            _fitCanditateButton.Show();
            _unfitCanditateButton.Show();
        }

        private void OnRestartClick(ButtonHandler arg0)
        {
            _restartButton.Hide();

            StartCoroutine(RestartAfterTime());
        }

        private System.Collections.IEnumerator RestartAfterTime()
        {
            yield return new WaitForSeconds(0.5f);

            StartLevel(_currentLevel);

            _fitCanditateButton.Show();
            _unfitCanditateButton.Show();
        }

        private void OnOutOfRevealArea()
        {
            _pointerPlayer.SetMoveToMouse(false);
            _scanManager.SetLinearScan(true);
        }

        private void OnInRevealArea()
        {
            _pointerPlayer.SetMoveToMouse(true);
            _scanManager.SetLinearScan(false);
        }
    }
}