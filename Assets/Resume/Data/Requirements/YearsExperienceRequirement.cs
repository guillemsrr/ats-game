// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using UnityEngine.Localization;
using Utils;

namespace Resume.Data.Requirements
{
    [CreateAssetMenu(menuName = "Resume/Requirements/YearsExperienceRequirement")]
    public class YearsExperienceRequirement : RequirementData
    {
        [Tooltip("Candidate must have at least {0} years of experience")] [SerializeField]
        private LocalizedString[] _yearsDescriptions;

        public override bool CanBeRequired(ResumeData resumeData)
        {
            return resumeData.WorkExperiences.Length > 0;
        }

        public override string GetDescription(ResumeData resumeData, bool isMet)
        {
            int actualYears = CalculateTotalExperienceYears(resumeData);
            int requiredYears;
            if (isMet)
            {
                requiredYears = actualYears;
            }
            else
            {
                requiredYears = actualYears + Random.Range(1, 4);
            }

            LocalizedString template = UtilsLibrary.RandomElement(_yearsDescriptions);
            string description = template.GetLocalizedString(requiredYears.ToString());

            if (string.IsNullOrWhiteSpace(description))
            {
                description = template.TableEntryReference;
            }

            return description;
        }

        private int CalculateTotalExperienceYears(ResumeData resumeData)
        {
            if (resumeData.WorkExperiences == null || resumeData.WorkExperiences.Length == 0)
                return 0;

            int totalYears = 0;

            foreach (var work in resumeData.WorkExperiences)
            {
                int years = Mathf.Max(0, work.YearEnd - work.YearStart);
                totalYears += years;
            }

            return totalYears;
        }
    }
}