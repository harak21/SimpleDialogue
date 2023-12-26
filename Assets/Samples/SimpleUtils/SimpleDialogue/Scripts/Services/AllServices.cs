using System;
using System.Collections.Generic;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services
{
    public static class AllServices
    {
        private static readonly Dictionary<Type, IService> Container = new();

        public static void Register<TService>(TService service) where TService : IService
        {
            Container.Add(typeof(TService), service);
        }

        public static TService Get<TService>() where TService : IService
        {
            return (TService)Container[typeof(TService)];
        }
    }
}