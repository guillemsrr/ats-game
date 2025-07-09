// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace Questions
{
    public class RequirementsHandler : MonoBehaviour
    {
        [SerializeField] private Requirement _requirementModel;
        [SerializeField] private float _requirementSeparation = 5;

        private float _currentHeight;

        private List<RequirementData> _requirementDatas;

        public void Initialize()
        {
            _currentHeight = 0f;

            _requirementDatas = new List<RequirementData>();
        }

        public void AddRequirement(RequirementData requirementData)
        {
            Requirement question = Instantiate(_requirementModel, transform);
            question.transform.localPosition = new Vector3(0, 0, _currentHeight);
            question.Text.SetTextKey(requirementData.Text);

            _currentHeight -= _requirementSeparation;

            _requirementDatas.Add(requirementData);
        }

        public bool IsFit()
        {
            foreach (var requirementData in _requirementDatas)
            {
                if (!requirementData.Fits)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}