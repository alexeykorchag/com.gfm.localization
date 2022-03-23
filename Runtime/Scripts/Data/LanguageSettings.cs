using System.Linq;
using UnityEngine;

namespace GFM.Localization.Data
{
    [CreateAssetMenu(fileName = "Settings", menuName = "GFM/Localization/Settings")]
    public class LanguageSettings : ScriptableObject
    {
        [SerializeField]
        private LanguageNode[] _nodes;


        public LanguageNode[] Nodes => _nodes;

        public LanguageNode DefaultNode => _nodes[0];

        public bool AreSharingSameAsset(string a, string b) => AreSharingSameAsset(GetNode(a), GetNode(b));
        public bool AreSharingSameAsset(LanguageNode a, LanguageNode b) => a.Font == b.Font;

        public LanguageNode GetNode(string language)
        {
            foreach (var fontNode in _nodes)
            {
                if (fontNode.Name == language)
                {
                    return fontNode;
                }
            }

            return DefaultNode;
        }

        public string GetLanguage(SystemLanguage systemLanguage)
        {
            foreach (var node in _nodes)
            {
                foreach (var language in node.System)
                    if (language == systemLanguage)
                        return node.Name;
            }

            return DefaultNode.Name;
        }

        public string[] GetNames() => _nodes
                            .Select(x => x.Name.Trim())
                            .Where(x => !string.IsNullOrEmpty(x))
                            .ToArray();

        public string[] GetCodes() => _nodes
                                    .Select(x => x.Code.Trim())
                                    .Where(x => !string.IsNullOrEmpty(x))
                                    .ToArray();
    }

}