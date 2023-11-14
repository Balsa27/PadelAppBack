using PadelApp.Domain.Enums;

namespace PadelApp.Application.Commands.Court.UpdateCourtStatus;

public record UpdateCourtStatusRequest(Guid CourtId, CourtStatus Status);
