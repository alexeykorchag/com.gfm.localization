using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GFM.Localization.Data
{
    [Serializable]
    public class LocalizationData : ScriptableObject
    {
        [SerializeField]
        private PairKeyValue[] _keyValuePairs;

        public void Fill(PairKeyValue[] keyValuePairs)
        {
            _keyValuePairs = keyValuePairs;
        }

        public Dictionary<string, string> ToDictionary() => _keyValuePairs.ToDictionary(x => x.Key, x => x.Value);
        public List<string> ToValue() => _keyValuePairs.Select(x => x.Value).ToList();
    }
}