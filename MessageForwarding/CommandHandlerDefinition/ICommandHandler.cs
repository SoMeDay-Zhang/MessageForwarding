using System;
using System.Threading.Tasks;
using MessageForwarding.CommandDefinition;

namespace MessageForwarding.CommandHandlerDefinition
{
    public interface ICommandHandler<in TCommand> : IDisposable where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }
}