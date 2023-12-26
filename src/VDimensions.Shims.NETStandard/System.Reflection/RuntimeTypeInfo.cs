// #if NETFRAMEWORK && !NET45_OR_NEWER
// using System.Globalization;
//
// namespace System.Reflection
// {
//     internal class RuntimeTypeInfo : TypeInfo
//     {
//         private readonly Type _runtimeType;
//         
//         public RuntimeTypeInfo(Type runtimeType)
//         {
//             _runtimeType = runtimeType;
//         }
//         
//         public override object[] GetCustomAttributes(bool inherit)
//         {
//             return _runtimeType.GetCustomAttributes(inherit);
//         }
//
//         public override bool IsDefined(Type attributeType, bool inherit)
//         {
//             return _runtimeType.IsDefined(attributeType, inherit);
//         }
//
//         public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetConstructors(bindingAttr);
//         }
//
//         public override Type GetInterface(string name, bool ignoreCase)
//         {
//             return _runtimeType.GetInterface(name, ignoreCase);
//         }
//
//         public override Type[] GetInterfaces()
//         {
//             return _runtimeType.GetInterfaces();
//         }
//
//         public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetEvent(name, bindingAttr);
//         }
//
//         public override EventInfo[] GetEvents(BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetEvents(bindingAttr);
//         }
//
//         public override Type[] GetNestedTypes(BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetNestedTypes(bindingAttr);
//         }
//
//         public override Type GetNestedType(string name, BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetNestedType(name, bindingAttr);
//         }
//
//         public override Type GetElementType()
//         {
//             return _runtimeType.GetElementType();
//         }
//
//         protected override bool HasElementTypeImpl()
//         {
//             return _runtimeType.HasElementTypeImpl();
//         }
//
//         protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types,
//             ParameterModifier[] modifiers)
//         {
//             return _runtimeType.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);
//         }
//
//         public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetProperties(bindingAttr);
//         }
//
//         protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention,
//             Type[] types, ParameterModifier[] modifiers)
//         {
//             return _runtimeType.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
//         }
//
//         public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetMethods(bindingAttr);
//         }
//
//         public override FieldInfo GetField(string name, BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetField(name, bindingAttr);
//         }
//
//         public override FieldInfo[] GetFields(BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetFields(bindingAttr);
//         }
//
//         public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
//         {
//             return _runtimeType.GetMembers(bindingAttr);
//         }
//
//         protected override TypeAttributes GetAttributeFlagsImpl()
//         {
//             throw new NotImplementedException();
//         }
//
//         protected override bool IsArrayImpl()
//         {
//             throw new NotImplementedException();
//         }
//
//         protected override bool IsByRefImpl()
//         {
//             throw new NotImplementedException();
//         }
//
//         protected override bool IsPointerImpl()
//         {
//             throw new NotImplementedException();
//         }
//
//         protected override bool IsPrimitiveImpl()
//         {
//             throw new NotImplementedException();
//         }
//
//         protected override bool IsCOMObjectImpl()
//         {
//             throw new NotImplementedException();
//         }
//
//         public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
//             ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
//         {
//             throw new NotImplementedException();
//         }
//
//         public override Type UnderlyingSystemType { get; }
//
//         protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention,
//             Type[] types, ParameterModifier[] modifiers)
//         {
//             throw new NotImplementedException();
//         }
//
//         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
//         {
//             return _runtimeType.GetCustomAttributes(attributeType, inherit);
//         }
//
//         public override string Name => _runtimeType.Name;
//         public override Guid GUID => _runtimeType.GUID;
//         public override Module Module => _runtimeType.Module;
//         public override Assembly Assembly => _runtimeType.Assembly;
//         public override string FullName => _runtimeType.FullName;
//         public override string Namespace => _runtimeType.Namespace;
//         public override string AssemblyQualifiedName => _runtimeType.AssemblyQualifiedName;
//         public override Type BaseType => _runtimeType.BaseType;
//     }
// }
// #endif