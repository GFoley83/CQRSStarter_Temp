using System.Threading.Tasks;

namespace CQRSStarter.Commands.Contract
{
    public interface ICommandHandler<TCommand>
    {
        Task HandleAsync(TCommand command);
    }
}