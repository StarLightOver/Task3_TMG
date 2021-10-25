using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Task3_TMG;
using Assert = NUnit.Framework.Assert;

namespace Tests
{
    public class TestCalculateLineIndex
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TestPetrenkoGoltzmanMethod_Success()
        {
            // Arrange
            var testLine = "Не выходи из комнаты, не совершай ошибку.";
            
            //Act
            var model = new PetrenkoGoltzmanMethod();
            var index = model.GetIndex(testLine);
                
            // Assert
            var expected = 17968.5d;
            Assert.AreEqual(expected, index);
        }
    }
}