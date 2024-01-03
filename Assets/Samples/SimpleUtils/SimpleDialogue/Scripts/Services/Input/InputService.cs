using UnityEngine.InputSystem;
using DefaultInputActions = Samples.SimpleUtils.SimpleDialogue.Input.DefaultInputActions;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Input
{
    internal class InputService : IInputService
    {
        public InputAction PointerPosition => _inputActions.UI.Point;
        public InputAction Cancel => _inputActions.UI.Cancel;
        public InputAction PointerClick => _inputActions.UI.Click;
        public InputAction Move => _inputActions.Player.Move;

        private readonly DefaultInputActions _inputActions;

        public InputService(DefaultInputActions inputActions)
        {
            _inputActions = inputActions;
            _inputActions.Enable();
        }
    }
}