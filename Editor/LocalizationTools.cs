using GFM.Localization.Data;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using ExportLib.Exporters;
using System.Collections.Generic;
using System.Linq;
#endif

[CreateAssetMenu(fileName = "Tools", menuName = "GFM/Localization/Tools")]
public class LocalizationTools : ScriptableObject
{
    [SerializeField]
    private LanguageSettings _localizationSettings;

    [SerializeField]
    private string _localizationGoogleFileID = "";

    [SerializeField]
    private string[] _sheets;

#if UNITY_EDITOR
    public void ExportLocalizationFromWeb()
    {
        Debug.Log("<color=yellow>Sync started, please wait for confirmation message...</color>");

        GoogleDriveExporter.Export(_localizationGoogleFileID, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            (data) =>
            {
                Debug.Log("Success handle data " + data.Length);

                var localizationDictionary = ImportLocaliztion(data);
                var localizationPairKeyValue = ImportLocaliztion(localizationDictionary);

                Save(localizationPairKeyValue);

                Debug.Log("<color=green>Localization successfully synced!</color>");
            });
    }

    private Dictionary<string, Dictionary<string, string>> ImportLocaliztion(byte[] data)
    {
        var dictionary = new Dictionary<string, Dictionary<string, string>>();
        var keys = _localizationSettings.GetNames();
        foreach (var locKey in keys)
        {

            if (!dictionary.ContainsKey(locKey))
                dictionary.Add(locKey, new Dictionary<string, string>());

            var defs = GetDefsFromBytes(data, locKey);
            var container = defs.ElementAt(0);

            foreach (Dictionary<string, string> s in container.Data)
            {
                foreach (var v in s)
                {
                    dictionary[locKey].Add(v.Key, v.Value);
                }
            }
        }

        return dictionary;
    }

    private IEnumerable<DefCollection> GetDefsFromBytes(byte[] odsByteArr, string locKey)
    {
        IEnumerable<DefCollection> defCollections;

        var _exporter = new LocalizationExporter("", locKey);
        _exporter.ParseOdsFile(odsByteArr, _sheets, out defCollections);
        return defCollections;
    }

    private Dictionary<string, PairKeyValue[]> ImportLocaliztion(Dictionary<string, Dictionary<string, string>> dictionary)
    {
        var result = new Dictionary<string, PairKeyValue[]>();

        var defaultKeyValuePairs = dictionary[_localizationSettings.DefaultNode.Name];
        foreach (var item in dictionary)
        {
            var list = new List<PairKeyValue>();

            foreach (var keyValuePair in item.Value)
            {
                var key = keyValuePair.Key;
                key = key.Trim();
                if (string.IsNullOrEmpty(key)) continue;
                
                var value = !string.IsNullOrEmpty(keyValuePair.Value) ? keyValuePair.Value : defaultKeyValuePairs[key];
                value = value.Trim();
                if (string.IsNullOrEmpty(value)) continue;

                list.Add(new PairKeyValue(key, value));
            }

            list.Sort((x1, x2) => { return x1.Key.CompareTo(x2.Key); });

            result.Add(item.Key, list.ToArray());
        }

        return result;
    }

    private void Save(Dictionary<string, PairKeyValue[]> dictionary)
    {
        foreach (var item in dictionary)
        {
            var path = $"Assets/Localization/Resources/{item.Key}.asset";

            var data = AssetDatabase.LoadAssetAtPath<LocalizationData>(path);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<LocalizationData>();
                AssetDatabase.CreateAsset(data, path);
            }

            data.Fill(item.Value);
            EditorUtility.SetDirty(data);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

#endif
}