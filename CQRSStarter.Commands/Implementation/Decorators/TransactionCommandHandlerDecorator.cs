using CQRSStarter.Commands.Contract;
using CQRSStarter.DAL;
using System.Threading.Tasks;

namespace Turnover.Command.Implementation.Decorators
{
    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _decorated;
        private readonly IRepository _repository;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decorated, 
            IRepository repository)
        {
            _decorated = decorated;
            _repository = repository;
        }

        public async Task HandleAsync(TCommand command)
        {
            await _decorated.HandleAsync(command);
            await _repository.SaveChangesAsync();
        }
    }
}