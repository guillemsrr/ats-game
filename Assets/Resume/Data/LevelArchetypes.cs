// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using Resume.Data.Requirements;
using UnityEngine;

namespace Resume.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Resume/LevelArchetypes")]
    public class LevelArchetypes : ScriptableObject
    {
        [SerializeField] public List<LevelArchetype> _levelArchetypes;

        public LevelArchetype GetRandomArchetype(int level)
        {
            List<LevelArchetype> matchingArchetypes = _levelArchetypes.Where(a => a.Level == level).ToList();
            if (matchingArchetypes.Count == 0)
            {
                return _levelArchetypes[Random.Range(0, _levelArchetypes.Count)];
            }

            return matchingArchetypes[Random.Range(0, matchingArchetypes.Count)];
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

        public List<RequirementData> GetRandomRequirements()
        {
            List<RequirementData> requirements = new List<RequirementData>();
            requirements.Add(_requirements[0]);
            return requirements;

            int count = Random.Range(1, Level + 1);
            count = _requirements.Length;
            requirements.AddRange(_requirements.OrderBy(x => Random.value).Take(count));

            return requirements;
        }
    }
}