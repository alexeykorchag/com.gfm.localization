using TMPro;

namespace GFM.Localization
{
    public interface ILocalized
    {
        void SetText(string text);

        void SetFont(TMP_FontAsset fontAsset);
    }
}