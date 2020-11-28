using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace simple_crud.ServicesRegistration
{
    public interface ITypesProvider
    {
        IEnumerable<TypeInfo> GetTypes();
    }

    public sealed class DefaultTypesProvider : ITypesProvider
    {
        public IEnumerable<TypeInfo> GetTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.StartsWith("simple-crud") ?? false).SelectMany(x => x.DefinedTypes);

            return types;
        }
    }
}