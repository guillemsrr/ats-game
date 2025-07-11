// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Utils;

namespace Resume.ScriptableObjects
{
    [Serializable]
    public class StatesListByCountry
    {
        public LocalizedString Country;
        public CityListByState[] States;
    }

    [Serializable]
    public class CityListByState
    {
        public LocalizedString State;
        public LocalizedString[] Cities;
    }

    [CreateAssetMenu(menuName = "Resume/LocationsData")]
    public class LocationsData : ScriptableObject
    {
        public List<StatesListByCountry> Locations;

        public string GetRandomLocation()
        {
            var country = UtilsLibrary.RandomElement(Locations.ToArray());
            var state = UtilsLibrary.RandomElement(country.States);
            return UtilsLibrary.RandomElement(state.Cities).GetLocalizedString() + ", " + state.State.
                GetLocalizedString() + ", " + country.Country.GetLocalizedString();
        }
    }
}