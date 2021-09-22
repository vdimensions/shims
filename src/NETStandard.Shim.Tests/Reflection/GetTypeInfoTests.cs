// using System.Reflection;
// using NUnit.Framework;
//
// namespace NETStandard.Shim.Tests.Reflection
// {
//     public class GetTypeInfoTests
//     {
//         [Test]
//         public void TestAssemblyLevelGetCustomAttributeGenericCall()
//         {
//             var typeInfo = IntrospectionExtensions.GetTypeInfo(typeof(GetCustomAttributesTests));
//             Assert.IsNotNull(typeInfo);
//             Assert.AreEqual("NETStandard.Shim", typeInfo.Assembly.GetName().Name);
//         }
//     }
// }