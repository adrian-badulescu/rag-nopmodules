using Microsoft.AspNetCore.Mvc;
using rag.Models;
using rag.Services;

namespace rag.Controllers;

public static class ChatsEndpoint
{
    public static IEndpointRouteBuilder MapChatEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/chat", async ([FromBody]ChatRequest request, IChatService chatService) =>
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return Results.BadRequest(new {error = "Please provide a message."});
            }
            // todo, set the response message
            string responseMessage = request.Message;

            return Results.Ok(responseMessage);
        });

        return app;
    }
}