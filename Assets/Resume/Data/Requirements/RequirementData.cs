// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;

namespace Resume.Data.Requirements
{
    public abstract class RequirementData : ScriptableObject
    {
        public abstract string GetDescription(ResumeData resumeData, bool isMet);

        public virtual bool CanBeRequired(ResumeData resumeData)
        {
            return false;
        }
    }
}