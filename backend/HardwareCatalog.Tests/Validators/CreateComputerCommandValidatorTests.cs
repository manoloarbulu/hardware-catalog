using FluentAssertions;
using HardwareCatalog.Application.Commands;
using HardwareCatalog.Application.Validators;
using HardwareCatalog.Domain.Enums;
using Xunit;

namespace HardwareCatalog.Tests.Validators;

public class CreateComputerCommandValidatorTests
{
    private readonly CreateComputerCommandValidator _validator;

    public CreateComputerCommandValidatorTests()
    {
        _validator = new CreateComputerCommandValidator();
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldReturnNoErrors()
    {
        // Arrange
        var command = new CreateComputerCommand
        {
            Type = ComputerType.Desktop,
            Weight = 10.5m,
            WeightUnit = WeightUnit.Kilograms,
            Products = new List<CreateComputerProductDto>
            {
                new CreateComputerProductDto { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithZeroWeight_ShouldReturnError()
    {
        // Arrange
        var command = new CreateComputerCommand
        {
            Type = ComputerType.Desktop,
            Weight = 0,
            WeightUnit = WeightUnit.Kilograms,
            Products = new List<CreateComputerProductDto>
            {
                new CreateComputerProductDto { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateComputerCommand.Weight));
    }

    [Fact]
    public void Validate_WithNoProducts_ShouldReturnError()
    {
        // Arrange
        var command = new CreateComputerCommand
        {
            Type = ComputerType.Desktop,
            Weight = 10.5m,
            WeightUnit = WeightUnit.Kilograms,
            Products = new List<CreateComputerProductDto>()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateComputerCommand.Products));
    }

    [Fact]
    public void Validate_WithZeroQuantity_ShouldReturnError()
    {
        // Arrange
        var command = new CreateComputerCommand
        {
            Type = ComputerType.Desktop,
            Weight = 10.5m,
            WeightUnit = WeightUnit.Kilograms,
            Products = new List<CreateComputerProductDto>
            {
                new CreateComputerProductDto { ProductId = Guid.NewGuid(), Quantity = 0 }
            }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains(nameof(CreateComputerProductDto.Quantity)));
    }
}
