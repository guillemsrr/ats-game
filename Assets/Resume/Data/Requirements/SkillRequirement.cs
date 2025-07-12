// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using Utils;

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

        public override string GetDescription(ResumeData resumeData, bool isMet)
        {
            List<string> existingSkills = resumeData.Skills
                .Split(',')
                .Select(skill => skill.Trim())
                .Where(skill => !string.IsNullOrEmpty(skill))
                .ToList();

            LocalizedString requirementDescription = UtilsLibrary.RandomElement(_skillDescriptions);

            string skill;
            if (isMet && existingSkills.Count > 0)
            {
                skill = UtilsLibrary.RandomElement(existingSkills);
            }
            else
            {
                skill = GetMissingSkill(resumeData, existingSkills);
            }

            string description = requirementDescription.GetLocalizedString(skill);
            if (string.IsNullOrWhiteSpace(description))
            {
                description = requirementDescription.TableEntryReference;
            }

            return description;
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