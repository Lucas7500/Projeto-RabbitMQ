﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoRabbitMQ.Models.Enums;
using ProjetoRabbitMQ.Models.Login.Commands;
using ProjetoRabbitMQ.Models.User.Commands;

namespace ProjetoRabbitMQ.Controllers
{
    public static class ApiEndpoints
    {
        public static void AddUsersEndpoints(this WebApplication app)
        {
            app.MapPost("users/login", 
                async (
                    [FromBody] LoginCommand command, 
                    [FromServices] IMediator mediator, 
                    [FromServices] CancellationToken ct) =>
                {
                    var result = await mediator.Send(command, ct);

                    return result.IsSuccess
                        ? Results.Ok(new { result.Value.Token })
                        : Results.BadRequest(result.ErrorMessage);
                });

            app.MapPost("users/register", [Authorize(Roles = nameof(UserRole.Admin))] 
                async (
                    [FromBody] CreateUserCommand command,
                    [FromServices] IMediator mediator,
                    [FromServices] CancellationToken ct) =>
                {
                    var result = await mediator.Send(command, ct);

                    return result.IsSuccess
                        ? Results.Ok($"User created successfully with id: {result.Value.Id}!")
                        : Results.BadRequest(result.ErrorMessage);
                });

            app.MapPatch("users/{id:guid}", [Authorize(Roles = nameof(UserRole.Admin))] 
                async (
                   [FromRoute] Guid id, 
                   [FromBody] UpdateUserCommand command,
                   [FromServices] IMediator mediator,
                   [FromServices] CancellationToken ct) =>
                {
                    command.UserId = id;

                    var result = await mediator.Send(command, ct);

                    return result.IsSuccess
                        ? Results.Ok($"User with id {id} updated successfully!")
                        : Results.BadRequest(result.ErrorMessage);
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

            app.MapGet("orders", async () =>
            {
            });

            app.MapGet("check-order/{id:ulid}", async (Ulid id) =>
            {

            });

            app.MapPost("order", async () =>
            {

            });
        }
    }
}
