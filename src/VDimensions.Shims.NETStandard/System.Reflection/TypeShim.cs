// #if NET20_OR_NEWER || !(NETSTANDARD || NET45_OR_NEWER)
// namespace System.Reflection
// {
//     internal static class TypeShim
//     {
//         internal const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
//         
//         internal static bool ImplementInterface(Type t, Type ifaceType)
//         {
//             System.Diagnostics.Contracts.Contract.Requires(ifaceType != null);
//             System.Diagnostics.Contracts.Contract.Requires(ifaceType.IsInterface, "ifaceType must be an interface type");
//
//             while (t != null)
//             {
//                 Type[] interfaces = t.GetInterfaces();
//                 if (interfaces != null)
//                 {
//                     for (int i = 0; i < interfaces.Length; i++)
//                     {
//                         // Interfaces don't derive from other interfaces, they implement them.
//                         // So instead of IsSubclassOf, we should use ImplementInterface instead.
//                         if (interfaces[i] == ifaceType || 
//                             (interfaces[i] != null && ImplementInterface(interfaces[i], ifaceType)))
//                             return true;
//                     }
//                 }
//
//                 t = t.BaseType;
//             }
//
//             return false;
//         }
//     }
// }
//
// #endif