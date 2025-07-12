// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Menu
{
    public class LanguagesMenu : MonoBehaviour
    {
        [SerializeField] private ButtonHandler _englishButton;
        [SerializeField] private ButtonHandler _spanishButton;
        [SerializeField] private ButtonHandler _catalanButton;

        private void Awake()
        {
            _englishButton.OnClick += UpdateLanguage;
            _spanishButton.OnClick += UpdateLanguage;
            _catalanButton.OnClick += UpdateLanguage;
        }

        private void Start()
        {
            Locale locale = LocalizationSettings.SelectedLocale;
            if (locale.Identifier == "ca")
            {
                _catalanButton.SetClicked();
            }
            else if (locale.Identifier == "es")
            {
                _spanishButton.SetClicked();
            }
            else if (locale.Identifier == "en")
            {
                _englishButton.SetClicked();
            }
        }

        private void UpdateLanguage(ButtonHandler arg0)
        {
            if (arg0 == _englishButton)
            {
                LocaleSelected("en");
            }
            else if (arg0 == _spanishButton)
            {
                LocaleSelected("es");
            }
            else if (arg0 == _catalanButton)
            {
                LocaleSelected("ca");
            }
        }

        static void LocaleSelected(LocaleIdentifier locale)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(locale);
        }
    }
}