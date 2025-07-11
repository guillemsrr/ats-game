// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Resume.Data.Requirements
{
    public abstract class RequirementData: ScriptableObject
    {
        public LocalizedString Description;

        public virtual string GetDescription(ResumeData resumeData, bool isMet)
        {
            return "Description";
        }
    }
}