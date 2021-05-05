using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DeliberateFail
    {
        [Test]
        public void DeliberateFail_Fails()
        {
            Assert.Fail();
        }
    }
}
