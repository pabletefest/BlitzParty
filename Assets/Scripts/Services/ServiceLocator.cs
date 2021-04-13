using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Services
{
    public class ServiceLocator
    {
        private static ServiceLocator instance;
        public static ServiceLocator Instance => instance ?? (instance = new ServiceLocator());

        private readonly Dictionary<Type, object> services;

        private ServiceLocator()
        {
            services = new Dictionary<Type, object>();
        }

        public void RegisterService<T>(T service)
        {
            Type type = typeof(T);
            Assert.IsFalse(services.ContainsKey(type), $"Service {type} already registered.");

            services.Add(type, service);
        }

        public T GetService<T>()
        {
            Type type = typeof(T);

            if(!services.TryGetValue(type, out object service))
            {
                throw new Exception($"Service {type} not found.");
            }

            return (T) service;
        }

        public void UnregisterService<T>()
        {
            Type type = typeof(T);

            Assert.IsFalse(!services.ContainsKey(type), $"Service {type} can not be removed if it is not registered.");

            services.Remove(type);
        }

        public void RemoveRegisteredServices()
        {
            services.Clear();
        }
    }
}