using System;
using GameJamBase.UI.View;
using Menu;
using UnityEngine;

namespace Questions
{
    public class SingleQuestionHandler : QuestionBase
    {
        [SerializeField] private SpatialButtonView YesSpatialButtonView;
        [SerializeField] private SpatialButtonView NoSpatialButtonView;

        private void Awake()
        {
            YesSpatialButtonView.OnClick += OnYes;
            NoSpatialButtonView.OnClick += OnNo;
        }

        private void OnNo(SpatialButtonView arg0)
        {
        }

        private void OnYes(SpatialButtonView arg0)
        {
        }
    }
}