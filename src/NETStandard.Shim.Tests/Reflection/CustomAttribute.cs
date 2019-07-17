using System;

namespace NETStandard.Shim.Tests.Reflection
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class CustomAttribute : Attribute
    {
        public CustomAttribute()
        {
        }

        public string Value { get; set; }
    }
}