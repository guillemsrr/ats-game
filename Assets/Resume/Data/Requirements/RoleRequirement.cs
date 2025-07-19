// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameJamBase.Utils;
using Resume.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

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

        public override async Task<string> GetDescription(ResumeData resumeData, bool isMet)
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

            AsyncOperationHandle<string> description = requirementDescription.GetLocalizedStringAsync(role);
            await description.Task;
            
            if (string.IsNullOrWhiteSpace(description.Result))
            {
                return requirementDescription.TableEntryReference;
            }

            return description.Result;
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