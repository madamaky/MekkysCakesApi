using MediatR;

namespace MekkysCakes.Application.Features.Authentication.Queries.CheckEmail
{
    public record CheckEmailQuery(string Email) : IRequest<bool>;
}
