using CQRSStarter.Commands.Contract;
using CQRSStarter.Commands.Implementation.Todo;
using CQRSStarter.DAL;
using CQRSStarter.DAL.Entities;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace CQRSStarter.API.Controllers
{
    public class TodoController : ApiController
    {
        private readonly IRepository _repository;
        private readonly ICommandHandler<AddTodoCommand> _addTodoCommandHandler;
        private readonly ICommandHandler<UpdateTodoCommand> _updateTodoCommandHandler;

        public TodoController(IRepository repository,
            ICommandHandler<AddTodoCommand> addTodoCommandHandler,
            ICommandHandler<UpdateTodoCommand> updateTodoCommandHandler)
        {
            _repository = repository;
            _addTodoCommandHandler = addTodoCommandHandler;
            _updateTodoCommandHandler = updateTodoCommandHandler;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get() {
            return Ok(await _repository.Todos.ToListAsync());
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostTodo(Todo todo)
        {
            await _addTodoCommandHandler.HandleAsync(new AddTodoCommand()
            {
                Entity = todo
            });

            return Ok(await _repository.Todos.ToListAsync());
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutTodo(Todo todo)
        {
            await _updateTodoCommandHandler.HandleAsync(new UpdateTodoCommand()
            {
                Entity = todo
            });

            return Ok(await _repository.Todos.ToListAsync());
        }
    }
}
