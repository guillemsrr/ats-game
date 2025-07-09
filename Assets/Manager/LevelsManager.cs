// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class LevelsManager: MonoBehaviour
    {
        
        [System.Serializable]
        private class LevelData
        {
            public int Level;
            public GameObject LevelObject;
        }

        [SerializeField] private List<LevelData> _levels;
        
        public void StartLevel(int level)
        {
            if (level != 0)
            {
                _levels[0].LevelObject.SetActive(true);
                return;
            }
            
            var levelObject = _levels.Find(x => x.Level == level)?.LevelObject;
            if (levelObject == null)
            {
                return;
            }

            levelObject.SetActive(true);
        }
    }
}