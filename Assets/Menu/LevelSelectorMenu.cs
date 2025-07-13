// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using Level;
using Level.Progression;
using Resume.Base;
using UnityEngine;
using UnityEngine.Localization;

namespace Menu
{
    public class LevelSelectorMenu : MonoBehaviour
    {
        [SerializeField] private List<ButtonTextHandler> _levelsButtons;

        [SerializeField] private TextHandler _levelText;
        [SerializeField] private LocalizedString LevelLocalizedString;

        private void Awake()
        {
            foreach (var buttonHandler in _levelsButtons)
            {
                buttonHandler.Button.OnClick += OnLevelSelected;
            }
        }

        private void Start()
        {
            UpdateLevelButtonsUnlocked();
        }

        private void UpdateLevelButtonsUnlocked()
        {
            int maxLevel = ProgressionHandler.GetUnlockedLevel();

            for (int i = 0; i < _levelsButtons.Count; i++)
            {
                if (i < maxLevel)
                {
                    _levelsButtons[i].Show();
                }
                else
                {
                    _levelsButtons[i].SetUnclickable();
                }
            }

            _levelsButtons[0].Show();

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
                _levelText.SetText(LevelLocalizedString, buttonTextHandler.Text);

                DeactivateButtons();
                return;
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

        public void DeactivateButtons()
        {
            foreach (ButtonTextHandler buttonTextHandler in _levelsButtons)
            {
                buttonTextHandler.Button.Deactivate();
            }
        }

        public void ActivateButtons()
        {
            foreach (ButtonTextHandler buttonTextHandler in _levelsButtons)
            {
                buttonTextHandler.Button.Activate();
            }

            UpdateLevelButtonsUnlocked();
        }
    }
}