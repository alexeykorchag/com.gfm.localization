using GFM.Localization.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GFM.Localization
{

    /// <summary>
    /// Localization manager.
    /// </summary>
    public static partial class LocalizationManager
    {
        private static LanguageSettings _settings;
        public static LanguageSettings Settings
        {
            get
            {
                if (_settings == null) _settings = Resources.Load<LanguageSettings>("Settings");
                return _settings;
            }
        }


        public static event Action LanguageChanged = () => { };
        public static event Action FontChanged = () => { };

        public static string CurrentLanguage { get; private set; } = "Russian";
        public static LanguageNode LanguageNode { get; private set; }

        private static Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public static void SetLanguage(string value)
        {
            var prevLang = CurrentLanguage;
            CurrentLanguage = value;

            Read();

            LanguageChanged?.Invoke();

            var sameAsset = Settings.AreSharingSameAsset(prevLang, CurrentLanguage);
            if (!sameAsset)
            {
                FontChanged?.Invoke();
            }
        }


        /// <summary>
        /// Set default language.
        /// </summary>
        public static void AutoLanguage()
        {
            CurrentLanguage = Settings.GetLanguage(Application.systemLanguage);
        }

        /// <summary>
        /// Read localization spreadsheets.
        /// </summary>
        public static void Read()
        {
            LanguageNode = Settings.GetNode(CurrentLanguage);

            var localizationData = Load(CurrentLanguage);

            _dictionary = localizationData.ToDictionary();
        }

        public static LocalizationData Load(string language)
        {
            var localizationData = Resources.Load<LocalizationData>(CurrentLanguage);
            if (localizationData == null)
                throw new KeyNotFoundException("Language not found: " + CurrentLanguage);

            return localizationData;
        }

        public static void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Get localized value by localization key.
        /// </summary>
        public static string Localize(string localizationKey)
        {
            Read();

            if (string.IsNullOrEmpty(localizationKey))
            {
                Debug.LogError("Null key");
                return "";
            }

            if (!_dictionary.TryGetValue(localizationKey, out var translate))
            {
                Debug.LogError("Translation not found: " + localizationKey);
                return localizationKey;
            }

            return translate;
        }

        /// <summary>
        /// Get localized value by localization key.
        /// </summary>
        public static string Localize(string localizationKey, params object[] args)
        {
            var pattern = Localize(localizationKey);
            return string.Format(pattern, args);
        }

    }
}