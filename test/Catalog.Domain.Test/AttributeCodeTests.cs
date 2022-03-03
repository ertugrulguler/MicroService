using Catalog.Domain.ValueObject;
using FluentAssertions;
using Framework.Core.Model;
using NUnit.Framework;
using System;

namespace Catalog.Domain.Test
{
    public class AttributeCodeTests
    {
        [Test]
        public void Create_Should_Success()
        {
            var attributeCode = new AttributeCode("qwerty");

            Assert.AreEqual(attributeCode.Code.Length, 6);
        }

        [Test]
        public void Create_Should_Fail_When_Code_Length_GreaterThan_Six()
        {
            Action action = () => new AttributeCode("qwertyas");

            action.Should().Throw<Exception>().And.GetType().Should().Be<BusinessRuleException>();
            action.Should().Throw<BusinessRuleException>().And.Code.Should().Be(ApplicationMessage.AttributeCodeInvalid);
            action.Should().Throw<BusinessRuleException>().And.Message.Should().Be(ApplicationMessage.AttributeCodeInvalid.Message());
        }
    }
}