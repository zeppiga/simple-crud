using System;

namespace simple_crud.ServicesRegistration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class SingletonAttribute : Attribute
    {
        public Type ExportType { get; }


        public SingletonAttribute(Type exportType)
        {
            ExportType = exportType;
        }
    }
}