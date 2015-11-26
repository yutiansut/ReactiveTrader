using System.Collections.Generic;
using NUnit.Framework;
using Adaptive.ReactiveTrader.Client;

namespace Adaptive.ReactiveTrader.Tests
{
    [TestFixture]
    public class FodyHasRunTests
    {
        [Test]
        public void Fody_Has_Run_On_Client()
        {
            var changes = new List<string>();
            var test = new TestClass();
            test.PropertyChanged += (sender, e) => changes.Add(e.PropertyName);
            test.Test = "hello world";
            Assert.AreEqual(1, changes.Count);
        }
    }
}

