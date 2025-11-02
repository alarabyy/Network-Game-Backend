using Application.Models.Writer;
using Presentation.Extensions;
using Application.Interfaces;
using Application.Common;
using FastEndpoints;

namespace Presentation.Endpoints.Writer;

public class Apply(IWriterService service) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Post("/api/writer/apply");
        Description(r=> r.WithTags("Writer"));
        Roles("Reader");
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.GetId();
        if (id == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var response = await service.ApplyAsync(id.Value, ct);
        await Send.OkAsync(response, ct);
    }
}