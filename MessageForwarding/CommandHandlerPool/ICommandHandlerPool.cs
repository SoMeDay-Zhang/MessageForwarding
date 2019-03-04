using MessageForwarding.CommandDefinition;
using MessageForwarding.CommandHandlerDefinition;

namespace MessageForwarding.CommandHandlerPool
{
    public interface ICommandHandlerPool
    {
        void InitialPool();

        /// <summary>
        /// 获取Handler
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        ICommandHandler<T> RentHandler<T>(T command) where T : ICommand;

        /// <summary>
        /// 返回Handler
        /// </summary>
        /// <param name="commandHandler"></param>
        void ReturnHandler<T>(ICommandHandler<T> commandHandler) where T : ICommand;
    }
}