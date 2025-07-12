// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Menu;
using Resume.Base;
using UnityEngine;

namespace Level.Progression
{
    public class ProgressionHandler : MonoBehaviour
    {
        [SerializeField] private TextHandler _candidateText;

        [SerializeField] private Transform _progressionContainer;
        [SerializeField] private ButtonLightHandler _progressionButtonModel;
        [SerializeField] private float _buttonSpace = 1f;

        private int _maxProgression;
        private int _currentProgression;
        public bool IsMaxProgression => _currentProgression >= _maxProgression;

        private List<ButtonLightHandler> _progressionLights;

        public void SetMaxProgression(int maxProgression)
        {
            _maxProgression = maxProgression;
            _currentProgression = 0;
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

        public void NextCandidate()
        {
            _currentProgression++;
            _candidateText.SetText("Candidate #" + (_currentProgression));
        }

        public ButtonLightHandler GetLastProgressionLight()
        {
            return _progressionLights[_currentProgression - 1];
        }
    }
}