using DataAccess.Types;

namespace Application.Models.Writer;

public record WriterApplicationAcceptanceRequest(int ApplicationId, bool Accepted, WriterType Type);