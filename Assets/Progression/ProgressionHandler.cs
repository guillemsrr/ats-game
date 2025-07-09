// Copyright (c) Guillem Serra. All Rights Reserved.

using Menu;
using Resume.Base;
using UnityEngine;

namespace Progression
{
    public class ProgressionHandler : MonoBehaviour
    {
        [SerializeField] private TextHandler _candidateText;

        [SerializeField] private Transform _progressionContainer;
        [SerializeField] private ButtonHandler _progressionButtonModel;
        [SerializeField] private float _buttonSpace = 1f;

        private int _maxProgression;
        private int _currentProgression;
        public bool IsMaxProgression => _currentProgression >= _maxProgression;

        public void SetMaxProgression(int maxProgression)
        {
            _maxProgression = maxProgression;
            _currentProgression = 0;
            NextCandidate();

            for (int i = 0; i < _maxProgression; i++)
            {
                ButtonHandler button = Instantiate(_progressionButtonModel, _progressionContainer);
                button.transform.localPosition = new Vector3(i * _buttonSpace, 0, 0);
            }
        }

        public void NextCandidate()
        {
            _currentProgression++;
            _candidateText.SetTextKey("Candidate #" + (_currentProgression));
        }
    }
}