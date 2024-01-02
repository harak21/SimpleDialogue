namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene
{
    internal interface ISceneUpdateLoop : IService
    {
        void Register(IUpdatable updatable);
        void Clear();
    }
}