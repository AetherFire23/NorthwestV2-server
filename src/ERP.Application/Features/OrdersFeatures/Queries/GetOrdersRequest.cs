using AetherFire23.ERP.Domain.Entity.Orders;
using Mediator;

namespace NorthwestV2.Application.Features.OrdersFeatures.Queries;

public class GetOrdersRequest : IRequest<List<Order>>
{

}