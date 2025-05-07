using System;
using System.Collections.Generic;

namespace LinkThemAll.Services
{
    public class ServiceProvider
    {
        private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public static T Get<T>() where T : IService
        {
            if (_services.ContainsKey(typeof(T)))
            {
                return (T) _services[typeof(T)];
            }
            
            return default;
        }

        public static void Add<T>(T service) where T : IService
        {
            if (!_services.ContainsKey(typeof(T)))
            {
                _services.Add(typeof(T), service);
            }
        }
    }
}