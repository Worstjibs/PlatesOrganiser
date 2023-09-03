using PlatesOrganiser.Application.Features.Collections.AddCollection;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Domain.Shared;

namespace PlatesOrganiser.Application.Unit.Tests.Features.Collections;

public class AddCollectionHandlerTests
{
    private readonly IPlateCollectionRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly AddCollectionHandler _sut;

    public AddCollectionHandlerTests()
    {
        _repository = Substitute.For<IPlateCollectionRepository>();
        _currentUserService = Substitute.For<ICurrentUserService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _sut = new AddCollectionHandler(_repository, _currentUserService, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ReturnsSuccessWithEntityId()
    {
        // Arrange
        Guid newEntityId = Guid.NewGuid();
        var command = new AddCollectionCommand("Collection 1");

        _currentUserService.GetCurrentUserAsync()
            .Returns(new PlateUser { Id = Guid.NewGuid(), UserName = "Plate User 1" });

        _repository.AddCollection(Arg.Do<PlateCollection>(x => x.Id = newEntityId), Arg.Any<CancellationToken>());

        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.NewEntityId.Should().Be(newEntityId);
    }

    [Fact]
    public async Task Handle_GivenNonExistentUser_ReturnsBad()
    {
        // Arrange
        Guid newEntityId = Guid.NewGuid();
        var command = new AddCollectionCommand("Collection 1");

        _repository.AddCollection(Arg.Do<PlateCollection>(x => x.Id = newEntityId), Arg.Any<CancellationToken>());

        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Bad);
    }
}
