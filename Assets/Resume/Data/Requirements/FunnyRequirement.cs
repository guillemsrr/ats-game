// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.Localization;
using Utils;

namespace Resume.Data.Requirements
{
    [CreateAssetMenu(menuName = "Resume/Requirements/FunnyRequirement")]
    public class FunnyRequirement : RequirementData
    {
        [Tooltip("Candidate must have dealt with {0}")] [SerializeField]
        private LocalizedString[] _funnyDescriptions;

        public override bool CanBeRequired(ResumeData resumeData)
        {
            return resumeData.SectionTypes.Contains(ResumeSectionType.Experience);
        }

        public override string GetDescription(ResumeData resumeData, bool isMet)
        {
            // Pick a description template
            LocalizedString descriptionTemplate = UtilsLibrary.RandomElement(_funnyDescriptions);
            LocalizedString keyElement = null;

            if (isMet)
            {
                if (resumeData.FunnyExperience == null)
                {
                    //TODO: handle problem
                    Debug.LogError("null funny experience");
                }
                else
                {
                    keyElement = resumeData.FunnyExperience.KeyElement;
                }
            }
            else
            {
                keyElement = GetMissingFunny(resumeData);
            }

            if (keyElement != null)
            {
                string result = descriptionTemplate.GetLocalizedString(keyElement.GetLocalizedString());
                if (string.IsNullOrWhiteSpace(result))
                {
                    result = descriptionTemplate.TableEntryReference;
                }

                return result;
            }

            return null;
        }

        private LocalizedString GetMissingFunny(ResumeData resumeData)
        {
            LocalizedString value;
            do
            {
                value = UtilsLibrary.RandomElement(resumeData.JobArchetype.FunnyExperiences.KeyElement);
            }
            while (value == resumeData.FunnyExperience.KeyElement);

            return value;
        }
    }
}