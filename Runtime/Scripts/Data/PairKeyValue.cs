using System;
using UnityEngine;

namespace GFM.Localization.Data
{
    [Serializable]
    public class PairKeyValue
    {
        [SerializeField]
        private string _key;

        [SerializeField]
        private string _value;

        public string Key => _key;
        public string Value => _value;

        public PairKeyValue(string key, string value)
        {
            _key = key;
            _value = value;
        }
    }

}