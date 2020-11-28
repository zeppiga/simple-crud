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
            var types = _typesProvider.GetTypes();
            var singletonTypes = GetSingletons(types);

            foreach (var (toImplement, implementation) in singletonTypes)
            {
                register(toImplement, implementation);
            }
        }

        private static IEnumerable<(Type toImplement, Type implementation)> GetSingletons(IEnumerable<TypeInfo> types)
        {
            foreach (var type in types)
            {
                if (!(type.GetCustomAttribute(typeof(SingletonAttribute)) is SingletonAttribute attribute)) continue;

                if (!type.IsClass) throw new RegistrationException(type);
                if (!type.ImplementedInterfaces.Contains(attribute.ExportType) && !attribute.ExportType.IsAssignableFrom(type)) throw new RegistrationException(type);

                var result = (attribute.ExportType, type);
                yield return result;
            }
        }
    }
}