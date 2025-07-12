// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<string> GetRandomLocation()
        {
            var country = UtilsLibrary.RandomElement(Locations.ToArray());
            var state = UtilsLibrary.RandomElement(country.States);
            var city = UtilsLibrary.RandomElement(state.Cities);

            var cityOp = city.GetLocalizedStringAsync();
            var stateOp = state.State.GetLocalizedStringAsync();
            var countryOp = country.Country.GetLocalizedStringAsync();

            await Task.WhenAll(cityOp.Task, stateOp.Task, countryOp.Task);

            return $"{cityOp.Result}, {stateOp.Result}, {countryOp.Result}";
        }
    }
}