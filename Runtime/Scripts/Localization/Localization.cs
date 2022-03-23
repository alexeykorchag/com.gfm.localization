using UnityEngine;

namespace GFM.Localization
{
    /// <summary>
    /// Localize text component.
    /// </summary>
    [RequireComponent(typeof(FontChanger))]
    public class Localization : MonoBehaviour
    {
        [SerializeField]
        private string _key;

        private ILocalized _localized;

        private void Start()
        {
            Localize();
            LocalizationManager.LanguageChanged += Localize;
        }

        private void OnDestroy()
        {
            LocalizationManager.LanguageChanged -= Localize;
        }

        private void Initialize()
        {
            if (_localized != null) return;
            _localized = LocalizedFactory.Create(this);
        }

        private void Localize()
        {
            if (string.IsNullOrEmpty(_key)) return;

            Initialize();
            _localized.SetText(LocalizationManager.Localize(_key));
        }
    }

}