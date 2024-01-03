using System.Collections.Generic;
using System.Linq;
using Samples.SimpleUtils.SimpleDialogue.Scripts.UI;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario
{
    internal class InteractiveSystem
    {
        private readonly IGameButtonPlace _buttonPlace;
        private readonly Transform _playerTransform;
        private readonly List<InteractiveComponent> _interactiveComponents;
        private CullingGroup _cullingGroup;

        private const float InteractionDistance = 4f;

        private readonly List<InteractiveComponent> _activeComponents = new();

        public InteractiveSystem(IGameButtonPlace buttonPlace,
            Transform playerTransform,
            List<InteractiveComponent> interactiveComponents)
        {
            _buttonPlace = buttonPlace;
            _playerTransform = playerTransform;
            _interactiveComponents = interactiveComponents;

            CreateCullingGroup();

#if UNITY_EDITOR
            Application.quitting += Cleanup;
#endif
        }
        
        public void Cleanup()
        {
            if (_cullingGroup is not null)
            {
                _cullingGroup.Dispose();
            }
        }

        private void CreateCullingGroup()
        {
            BoundingSphere[] spheres = new BoundingSphere[_interactiveComponents.Count];
            for (int i = 0; i < _interactiveComponents.Count; i++)
            {
                spheres[i] = new BoundingSphere(_interactiveComponents[i].InteractionPoint, .01f);
            }
            float[] distances = {InteractionDistance, float.PositiveInfinity};
            _cullingGroup = new CullingGroup {targetCamera = Camera.main};
            _cullingGroup.SetBoundingSpheres(spheres);
            _cullingGroup.SetBoundingSphereCount(_interactiveComponents.Count);
            _cullingGroup.SetDistanceReferencePoint(_playerTransform);
            _cullingGroup.SetBoundingDistances(distances);
            _cullingGroup.onStateChanged = CullingGroupStateChanged;
        }

        private void CullingGroupStateChanged(CullingGroupEvent evt)
        {
            var component = _interactiveComponents[evt.index];
            
            if (evt is {currentDistance: 0, isVisible: true})
            {
                if (component.CanInteract && !_activeComponents.Contains(component))
                {
                    _activeComponents.Add(component);
                    _buttonPlace.SetButtonItems(
                        _activeComponents.Select(c => c.Description).ToList(),
                        InteractiveButtonClicked);
                }
            }

            if (evt.currentDistance != 0 && evt.previousDistance == 0)
            {
                if (_activeComponents.Contains(component))
                {
                    _activeComponents.Remove(component);
                    _buttonPlace.SetButtonItems(
                        _activeComponents.Select(c => c.Description).ToList(),
                        InteractiveButtonClicked);
                }
            }
        }

        private void InteractiveButtonClicked(int i)
        {
            _activeComponents[i].Interact();
        }
    }
}