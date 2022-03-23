using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;

namespace GFM.Localization
{
    public static class SearchUsingSymbols
    {

        private const string menuPath = "Tools/Localization/Search Using Symbols";

        private static string charactersPathFolder => $"{Application.dataPath}/Localization/UsingSymbols/";
        private static string charactersFileFormat => "{0}_Characters.txt";
        private static string defaultSymbolsFile => "DefaultSymbols.txt";

        [MenuItem(menuPath, priority = -1)]
        public static void SearchSymbols()
        {
            var defaultSymbols = ReadDefaultSymbols();

            var fontsChars = new Dictionary<TMP_FontAsset, List<string>>();

            var fontSettings = LocalizationManager.Settings;
            foreach (var fontNode in fontSettings.Nodes)
            {
                var localizationData = LocalizationManager.Load(fontNode.Name);
                var translations = localizationData.ToValue();
                translations.Add(defaultSymbols);

                // Получаем список всех используемых символов
                var chars = GetUsingChars(translations);

                // Переводим симолы в коды
                var charsCode = GetCharCode(chars);

                // Добавляем символы в словарь по шрифту
                if (fontsChars.TryGetValue(fontNode.Font, out var list))
                {
                    foreach (var charCode in charsCode)
                    {
                        if (!list.Contains(charCode))
                            list.Add(charCode);
                    }

                    fontsChars[fontNode.Font] = list;
                }
                else
                {
                    fontsChars.Add(fontNode.Font, charsCode);
                }
            }


            if (!Directory.Exists(charactersPathFolder))
                Directory.CreateDirectory(charactersPathFolder);

            foreach (var item in fontsChars)
            {
                item.Value.Sort();
                var charList = item.Value.Distinct();

                var chars = string.Join("", charList);
                var path = Path.Combine(charactersPathFolder, string.Format(charactersFileFormat, item.Key.name));
                System.IO.File.WriteAllText(path, chars);
            }

            AssetDatabase.Refresh();
        }

        private static string ReadDefaultSymbols()
        {
            var path = Path.Combine(charactersPathFolder, defaultSymbolsFile);

            if (!File.Exists(path))
            {
                Debug.LogError($"File not fount {path}");
                return "";
            }

            return File.ReadAllText(path);
        }

        private static List<char> GetUsingChars(List<string> translations)
        {
            var chars = new List<char>();

            foreach (var translation in translations)
            {
                foreach (var symbol in translation)
                {
                    if (!chars.Contains(symbol))
                        chars.Add(symbol);
                }
            }

            return chars;
        }

        private static List<string> GetCharCode(List<char> chars)
        {
            var charsCode = new List<string>();

            foreach (var symbol in chars)
            {
                try
                {
                    if (Char.IsLetter(symbol))
                    {
                        charsCode.Add(Char.ToLowerInvariant(symbol).ToString());
                        charsCode.Add(Char.ToUpperInvariant(symbol).ToString());
                    }
                    else
                    {
                        charsCode.Add(symbol.ToString());
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(symbol + " " + e.ToString());
                }
            }

            return charsCode;

        }

    }

}