using System.Threading.Tasks;
using MessageForwarding.CommandDefinition;

namespace MessageForwarding.MessageBus
{
    public interface ICommandBus
    {
        Task SendAsync<T>(T command) where T : ICommand;
    }
}