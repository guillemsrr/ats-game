// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using UnityEngine.Localization;

namespace Resume.Data.Requirements
{
    [CreateAssetMenu(menuName = "Resume/Requirements/YearsExperienceRequirement")]
    public class YearsExperienceRequirement : RequirementData
    {
        public override string GetDescription(ResumeData resumeData, bool isMet)
        {
            return "years experience";
        }
    }
}