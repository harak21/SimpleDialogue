using UnityEngine.InputSystem;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Input
{
    internal interface IInputService : IService
    {
        InputAction PointerPosition { get; }
        InputAction Cancel { get; }
        InputAction PointerClick { get; }
        InputAction Move { get; }
    }
}