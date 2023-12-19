using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.Utils
{
    public class DraggableItem : VisualElement
    {
        public event Action<Vector2> OnItemDropped;

        public DraggableItem(VisualElement parent)
        {
            parent.Add(this);
            this.StretchToParentSize();
            var dragManipulator = new DragManipulator();
            dragManipulator.OnDragged += pos => OnItemDropped?.Invoke(pos);
            this.AddManipulator(dragManipulator);
            tooltip = "Drag and drop";
        }
    }
}