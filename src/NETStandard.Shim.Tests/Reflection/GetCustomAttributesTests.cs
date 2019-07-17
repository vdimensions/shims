using System.Linq;
using System.Reflection;
using NETStandard.Shim.Tests.Reflection;
using NUnit.Framework;

[assembly:Custom(Value = "Assembly")]

namespace NETStandard.Shim.Tests.Reflection
{
    [Custom(Value = "Class")]
    public class GetCustomAttributesTests
    {
        [SetUp]
        public void Setup() { }

        [Custom(Value = "Method")]
        private static void DecoratedMethod() { }

        [Test]
        public void TestAssemblyLevelGetCustomAttributeGenericCall()
        {
            var a = CustomAttributeExtensions.GetCustomAttributes<CustomAttribute>(typeof(GetCustomAttributesTests).Assembly).Single();
            Assert.AreEqual("NETStandard.Shim", typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Assembly", a.Value);
        }

        [Test]
        public void TestAssemblyLevelGetCustomAttributeNonGenericCall()
        {
            var a = CustomAttributeExtensions.GetCustomAttributes(typeof(GetCustomAttributesTests).Assembly).OfType<CustomAttribute>().Single();
            Assert.AreEqual("NETStandard.Shim", typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Assembly", a.Value);
        }

        [Test]
        public void TestTypeLevelGetCustomAttributeGenericCall()
        {
            var a = CustomAttributeExtensions.GetCustomAttributes<CustomAttribute>(typeof(GetCustomAttributesTests)).Single();
            Assert.AreEqual("NETStandard.Shim", typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Class", a.Value);
        }

        [Test]
        public void TestTypeLevelGetCustomAttributeNonGenericCall()
        {
            var a = CustomAttributeExtensions.GetCustomAttributes(typeof(GetCustomAttributesTests)).OfType<CustomAttribute>().Single();
            Assert.AreEqual("NETStandard.Shim", typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Class", a.Value);
        }

        [Test]
        public void TestMethodLevelGetCustomAttributeGenericCall()
        {
            var method = typeof(GetCustomAttributesTests).GetMethod(nameof(DecoratedMethod), BindingFlags.NonPublic|BindingFlags.Static);
            var a = CustomAttributeExtensions.GetCustomAttributes<CustomAttribute>(method).Single();
            Assert.AreEqual("NETStandard.Shim", typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Method", a.Value);
        }

        [Test]
        public void TestMethodLevelGetCustomAttributeNonGenericCall()
        {
            var method = typeof(GetCustomAttributesTests).GetMethod(nameof(DecoratedMethod), BindingFlags.NonPublic|BindingFlags.Static);
            var a = CustomAttributeExtensions.GetCustomAttributes(method).OfType<CustomAttribute>().Single();
            Assert.AreEqual("NETStandard.Shim", typeof(CustomAttributeExtensions).Assembly.GetName().Name);
            Assert.AreEqual("Method", a.Value);
        }
    }
}