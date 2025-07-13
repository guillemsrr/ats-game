// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Menu;
using Resume.Base;
using UnityEngine;
using UnityEngine.Localization;

namespace Level.Progression
{
    public class ProgressionHandler : MonoBehaviour
    {
        [SerializeField] private TextHandler _candidateText;

        [SerializeField] private Transform _progressionContainer;
        [SerializeField] private ButtonLightHandler _progressionButtonModel;
        [SerializeField] private float _buttonSpace = 1f;
        [SerializeField] private LocalizedString _candidateLocalizedString;

        private int _maxProgression;
        private int _currentProgression;
        private int _currentCandidate;
        public bool IsMaxProgression => _currentProgression >= _maxProgression - 1;

        private List<ButtonLightHandler> _progressionLights;

        private const string LEVEL_KEY = "UnlockedLevel";

        public void SetMaxProgression(int maxProgression)
        {
            _maxProgression = maxProgression;
            _currentProgression = 0;
            _currentCandidate = 0;
            NextCandidate();

            _progressionLights = new List<ButtonLightHandler>();
            foreach (Transform child in _progressionContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _maxProgression; i++)
            {
                ButtonLightHandler button = Instantiate(_progressionButtonModel, _progressionContainer);
                button.transform.localPosition = new Vector3(i * _buttonSpace, 0, 0);
                _progressionLights.Add(button);
            }
        }

        public void NextProgression()
        {
            GetLastProgressionLight().SetCorrect();
            _currentProgression++;
        }

        public void NextCandidate()
        {
            _currentCandidate++;
            _candidateText.SetText(_candidateLocalizedString,  "#" + _currentCandidate);
        }

        public ButtonLightHandler GetLastProgressionLight()
        {
            return _progressionLights[_currentProgression];
        }

        public static int GetUnlockedLevel()
        {
            return PlayerPrefs.GetInt(LEVEL_KEY);
        }

        public static void ResetProgress()
        {
            PlayerPrefs.DeleteKey(LEVEL_KEY);
        }

        public void UnlockLevel(int level)
        {
            int current = GetUnlockedLevel();
            if (level > current)
            {
                PlayerPrefs.SetInt(LEVEL_KEY, level);
                PlayerPrefs.Save();
            }
        }
    }
}