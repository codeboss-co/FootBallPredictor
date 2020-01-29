using System.Threading;
using System.Threading.Tasks;

namespace FootballPredictor.Domain.Services
{
    public interface IQuery<TResult> { }

    public interface IQueryHandler<in TQuery, TResult> where TQuery: IQuery<TResult>
    {
        ValueTask<TResult> Handle(TQuery query, CancellationToken token = default);
    }
}
