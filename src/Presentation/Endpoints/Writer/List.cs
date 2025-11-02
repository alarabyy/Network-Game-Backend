using Application.Interfaces;
using Application.Dtos;
using FastEndpoints;

namespace Presentation.Endpoints.Writer;

public class List(IWriterService service) : EndpointWithoutRequest<List<WriterApplicationRequestDto>>
{
    public override void Configure()
    {
        Get("/api/writer/list");
        Description(r=> r.WithTags("Writer"));
        Roles("Admin");
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {;
        var response = await service.GetAllAsync(ct);
        await Send.OkAsync(response, ct);
    }
}