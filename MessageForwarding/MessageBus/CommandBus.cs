using System.Threading.Tasks;
using MessageForwarding.CommandDefinition;
using MessageForwarding.CommandHandlerDefinition;
using MessageForwarding.CommandHandlerPool;
using Microsoft.Extensions.Logging;

namespace MessageForwarding.MessageBus
{
    public sealed class CommandBus : ICommandBus
    {
        private readonly ICommandHandlerPool _commandHandlerPool;
        private readonly ILogger<CommandBus> _logger;

        public CommandBus(ILogger<CommandBus> logger, ICommandHandlerPool commandHandlerPool)
        {
            _logger = logger;
            _commandHandlerPool = commandHandlerPool;
        }

        public async Task SendAsync<T>(T command) where T : ICommand
        {
            ICommandHandler<T> handler = GetHandler(command);
            if (handler != null)
            {
                await handler.ExecuteAsync(command);
                ReturnHandler(handler);
            }
            else
            {
                _logger.LogWarning($"no handler registered,command name:{command.GetType().Name}");
            }
        }

        private ICommandHandler<T> GetHandler<T>(T command) where T : ICommand
        {
            return _commandHandlerPool.RentHandler(command);
        }

        private void ReturnHandler<T>(ICommandHandler<T> handler) where T : ICommand
        {
            _commandHandlerPool.ReturnHandler(handler);
        }
    }
}