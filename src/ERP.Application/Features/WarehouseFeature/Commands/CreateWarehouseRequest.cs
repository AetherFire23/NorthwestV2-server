using Mediator;

namespace NorthwestV2.Application.Features.WarehouseFeature.Commands;

public class CreateWarehouseRequest : IRequest<Guid>
{
    public required Guid CompanyId { get; set; }
    public required string WarehouseName { get; set; }
}