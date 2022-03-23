using TMPro;
using UnityEngine;

namespace GFM.Localization.Data
{
    [System.Serializable]
    public partial class LanguageNode
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private string _code;

        [SerializeField]
        private SystemLanguage[] _system;

        [SerializeField]
        private TMP_FontAsset _font;


        public string Name => _name;
        public string Code => _code;
        public SystemLanguage[] System => _system;
        public TMP_FontAsset Font => _font;
    }
}