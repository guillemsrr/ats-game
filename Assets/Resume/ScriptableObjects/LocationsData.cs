// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resume.ScriptableObjects
{
    [Serializable]
    public class StatesListByCountry
    {
        public string Country;
        public CityListByState[] States;
    }

    [Serializable]
    public class CityListByState
    {
        public string State;
        public string[] Cities;
    }

    [CreateAssetMenu(menuName = "ResumeGen/LocationsData")]
    public class LocationsData : ScriptableObject
    {
        public List<StatesListByCountry> Locations;
    }
}