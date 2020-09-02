using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduling.Domain.Infrastructure.Commands
{
    public class CommandHandlerMap
    {
        private readonly Dictionary<string, Func<object, Task>> _handlers =
            new Dictionary<string, Func<object, Task>>();

        public CommandHandlerMap(params CommandHandler[] commandHandlers)
        {
            foreach (var handler in commandHandlers.SelectMany(h => h.Handlers))
            {
                if (!_handlers.TryAdd(handler.Key, handler.Value))
                {

                    throw new DuplicateCommandHandlerException(handler.Key);
                }
            }
        }

        public Func<object, Task> Get(object command) =>
            _handlers[command.GetType().Name];
    }

    public class DuplicateCommandHandlerException : Exception
    {
        public DuplicateCommandHandlerException(string type) :
            base($"A handler has already been registered for {type}")
        {
        }
    }
}
