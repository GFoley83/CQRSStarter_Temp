using CQRSStarter.Commands.Contract;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSStarter.Commands.Implementation.Decorators
{
    public class DeadlockRetryCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _decorated;

        public DeadlockRetryCommandHandlerDecorator(ICommandHandler<TCommand> decorated)
        {
            _decorated = decorated;
        }

        public async Task HandleAsync(TCommand command)
        {
            await HandleWithCountDown(command, 5);
        }

        private async Task HandleWithCountDown(TCommand command, int count)
        {
            try
            {
                await _decorated.HandleAsync(command);
            }
            catch(Exception ex)
            {
                if(count <= 0 || !IsDeadlockException(ex))
                {
                    throw;
                }

                Thread.Sleep(100);

                await HandleWithCountDown(command, count - 1);
            }
        }

        private static bool IsDeadlockException(Exception ex)
        {
            while(ex != null)
            {
                if(ex is DbException && ex.Message.Contains("deadlock"))
                    return true;

                ex = ex.InnerException;
            }

            return false;
        }
    }
}