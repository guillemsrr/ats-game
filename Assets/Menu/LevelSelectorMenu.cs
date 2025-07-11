// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using Manager;
using Resume.Base;
using UnityEngine;

namespace Menu
{
    public class LevelSelectorMenu : MonoBehaviour
    {
        [SerializeField] private List<ButtonTextHandler> _levelsButtons;

        [SerializeField] private TextHandler _levelText;

        private void Awake()
        {
            foreach (var buttonHandler in _levelsButtons)
            {
                buttonHandler.Button.OnClick += OnLevelSelected;
            }
        }

        private void OnLevelSelected(ButtonHandler button)
        {
            int level = 0;
            foreach (ButtonTextHandler buttonTextHandler in _levelsButtons)
            {
                level++;
                if (buttonTextHandler.Button != button)
                {
                    continue;
                }

                GameManager.Instance.PrepareLevel(level);
                _levelText.SetText("Level: " + buttonTextHandler.Text);
            }
        }

        public void SetLevel(int level)
        {
            if (level < 0 || level >= _levelsButtons.Count)
            {
                return;
            }

            OnLevelSelected(_levelsButtons[level].Button);
        }
    }
}