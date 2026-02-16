using Mediator;

namespace NorthwestV2.Application.Features.ProductFeature.Commands.ProductCreation;

public class CreateProductRequest : IRequest<Guid>
{
    public required string ProductName { get; set; }
    public required decimal BasePrice { get; set; }
}