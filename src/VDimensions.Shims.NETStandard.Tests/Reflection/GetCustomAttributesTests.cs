using System.Linq;
using System.Reflection;
using NUnit.Framework;
using VDimensions.Shims.NETStandard.Tests.Reflection;

[assembly:Custom(Value = "Assembly")]

namespace VDimensions.Shims.NETStandard.Tests.Reflection
{
    [Custom(Value = "Class")]
    public class GetCustomAttributesTests
    {
        [SetUp]
        public void Setup() { }

        [Custom(Value = "Method")]
        private static void DecoratedMethod() { }

        #if NETFRAMEWORK && !NET45_OR_NEWER
        private const string CustomAttributesAssemblyName = "VDimensions.Shims.NETStandard";
        #elif NET5_0_OR_NEWER || NETCOREAPP
        private const string CustomAttributesAssemblyName = "System.Private.CoreLib";
        #else
        private const string CustomAttributesAssemblyName = "mscorlib";
        #endif

        [Test]
        public void TestAssemblyLevelGetCustomAttributeGenericCall()
        {
            var a = CustomAttributeExtensions.GetCustomAttributes<CustomAttribute>(typeof(GetCustomAttributesTests).Assembly).Single();
            Assert.AreEqual(CustomAttributesAssemblyName, typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Assembly", a.Value);
        }

        [Test]
        public void TestAssemblyLevelGetCustomAttributeNonGenericCall()
        {
            var a = CustomAttributeExtensions.GetCustomAttributes(typeof(GetCustomAttributesTests).Assembly).OfType<CustomAttribute>().Single();
            Assert.AreEqual(CustomAttributesAssemblyName, typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Assembly", a.Value);
        }

        [Test]
        public void TestTypeLevelGetCustomAttributeGenericCall()
        {
            var a = CustomAttributeExtensions.GetCustomAttributes<CustomAttribute>(typeof(GetCustomAttributesTests)).Single();
            Assert.AreEqual(CustomAttributesAssemblyName, typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Class", a.Value);
        }

        [Test]
        public void TestTypeLevelGetCustomAttributeNonGenericCall()
        {
            var a = CustomAttributeExtensions.GetCustomAttributes(typeof(GetCustomAttributesTests)).OfType<CustomAttribute>().Single();
            Assert.AreEqual(CustomAttributesAssemblyName, typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Class", a.Value);
        }

        [Test]
        public void TestMethodLevelGetCustomAttributeGenericCall()
        {
            var method = typeof(GetCustomAttributesTests).GetMethod(nameof(DecoratedMethod), BindingFlags.NonPublic|BindingFlags.Static);
            var a = CustomAttributeExtensions.GetCustomAttributes<CustomAttribute>(method).Single();
            Assert.AreEqual(CustomAttributesAssemblyName, typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Method", a.Value);
        }

        [Test]
        public void TestMethodLevelGetCustomAttributeNonGenericCall()
        {
            var method = typeof(GetCustomAttributesTests).GetMethod(nameof(DecoratedMethod), BindingFlags.NonPublic|BindingFlags.Static);
            var a = CustomAttributeExtensions.GetCustomAttributes(method).OfType<CustomAttribute>().Single();
            Assert.AreEqual(CustomAttributesAssemblyName, typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Method", a.Value);
        }
    }
}