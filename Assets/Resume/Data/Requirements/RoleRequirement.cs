// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using Resume.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using Utils;

namespace Resume.Data.Requirements
{
    [CreateAssetMenu(menuName = "Resume/Requirements/RoleRequirement")]
    public class RoleRequirement: RequirementData
    {
        [Tooltip("Candidate must be a {0}")] [SerializeField]
        private LocalizedString[] _roleDescriptions;

        public override bool CanBeRequired(ResumeData resumeData)
        {
            return !string.IsNullOrEmpty(resumeData.Role);
        }

        public override string GetDescription(ResumeData resumeData, bool isMet)
        {
            List<string> roles = new List<string>();
            if (!string.IsNullOrEmpty(resumeData.Role))
            {
                roles.Add(resumeData.Role);
            }

            LocalizedString requirementDescription = UtilsLibrary.RandomElement(_roleDescriptions);

            string role;
            if (isMet && roles.Count > 0)
            {
                role = UtilsLibrary.RandomElement(roles);
            }
            else
            {
                role = GetMissingRole(resumeData, roles);
            }

            string description = requirementDescription.GetLocalizedString(role);
            if (string.IsNullOrWhiteSpace(description))
            {
                description = requirementDescription.TableEntryReference;
            }

            return description;
        }

        private string GetMissingRole(ResumeData resumeData, List<string> existingRoles)
        {
            JobArchetype jobArchetype = resumeData.JobArchetype;
            string role;
            do
            {
                role = UtilsLibrary.RandomElement(jobArchetype.JobTitles);
            }
            while (existingRoles.Contains(role));

            return role;
        }
    }
}