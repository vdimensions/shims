#if NETSTANDARD || NET35_OR_NEWER
using System.Collections.Concurrent;
#endif
using NUnit.Framework;

namespace VDimensions.Shims.NETStandard.Tests.Collections.Concurrent
{
    [TestFixture]
    public class ConcurrentDictionaryTest
    {
        #if NETSTANDARD && !NETFRAMEWORK
        private const string ConcurrentDictionaryAssemblyName = "System.Collections.Concurrent";
        #elif NETSTANDARD1_0
        private const string ConcurrentDictionaryAssemblyName = "Portable.ConcurrentDictionary";
        #elif NET35
        private const string ConcurrentDictionaryAssemblyName = "System.Threading.Tasks.NET35";
        #else
        private const string ConcurrentDictionaryAssemblyName = "mscorlib";
        #endif
        
        #if NETSTANDARD || NET35_OR_NEWER
        [Test]
        public void TestAssemblyLevelGetCustomAttributeGenericCall()
        {
            var concurrentDictionary = new ConcurrentDictionary<string, int>();
            var concurrentDictionaryAssembly = concurrentDictionary.GetType().Assembly;
            Assert.AreEqual(ConcurrentDictionaryAssemblyName, concurrentDictionaryAssembly.GetName().Name);
        }
        #endif
    }
}