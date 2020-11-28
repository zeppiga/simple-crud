using System;
using System.Reflection;

namespace simple_crud.ServicesRegistration
{
    public sealed class RegistrationException : Exception
    {
        public RegistrationException(MemberInfo typeInfo) : base(GetMessage(typeInfo))
        { }

        private static string GetMessage(MemberInfo typeInfo) => $"Cannot register properly type {typeInfo.Name}!";
    }
}