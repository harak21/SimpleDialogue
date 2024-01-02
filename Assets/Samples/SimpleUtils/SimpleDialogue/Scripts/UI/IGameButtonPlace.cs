using System;
using System.Collections.Generic;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.UI
{
    internal interface IGameButtonPlace
    {
        void SetButtonItems(List<string> labels, Action<int> callback);
        void Clear();
    }
}