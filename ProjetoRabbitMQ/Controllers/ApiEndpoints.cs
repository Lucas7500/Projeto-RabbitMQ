﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProjetoRabbitMQ.Models.Commands;
using ProjetoRabbitMQ.Models.Enums;

namespace ProjetoRabbitMQ.Controllers
{
    public static class ApiEndpoints
    {
        public static void AddUsersEndpoints(this WebApplication app)
        {
            app.MapPost("users/login", async (IMediator mediator, LoginCommand command, CancellationToken ct) =>
            {
                var result = await mediator.Send(command, ct);

                return result.IsSuccess
                    ? Results.Ok(new { result.Value.Token })
                    : Results.BadRequest(result.ErrorMessage);
            });

            app.MapPost("users/register", [Authorize(Roles = nameof(UserRole.Admin))] async () =>
            {

            });

            app.MapPatch("users/{id:guid}", [Authorize(Roles = nameof(UserRole.Admin))] async (Guid id) =>
            {

            });

            app.MapDelete("users/{id:guid}", [Authorize(Roles = nameof(UserRole.Admin))] async (Guid id) =>
            {

            });
        }

        public static void AddMangasEndpoints(this WebApplication app)
        {
            app.MapPost("mangas/register", [Authorize(Roles = nameof(UserRole.Admin))] async () =>
            {

            });

            app.MapPatch("mangas/{id:ulid}", [Authorize(Roles = nameof(UserRole.Admin))] async (Ulid id) =>
            {

            });

            app.MapDelete("mangas/{id:ulid}", [Authorize(Roles = nameof(UserRole.Admin))] async (Ulid id) =>
            {

            });
        }
        
        public static void AddOrdersEndpoints(this WebApplication app)
        {
            var ctSource = new CancellationTokenSource();

            app.MapPost("orders", async () =>
            {
            });
            
            app.MapPost("order", async () =>
            {

            });

            app.MapGet("check-order-status/{id:ulid}", async (Ulid id) =>
            {

            });
        }
    }
}
