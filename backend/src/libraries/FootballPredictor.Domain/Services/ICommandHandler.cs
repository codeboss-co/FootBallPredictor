using System.Threading;
using System.Threading.Tasks;

namespace FootballPredictor.Domain.Services
{
    public interface ICommandHandler<TCommand>
    {
        Task Handle(TCommand command, CancellationToken token = default);
    }
}
