﻿namespace MyCarAuction.Api.Domain.Models;

internal sealed class User(Guid id, string name, string email)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Email { get; } = email;
}
