// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Resume.Base
{
    public class TextHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private LocalizeStringEvent _localizeStringEvent;

        public string Text => _text.text;

        public float TextHeight => _text.rectTransform.rect.height;

        public void SetText(string key, string tableName = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "Empty or null key";
                Debug.LogError("Key cannot be null or empty");
            }

            if (string.IsNullOrEmpty(tableName))
            {
                SetUnlocalizedText(key);
                return;
            }

            StringTable table = LocalizationSettings.StringDatabase.GetTable(tableName,
                LocalizationSettings.SelectedLocale);
            if (table == null || table.GetEntry(key) == null)
            {
                SetUnlocalizedText(key);
                return;
            }

            _localizeStringEvent.SetTable(tableName);
            _localizeStringEvent.SetEntry(key);
            _localizeStringEvent.RefreshString();
        }

        public void SetText(LocalizedString localizedString)
        {
            if (localizedString.IsEmpty)
            {
                return;
            }

            _localizeStringEvent.StringReference = localizedString;
            _localizeStringEvent.enabled = true;
            _localizeStringEvent.RefreshString();
        }

        public IEnumerator DelayedSizeUpdate()
        {
            yield return new WaitForEndOfFrame();

            _text.ForceMeshUpdate();

            Vector2 textSize = _text.GetPreferredValues(_text.text, _text.rectTransform.rect.width, Mathf.Infinity);
            _text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textSize.y);
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

        public void SetFont(TMP_FontAsset font)
        {
            _text.font = font;
        }

        public void SetAlignment(TextAlignmentOptions alignment)
        {
            _text.alignment = alignment;
        }
    }
}