// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;

namespace Resume
{
    public class ResumeData
    {
        public string FullName;
        public string Role;
        public string LocationContact;
        public string Summary;
        public List<string> Skills;
        public List<WorkExperience> WorkExperiences;
        public List<SectionData> Sections;
    }

    public class SectionData
    {
        public string Title;
        public List<Experience> Entries;
    }

    public class WorkExperience
    {
        public string Position;
        public string Company;
        public string YearRange;
        public string Description;
    }

    public class Experience
    {
        public string YearRange;
        public string Description;
    }
}