using Application.Interfaces;
using Application.Common;
using FastEndpoints;

namespace Presentation.Endpoints.Writer;

public class Remove(IWriterService service) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Post("/api/writer/remove/{id:int}");
        Description(r=> r.WithTags("Writer"));
        Roles("Admin");
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var response = await service.RemoveAsync(id, ct);
        await Send.OkAsync(response, ct);
    }
}