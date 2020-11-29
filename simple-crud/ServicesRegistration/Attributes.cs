using System;

namespace simple_crud.ServicesRegistration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class SingletonAttribute : RegistrationAttribute
    {
        public SingletonAttribute(Type exportType) : base(exportType)
        { }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TransientAttribute : RegistrationAttribute
    {
        public TransientAttribute(Type exportType) : base(exportType)
        { }
    }

    public class RegistrationAttribute : Attribute
    {
        public Type ExportType { get; }


        public RegistrationAttribute(Type exportType)
        {
            ExportType = exportType;
        }
    }
}