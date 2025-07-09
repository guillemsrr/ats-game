// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Menu
{
    public class LevelSelectorMenu: MonoBehaviour
    {
        [SerializeField] private List<ButtonHandler> _levelsButtons;
        [SerializeField] private GameManager _gameManager;

        private void Awake()
        {
            foreach (var buttonHandler in _levelsButtons)
            {
                buttonHandler.OnClick += OnLevelSelected;
            }
        }

        private void OnLevelSelected(ButtonHandler button)
        {
            int level = _levelsButtons.IndexOf(button);
            _gameManager.StartLevel(level);
        }
    }
}