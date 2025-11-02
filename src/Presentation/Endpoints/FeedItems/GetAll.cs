using Application.Interfaces;
using DataAccess.Entities;
using FastEndpoints;
using System.Text.Json;

namespace Presentation.Endpoints.Feeds
{
    public class GetAllFeedsEndpoint : EndpointWithoutRequest
    {
        private readonly IFeedService _feedService;

        public GetAllFeedsEndpoint(IFeedService feedService)
        {
            _feedService = feedService;
        }

        public override void Configure()
        {
            Get("/api/feeds");
            Description(r => r.WithTags("Feeds"));
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var feeds = await _feedService.GetAllFeedsAsync(ct);

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            HttpContext.Response.ContentType = "application/json";
            await HttpContext.Response.WriteAsync(JsonSerializer.Serialize(feeds), ct);
        }
    }
}
