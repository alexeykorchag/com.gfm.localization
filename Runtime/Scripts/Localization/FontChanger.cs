using UnityEngine;

namespace GFM.Localization
{
    public class FontChanger : MonoBehaviour
    {
        private ILocalized _localized;

        private void Start()
        {
            OnFontChanged();
            LocalizationManager.FontChanged += OnFontChanged;
        }

        private void OnDestroy()
        {
            LocalizationManager.FontChanged -= OnFontChanged;
        }

        private void Initialize()
        {
            if (_localized != null) return;
            _localized = LocalizedFactory.Create(this);
        }

        private void OnFontChanged()
        {
            var fontAsset = LocalizationManager.LanguageNode.Font;
            if (fontAsset == null) return;

            Initialize(); 
            _localized.SetFont(fontAsset);
        }
    }
}