// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using Resume.Data.Requirements;
using UnityEngine;

namespace Questions
{
    public class RequirementsHandler : MonoBehaviour
    {
        [SerializeField] private Requirement _requirementModel;
        [SerializeField] private float _requirementSeparation = 5;

        private float _currentHeight;

        private bool _areAllMet;

        private void Start()
        {
            Clear();
        }

        public void SetRequirements(List<RequirementPoco> requirements, bool allMet)
        {
            _currentHeight = 0f;
            _areAllMet = allMet;

            Clear();

            StartCoroutine(AddRequirementCoroutine(requirements));
        }

        private IEnumerator AddRequirementCoroutine(List<RequirementPoco> requirements)
        {
            foreach (var requirementData in requirements)
            {
                yield return AddRequirement(requirementData);
            }
        }

        private IEnumerator AddRequirement(RequirementPoco requirementData)
        {
            if (requirementData.Description == null)
            {
                yield break;
            }

            Requirement requirement = Instantiate(_requirementModel, transform);
            requirement.transform.localPosition = new Vector3(0, _currentHeight, 0);
            requirement.Text.SetText(requirementData.Description);

            yield return requirement.Text.DelayedSizeUpdate();

            _currentHeight -= requirement.Text.TextHeight;
            _currentHeight -= _requirementSeparation;
        }

        public bool IsFit()
        {
            return _areAllMet;
        }

        public void Clear()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}