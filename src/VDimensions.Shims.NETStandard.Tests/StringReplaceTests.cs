using System;
using NUnit.Framework;

namespace VDimensions.Shims.NETStandard.Tests
{
    public class StringReplaceTests
    {
        [Test]
        public void Replace()
        {
            var patternString = "Hello, ${Placeholder}!";
            #if NET20
            var replacedString = StringExtensions.Replace(patternString, "${Placeholder}", "World", StringComparison.Ordinal);
            #else
            var replacedString = patternString.Replace("${Placeholder}", "World", StringComparison.Ordinal);
            #endif
            Assert.AreEqual(replacedString, "Hello, World!");
        }
    }
}