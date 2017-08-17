using CQRSStarter.Commands.Contract;
using CQRSStarter.DAL;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CQRSStarter.Commands.Implementation.Todo
{
    public class TodoCommandHandler : ICommandHandler<AddTodoCommand>,
                                      ICommandHandler<UpdateTodoCommand>,
                                      ICommandHandler<DeleteTodoCommand>
    {
        private readonly IRepository _repository;

        public TodoCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(AddTodoCommand command)
        {
            _repository.Todos.Add(command.Entity);
        }

        public async Task HandleAsync(UpdateTodoCommand command)
        {
            var entry = _repository.Entry(command.Entity);

            if(entry.State == EntityState.Detached)
            {
                _repository.Todos.Attach(command.Entity);
            }

            entry.State = EntityState.Modified;
        }

        public async Task HandleAsync(DeleteTodoCommand command)
        {
            var entity = await _repository.Todos
                .SingleOrDefaultAsync(e => e.Id == command.Id);

            if(entity != null)
            {
                _repository.Todos.Remove(entity);
            }
        }
    }
}