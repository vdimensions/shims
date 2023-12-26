namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Constructor|AttributeTargets.Method, Inherited = false)]
    internal sealed class NonVersionableAttribute : Attribute { }
}