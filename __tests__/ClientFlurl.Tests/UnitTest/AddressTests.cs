using ClientFlurl.Entities;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace ClientFlurl.Tests.UnitTest
{
    public class AddressTests
    {
        [Fact(DisplayName = "Test if class Adrress is valid")]
        public void Should_be_address_valid()
        {
            //Arrange
            var address = Builder<Address>.CreateNew()
                                          .Build();

            //Act

            //Assert 
            address.IsNotValid().Should().BeFalse();
        }

        [Fact(DisplayName = "Test if class Adrress not is valid")]
        public void Should_be_not_address_valid()
        {
            //Arrange
            var address = Builder<Address>.CreateNew()
                                             .With(x => x.ZipCode = null)
                                             .Build();

            //Act
            address.ZipCode = null;

            //Assert 
            address.IsNotValid().Should().BeTrue();
        }

    }
}
