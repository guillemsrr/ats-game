// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using GameJamBase.UI.View;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Menu
{
    public class LanguagesMenu : MonoBehaviour
    {
        [SerializeField] private SpatialButtonView EnglishSpatialButton;
        [SerializeField] private SpatialButtonView SpanishSpatialButton;
        [SerializeField] private SpatialButtonView CatalanSpatialButton;

        private void Awake()
        {
            EnglishSpatialButton.OnClick += UpdateLanguage;
            SpanishSpatialButton.OnClick += UpdateLanguage;
            CatalanSpatialButton.OnClick += UpdateLanguage;
        }

        private void Start()
        {
            Locale locale = LocalizationSettings.SelectedLocale;
            if (locale.Identifier == "ca")
            {
                //_catalanButton mark selected?
            }
            else if (locale.Identifier == "es")
            {
                //_spanishButton mark selected?
            }
            else if (locale.Identifier == "en")
            {
                //_englishButton mark selected?
            }
        }

        private void UpdateLanguage(SpatialButtonView arg0)
        {
            if (arg0 == EnglishSpatialButton)
            {
                LocaleSelected("en");
            }
            else if (arg0 == SpanishSpatialButton)
            {
                LocaleSelected("es");
            }
            else if (arg0 == CatalanSpatialButton)
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