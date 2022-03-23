using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GFM.Localization
{
    public class LocalizedFactory
    {
        public static ILocalized Create(Component component)
        {
            if (component.TryGetComponent<TMP_Text>(out var tmpText))
                return new LocalizedTMPText(tmpText);

            if (component.TryGetComponent<Text>(out var text))
                return new LocalizedText(text);

            return null;
        }
    }
}