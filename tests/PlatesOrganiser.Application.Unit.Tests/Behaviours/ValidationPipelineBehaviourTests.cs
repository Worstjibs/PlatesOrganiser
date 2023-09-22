using FluentValidation;
using FluentValidation.Results;
using MediatR;
using PlatesOrganiser.Application.Behaviours;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Unit.Tests.Behaviours;

public class ValidationPipelineBehaviourTests
{
    public class ValidationPipelineBehaviourTests_Query
    {
        private readonly IPipelineBehavior<FakeQuery, Result<FakeResponse>> _sut;

        private readonly IValidator<FakeQuery> _validator;

        public ValidationPipelineBehaviourTests_Query()
        {
            _validator = Substitute.For<IValidator<FakeQuery>>();

            _sut = new ValidationPipelineBehaviour<FakeQuery, Result<FakeResponse>>(new[] { _validator });
        }

        [Fact]
        public async Task Handle_GivenQueryWithValidationErrors_ReturnsBadResult()
        {
            // Arrange
            var query = new FakeQuery();

            var validationFailure = new ValidationFailure { ErrorMessage = "Validation Failed" };
            var validationResult = new ValidationResult(new[] { validationFailure });

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(validationResult);

            // Act
            var result = await _sut.Handle(query, null, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Message.Should().Be(validationFailure.ErrorMessage);
        }
    }

    public class ValidationPipelineBehaviourTests_Command
    {
        private readonly IPipelineBehavior<FakeCommand, Result> _sut;

        private readonly IValidator<FakeCommand> _validator;

        public ValidationPipelineBehaviourTests_Command()
        {
            _validator = Substitute.For<IValidator<FakeCommand>>();

            _sut = new ValidationPipelineBehaviour<FakeCommand, Result>(new[] { _validator });
        }

        [Fact]
        public async Task Handle_GivenCommandWithValidationErrors_ReturnsBadResult()
        {
            // Arrange
            var query = new FakeCommand();

            var validationFailure = new ValidationFailure { ErrorMessage = "Validation Failed" };
            var validationResult = new ValidationResult(new[] { validationFailure });

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(validationResult);

            // Act
            var result = await _sut.Handle(query, null, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Message.Should().Be(validationFailure.ErrorMessage);
        }
    }

    public record FakeCommand : IRequest<Result>;
    public record FakeQuery : IRequest<Result<FakeResponse>>;
    public record FakeResponse();
}
