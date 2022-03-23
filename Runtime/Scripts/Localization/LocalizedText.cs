using TMPro;
using UnityEngine.UI;

namespace GFM.Localization
{
    public class LocalizedText : ILocalized
    {
        private Text _text;

        public LocalizedText(Text text)
        {
            _text = text;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetFont(TMP_FontAsset fontAsset)
        {
            _text.font = fontAsset.sourceFontFile;
        }
    }
}