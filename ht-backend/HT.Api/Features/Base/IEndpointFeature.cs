namespace HT.Api.Features.Base;

public interface IEndpointFeature
{
    RouteHandlerBuilder Map(RouteGroupBuilder group);
}