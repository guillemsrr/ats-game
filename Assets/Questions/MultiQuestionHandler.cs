// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Menu;
using UnityEngine;

namespace Questions
{
    public class MultiQuestionHandler : QuestionBase
    {
        [SerializeField] private List<ButtonHandler> _buttonHandlers;

        private void Awake()
        {
            foreach (var buttonHandler in _buttonHandlers)
            {
                buttonHandler.OnClick += OnButtonClick;
            }
        }

        private void OnButtonClick(ButtonHandler button)
        {
            if (button.IsClicked)
            {
                
            }
        }
    }
}