// Copyright (c) Guillem Serra. All Rights Reserved.

using TMPro;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Resume.Base
{
    public class TextHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private LocalizeStringEvent _localizeStringEvent;

        public void SetTextKey(string key, StringTableCollection tableCollection = null)
        {
            if (!tableCollection)
            {
                SetUnlocalizedText(key);
                return;
            }

            var table = tableCollection.GetTable(LocalizationSettings.SelectedLocale.Identifier) as StringTable;
            if (table == null || !table.SharedData.Contains(key))
            {
                SetUnlocalizedText(key);
                return;
            }

            _localizeStringEvent.SetTable(tableCollection.TableCollectionNameReference);
            _localizeStringEvent.SetEntry(key);
            _localizeStringEvent.RefreshString();
        }

        private void SetUnlocalizedText(string key)
        {
            _text.text = key;
            _localizeStringEvent.enabled = false;
        }

        public void SetFontSize(float fontSize)
        {
            _text.fontSize = fontSize;
        }

        public void SetColor(Color color)
        {
            _text.color = color;
        }
    }
}