using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.Utils
{
    internal class DraggableListItem : VisualElement
    {
        public event Action<long, Guid, Vector2> OnNodeCreate;

        private Guid _guidKey;
        private long _longKey;

        public DraggableListItem(VisualTreeAsset template)
        {
            template.CloneTree(this);
            var dragManipulator = new ItemListDragManipulator();
            dragManipulator.OnDragged += (pos) => OnNodeCreate?.Invoke(_longKey, _guidKey, pos);
            this.AddManipulator(dragManipulator);
        }

        public void Bind(long longKey, Guid guidKey, string title)
        {
            _guidKey = guidKey;
            _longKey = longKey;
            this.Q<Label>().text = title;
        }
    }
}