using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sma.Stm.Common.PlugIns.ExtendedValidation;
using System.IO;

namespace Sma.Stm.Test
{
    [TestClass]
    public class PluginTest
    {
        [TestMethod]
        public void ValidRtz()
        {
            var validator = new ExtendedValidationHandler(@"..\..\..\..\Sma.Stm.Plugins.Rtz\bin\Debug\netcoreapp2.0");
            var result = validator.Validate(File.ReadAllText(@".\TestData\BARC-GOT.rtz"));

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void InvalidRtz()
        {
            var validator = new ExtendedValidationHandler(@"..\..\..\..\Sma.Stm.Plugins.Rtz\bin\Debug\netcoreapp2.0");
            var result = validator.Validate(File.ReadAllText(@".\TestData\BARC-GOT_invalidUvid.rtz"));

            Assert.AreEqual(1, result.Count);
        }

    }
}
