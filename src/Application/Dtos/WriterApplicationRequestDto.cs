using DataAccess.Types;

namespace Application.Dtos;

public record WriterApplicationRequestDto(int Id, WriterType? Type, DateTime ApplicationDate, DateTime? ApprovedDate, UserDto User);