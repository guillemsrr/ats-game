using UnityEngine.Localization;

namespace Resume.Data.Requirements
{
    public struct RequirementPoco
    {
        public string Description;

        public RequirementPoco(string description)
        {
            Description = description;
        }
    }
}