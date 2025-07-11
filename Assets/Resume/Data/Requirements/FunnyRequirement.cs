// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Resume.Data.Requirements
{
    [CreateAssetMenu(menuName = "Resume/Requirements/FunnyRequirement")]
    public class FunnyRequirement: RequirementData
    {
        public override string GetDescription(ResumeData resumeData, bool isMet)
        {
            return "Funny";
        }
    }
}