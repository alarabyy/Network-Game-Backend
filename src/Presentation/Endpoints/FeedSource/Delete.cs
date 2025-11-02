// using Application.Interfaces;
// using Application.Common;
// using FastEndpoints;
//
// namespace Presentation.Endpoints.FeedSource;
//
// public class DeleteFeedSourceEndpoint(IFeedSourceService service) : EndpointWithoutRequest<StandardResponse>
// {
//     public override void Configure()
//     {
//         Delete("/api/feedsource/delete/{id:int}");
//         Description(s => s.WithTags("Feed Source"));
//         AllowAnonymous();
//     }
//
//     public override async Task HandleAsync(CancellationToken ct)
//     {
//         var id = Route<int>("id");
//         var response = await service.DeleteFeedSource(id, ct);
//         await Send.OkAsync(response, ct);
//     }
// }
