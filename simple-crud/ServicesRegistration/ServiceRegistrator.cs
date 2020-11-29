using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace simple_crud.ServicesRegistration
{
    public sealed class ServiceRegistrator
    {
        private readonly ITypesProvider _typesProvider;

        public ServiceRegistrator(ITypesProvider typesProvider)
        {
            _typesProvider = typesProvider;
        }

        public void RegisterSingletons(Action<Type,Type> register)
        {
            Register<SingletonAttribute>(register);
        }

        public void RegisterTransient(Action<Type, Type> register)
        {
            Register<TransientAttribute>(register);
        }

        private void Register<T>(Action<Type, Type> register) where T : RegistrationAttribute
        {
            var types = _typesProvider.GetTypes();
            var typesToRegister = GetTypes<T>(types);

            foreach (var (toImplement, implementation) in typesToRegister)
            {
                register(toImplement, implementation);
            }
        }


        private static IEnumerable<(Type toImplement, Type implementation)> GetTypes<T>(IEnumerable<TypeInfo> types) where T : RegistrationAttribute
        {
            foreach (var type in types)
            {
                if (!(type.GetCustomAttribute(typeof(T)) is T attribute)) continue;

                if (!type.IsClass) throw new RegistrationException(type);
                if (!type.ImplementedInterfaces.Contains(attribute.ExportType) && !attribute.ExportType.IsAssignableFrom(type)) throw new RegistrationException(type);

                var result = (attribute.ExportType, type);
                yield return result;
            }
        }
    }
}