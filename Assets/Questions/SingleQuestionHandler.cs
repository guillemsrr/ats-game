using System;
using Menu;
using Resume.Base;
using UnityEngine;

namespace Questions
{
    public class SingleQuestionHandler : QuestionBase
    {
        [SerializeField] private ButtonHandler _yesButtonHandler;
        [SerializeField] private ButtonHandler _noButtonHandler;

        private void Awake()
        {
            _yesButtonHandler.OnClick += OnYes;
            _noButtonHandler.OnClick += OnNo;
        }

        private void OnNo(ButtonHandler arg0)
        {
        }

        private void OnYes(ButtonHandler arg0)
        {
        }
    }
}