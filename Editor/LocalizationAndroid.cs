using System.IO;
using UnityEditor;
using UnityEngine;

namespace GFM.Localization.Android
{
    public class LocalizationAndroid
    {
        private const string androidFolderPath = "Localization/Plugins/Android/res";



        [MenuItem("Tools/Localization/UpdateFolders")]
        public static void UpdateFolders()
        {
            var fullPath = Path.Combine(Application.dataPath, androidFolderPath);

            CreateOrClearDirectory(fullPath);

            var codes = LocalizationManager.Settings.GetCodes();
            CreateFiles(fullPath, codes);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateOrClearDirectory(string fullPath)
        {
            if (Directory.Exists(fullPath))
            {
                var files = Directory.GetFiles(fullPath);
                foreach (var file in files)
                    File.Delete(file);
            }
            else
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        private static void CreateFiles(string fullPath, string[] codes)
        {
            var fileText = GetFileText();

            foreach (var code in codes)
            {
                var filePath = $"{fullPath}/values-{code}.xml";
                File.WriteAllText(filePath, fileText);
            }
        }

        private static string GetFileText() => $"<resources>" + $"\n	<string name=\"app_name\">{Application.productName}</string>" + $"\n</resources>";


    }
}
