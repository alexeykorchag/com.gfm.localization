#if UNITY_EDITOR && UNITY_IOS
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace GFM.Localization.iOS
{
    public class LocalizationPostBuildProcess
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            var infoPList = System.IO.Path.Combine(path, "Info.plist");
            if (!File.Exists(infoPList))
            {
                Debug.LogError("Could not add localizations to Info.plist file.");
                return;
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(infoPList);

            var pListDictionary = xmlDocument.SelectSingleNode("plist/dict");
            if (pListDictionary == null)
            {
                Debug.LogError("Could not add localizations to Info.plist file.");
                return;
            }

            var kProjectLocalizations = LocalizationManager.Settings.GetCodes();

            var localizationsKey = xmlDocument.CreateElement("key");
            localizationsKey.InnerText = "CFBundleLocalizations";
            pListDictionary.AppendChild(localizationsKey);

            var localizationsArray = xmlDocument.CreateElement("array");
            foreach (string localization in kProjectLocalizations)
            {
                var localizationElement = xmlDocument.CreateElement("string");
                localizationElement.InnerText = localization;
                localizationsArray.AppendChild(localizationElement);
            }

            pListDictionary.AppendChild(localizationsArray);

            xmlDocument.Save(infoPList);

        }
    }
}
#endif