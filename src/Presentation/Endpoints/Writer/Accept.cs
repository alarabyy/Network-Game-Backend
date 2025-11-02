using Application.Models.Writer;
using Application.Interfaces;
using Application.Common;
using FastEndpoints;

namespace Presentation.Endpoints.Writer;

public class Accept(IWriterService service) : Endpoint<WriterApplicationAcceptanceRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/api/writer/application/accept");
        Description(r=> r.WithTags("Writer"));
        Roles("Admin");
    }
    
    public override async Task HandleAsync(WriterApplicationAcceptanceRequest request, CancellationToken ct)
    {;
        var response = await service.AcceptAsync(request, ct);
        await Send.OkAsync(response, ct);
    }
}