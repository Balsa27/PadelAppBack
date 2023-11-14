using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Queries.Court.CourtById;

public record CourtByIdResponse(Guid CourtId, string Name, string Description, Address Address, List<Price> Price, string? ProfilePictureUrl, List<string>? CourtImages);
