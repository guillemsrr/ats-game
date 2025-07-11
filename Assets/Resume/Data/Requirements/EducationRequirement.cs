// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using Resume.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using Utils;

namespace Resume.Data.Requirements
{
    [CreateAssetMenu(menuName = "Resume/Requirements/EducationRequirement")]
    public class EducationRequirement : RequirementData
    {
        [Tooltip("Templates like 'Candidate must have studied {0}'")] [SerializeField]
        private LocalizedString[] _educationDescriptions;

        public override string GetDescription(ResumeData resumeData, bool isMet)
        {
            List<string> degrees = new List<string>();
            foreach (EducationExperience resumeDataEducationExperience in resumeData.EducationExperiences)
            {
                degrees.Add(resumeDataEducationExperience.Degree);
            }

            LocalizedString requirementDescription = UtilsLibrary.RandomElement(_educationDescriptions);

            string degree;
            if (isMet)
            {
                degree = UtilsLibrary.RandomElement(degrees);
            }
            else
            {
                degree = GetMissingDegree(resumeData, degrees);
            }

            string description = requirementDescription.GetLocalizedString(degree);
            if (string.IsNullOrWhiteSpace(description))
            {
                description = requirementDescription.TableEntryReference;
            }

            return description;
        }

        private string GetMissingDegree(ResumeData resumeData, List<string> degrees)
        {
            string degree = "";
            do
            {
                EducationData educationData = UtilsLibrary.RandomElement(resumeData.JobArchetype.EducationData);
                degree = UtilsLibrary.RandomElement(educationData.Degrees);
            }
            while (degrees.Contains(degree));

            return degree;
        }
    }
}