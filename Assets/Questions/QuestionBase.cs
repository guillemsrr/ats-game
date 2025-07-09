// Copyright (c) Guillem Serra. All Rights Reserved.

using Resume.Base;
using UnityEngine;

namespace Questions
{
    public class QuestionBase: MonoBehaviour
    {
        [SerializeField] private TextHandler _questionText;

        public void SetQuestion(QuestionData questionData)
        {
            _questionText.SetTextKey(questionData.Text, null);
        }
    }
}