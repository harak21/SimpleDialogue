using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Input;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Character
{
    internal class CharacterController : IUpdatable
    {
        private readonly IInputService _inputService;
        private readonly NavMeshAgent _agent;

        private float _speed = 10f;
        private bool _hasMovementValue;
        private Vector3 _offset;

        public CharacterController(IInputService inputService, NavMeshAgent agent)
        {
            _inputService = inputService;
            _agent = agent;

            inputService.Move.started += MoveStarted;
            inputService.Move.canceled += MoveCanceled;
        }

        private void MoveCanceled(InputAction.CallbackContext obj)
        {
            _hasMovementValue = false;
        }

        private void MoveStarted(InputAction.CallbackContext callbackContext)
        {
            _hasMovementValue = true;
        }

        public void OnUpdate()
        {
            if (!_hasMovementValue)
                return;
            
            var direction = _inputService.Move.ReadValue<Vector2>();
            var offset = Time.deltaTime * _speed * direction;
            _offset.x = offset.x;
            _offset.z = offset.y;
            _agent.Move(_offset);
        }
    }
}