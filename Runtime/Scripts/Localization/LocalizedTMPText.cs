using TMPro;

namespace GFM.Localization
{
    public class LocalizedTMPText : ILocalized
    {
        private TMP_Text _text;

        public LocalizedTMPText(TMP_Text text)
        {
            _text = text;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetFont(TMP_FontAsset fontAsset)
        {
            _text.font = fontAsset;
        }
    }
}