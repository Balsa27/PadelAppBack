﻿using PadelApp.Domain.Enums;

namespace PadelApp.Application.Commands.Auth.Login;

public record UserLoginResponse(string Token, Guid Id, Role Role);
