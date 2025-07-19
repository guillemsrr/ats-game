// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameJamBase.Utils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Resume.Data.Requirements
{
    [CreateAssetMenu(menuName = "Resume/Requirements/SkillRequirement")]
    public class SkillRequirement : RequirementData
    {
        [Tooltip("Candidate must know {0}")] [SerializeField]
        private LocalizedString[] _skillDescriptions;

        public override bool CanBeRequired(ResumeData resumeData)
        {
            return resumeData.Skills.Length > 0;
        }

        public override async Task<string> GetDescription(ResumeData resumeData, bool isMet)
        {
            string[] existingSkills = resumeData.Skills;

            LocalizedString requirementDescription = UtilsLibrary.RandomElement(_skillDescriptions);

            string skill;
            if (isMet && existingSkills.Length > 0)
            {
                skill = UtilsLibrary.RandomElement(existingSkills);
            }
            else
            {
                skill = GetMissingSkill(resumeData, existingSkills.ToList());
            }

            AsyncOperationHandle<string> localizedStringAsync = requirementDescription.GetLocalizedStringAsync(skill);
            await localizedStringAsync.Task;

            if (string.IsNullOrWhiteSpace(localizedStringAsync.Result))
            {
                return requirementDescription.TableEntryReference;
            }

            return localizedStringAsync.Result;
        }

        private string GetMissingSkill(ResumeData resumeData, List<string> existingSkills)
        {
            List<string> potentialSkills = new List<string>();

            if (resumeData.JobArchetype != null)
            {
                potentialSkills.AddRange(resumeData.JobArchetype.Skills);
            }

            potentialSkills = potentialSkills
                .Where(skill => !existingSkills.Contains(skill))
                .Distinct()
                .ToList();

            return UtilsLibrary.RandomElement(potentialSkills);
        }
    }
}