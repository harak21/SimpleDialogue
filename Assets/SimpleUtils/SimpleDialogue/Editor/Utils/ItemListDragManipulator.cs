using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.Utils
{
    internal class ItemListDragManipulator : MouseManipulator
    {
        public event Action<Vector2> OnDragged;
        private bool _isActive;
        private Vector2 _startPos;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        }
        
        private void OnMouseDown(MouseDownEvent evt)
        {
            if (_isActive)
            {
                evt.StopImmediatePropagation();
            }
            else
            {
                if (evt.target is not DraggableListItem &&
                    (evt.target is not VisualElement ve || ve.GetFirstAncestorOfType<DraggableListItem>() is null)) 
                    return;
                
                _startPos = evt.localMousePosition;

                _isActive = true;
                target.CaptureMouse();
                evt.StopPropagation();
            }
        }
        
        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (evt.target is not DraggableListItem view || !_isActive)
                return;

            if (Mathf.Abs(Mathf.Abs(_startPos.x) - Mathf.Abs(evt.localMousePosition.x)) > 20)
            {
                view.AddToClassList("phrasesList__item--moved");
            }
            
            evt.StopPropagation();
        }
        
        private void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.target is not DraggableListItem view || !_isActive)
                return;

            _isActive = false;
            target.ReleaseMouse();
            
            if (Mathf.Abs(Mathf.Abs(_startPos.x) - Mathf.Abs(evt.localMousePosition.x)) > 20)
            {            
                OnDragged?.Invoke(evt.mousePosition);
            }
            
            view.RemoveFromClassList("phrasesList__item--moved");
            evt.StopPropagation();
        }
    }
}