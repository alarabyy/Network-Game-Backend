using Application.Interfaces;
using DataAccess.Entities;
using FastEndpoints;
using System.Text.Json;

namespace Presentation.Endpoints.Feeds
{
    public class GetFeedByIdRequest
    {
        public int Id { get; set; } // رقم الخبر نفسه
    }

    public class GetFeedByIdEndpoint : Endpoint<GetFeedByIdRequest>
    {
        private readonly IFeedService _feedService;

        public GetFeedByIdEndpoint(IFeedService feedService)
        {
            _feedService = feedService;
        }

        public override void Configure()
        {
            Get("/api/feeds/{Id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetFeedByIdRequest req, CancellationToken ct)
        {
            var feed = await _feedService.GetFeedByIdAsync(req.Id, ct);

            if (feed == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await HttpContext.Response.WriteAsync("Feed not found", ct);
                return;
            }

            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            HttpContext.Response.ContentType = "application/json";
            await HttpContext.Response.WriteAsync(JsonSerializer.Serialize(feed), ct);
        }
    }
}
