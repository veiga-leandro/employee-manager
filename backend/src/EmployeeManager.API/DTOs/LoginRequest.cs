﻿namespace EmployeeManager.API.DTOs
{
    public sealed record LoginRequest(
        string Email,
        string Password
    );
}
