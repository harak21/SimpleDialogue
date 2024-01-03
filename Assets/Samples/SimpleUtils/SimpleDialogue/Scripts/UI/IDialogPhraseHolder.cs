using UnityEngine.UIElements;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.UI
{
    internal interface IDialogPhraseHolder
    {
        public void SetLabelText(string text);
        public void Clear();
    }
}