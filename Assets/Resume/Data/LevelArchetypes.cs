// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using GameJamBase.Utils;
using Resume.Data.Requirements;
using UnityEngine;

namespace Resume.Data
{
    [CreateAssetMenu(menuName = "Resume/LevelArchetypes")]
    public class LevelArchetypes : ScriptableObject
    {
        [SerializeField] public List<LevelArchetype> _levelArchetypes;

        public LevelArchetype GetRandomArchetype(int level)
        {
            //forcing level atm, TODO design refactor 
            LevelArchetype levelArchetype = _levelArchetypes[0];
            levelArchetype.Level = level;
            return levelArchetype;

            /*List<LevelArchetype> matchingArchetypes = _levelArchetypes.Where(a => a.Level == level).ToList();
            if (matchingArchetypes.Count == 0)
            {
                return _levelArchetypes[Random.Range(0, _levelArchetypes.Count)];
            }

            return matchingArchetypes[Random.Range(0, matchingArchetypes.Count)];*/
        }
    }

    [System.Serializable]
    public class LevelArchetype
    {
        public int Level;

        [SerializeField] private JobArchetype[] _jobsDatas;
        [SerializeField] private RequirementData[] _requirements;

        public JobArchetype GetJobArchetype()
        {
            return _jobsDatas[Random.Range(0, _jobsDatas.Length)];
        }

        public List<RequirementData> GetRandomRequirements(ResumeData resumeData, bool allMet = true)
        {
            List<RequirementData> requirements = new List<RequirementData>();
            //requirements.Add(_requirements[2]);
            //return requirements;
            
            int min = Mathf.Clamp(Level - 1, 1, 2);
            int max = Mathf.Clamp(Level + 1, 2, 5);
            int numberRequirements = Random.Range(min, max);

            List<RequirementData> requirementsPool = new List<RequirementData>(_requirements);
            int attempts = 0;

            while (requirements.Count < numberRequirements && requirementsPool.Count > 0 && attempts < 100)
            {
                RequirementData randomReq = UtilsLibrary.RandomElement(requirementsPool);
                if (randomReq == null)
                {
                    Debug.LogWarning("Skipped null requirement");
                    requirementsPool.Remove(randomReq);
                    continue;
                }

                if (!allMet || randomReq.CanBeRequired(resumeData))
                {
                    requirements.Add(randomReq);
                }

                requirementsPool.Remove(randomReq);
                attempts++;
            }

            return requirements;
        }
    }
}