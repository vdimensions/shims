using System;

namespace VDimensions.Shims.NETStandard.Tests.Reflection
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class CustomAttribute : Attribute
    {
        public CustomAttribute()
        {
        }

        public string Value { get; set; }
    }
}