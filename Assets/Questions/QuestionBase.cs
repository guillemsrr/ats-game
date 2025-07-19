// Copyright (c) Guillem Serra. All Rights Reserved.

using GameJamBase.UI.View;
using Menu;
using UnityEngine;

namespace Questions
{
    public class QuestionBase: MonoBehaviour
    {
        [SerializeField] private TextHandler _questionText;

        public void SetQuestion(QuestionData questionData)
        {
            _questionText.SetText(questionData.Text, null);
        }
    }
}