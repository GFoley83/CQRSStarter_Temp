using System;

namespace CQRSStarter.Commands.Implementation.Todo
{
    public class DeleteTodoCommand
    {
        public Guid Id { get; set; }
    }
}