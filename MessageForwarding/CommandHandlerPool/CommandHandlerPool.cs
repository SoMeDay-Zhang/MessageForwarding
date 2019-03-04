using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CustomObjectPool;
using MessageForwarding.CommandDefinition;
using MessageForwarding.CommandHandlerDefinition;
using MessageForwarding.HandlerHelper;
using Microsoft.Extensions.DependencyInjection;

namespace MessageForwarding.CommandHandlerPool
{
    public sealed class CommandHandlerPool : ICommandHandlerPool
    {
        private static readonly ConcurrentDictionary<string, Pool<ICommandHandler<ICommand>>> HandlerDic =
            new ConcurrentDictionary<string, Pool<ICommandHandler<ICommand>>>();

        private readonly ICommandHandlerHelper _handlerHelper;
        private readonly CommandHandlerPoolConfig _poolConfig;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerPool(CommandHandlerPoolConfig poolConfig, ICommandHandlerHelper handlerHelper,
            IServiceProvider serviceProvider)
        {
            _poolConfig = poolConfig;
            _handlerHelper = handlerHelper;
            _serviceProvider = serviceProvider;
        }

        public void InitialPool()
        {
            IEnumerable<Type> handlers = _handlerHelper.GetCommandHandlerTypes().ToList();
            Console.WriteLine("Begin initial commandHandlers");

            Dictionary<string, int> commandHandlerNameAndPoolCapacity = _poolConfig.CommandHandlerNameAndPoolCapacity;
            foreach (KeyValuePair<string, int> keyValue in commandHandlerNameAndPoolCapacity)
            {
                Console.WriteLine($"{keyValue.Key} begin initial pool");
                Type handlerType = handlers.FirstOrDefault(q => q.Name.Equals(keyValue.Key));
                if (handlerType == null) continue;

                ICommandHandler<ICommand> Func() => CreateCommandHandler(handlerType);
                var pool = new Pool<ICommandHandler<ICommand>>(AccessModel.FIFO, keyValue.Value, Func);
                HandlerDic.TryAdd(handlerType.Name, pool);
                Console.WriteLine($"{handlerType.Name} initial finished");
            }
        }

        public ICommandHandler<T> RentHandler<T>(T command) where T : ICommand
        {
            string commandHandlerName = GetCommandHandlerName(command);
            bool getHandlerSucceed =
                HandlerDic.TryGetValue(commandHandlerName, out Pool<ICommandHandler<ICommand>> pool);
            if (!getHandlerSucceed) return GenerateCommandHandler(commandHandlerName) as ICommandHandler<T>;

            ICommandHandler<ICommand> temp = pool.Rent();
            return (temp ?? GenerateCommandHandler(commandHandlerName)) as ICommandHandler<T>;
        }

        public void ReturnHandler<T>(ICommandHandler<T> commandHandler) where T : ICommand
        {
            string commandHandlerName = commandHandler.GetType().Name;
            bool isPool = _poolConfig.CommandHandlerNameAndPoolCapacity.Keys.Contains(commandHandlerName);
            if (isPool)
            {
                bool getValueSucceed =
                    HandlerDic.TryGetValue(commandHandlerName, out Pool<ICommandHandler<ICommand>> commandHandlerPool);
                if (getValueSucceed)
                {
                    commandHandlerPool.Return(commandHandler as ICommandHandler<ICommand>);
                }
            }

            commandHandler.Dispose();
        }

        private string GetCommandHandlerName<T>(T command) where T : ICommand
        {
            Type temp = _handlerHelper.GetCommandHandlerTypes()
                .FirstOrDefault(q => typeof(ICommandHandler<T>).IsAssignableFrom(q));
            if (temp != null) return temp.Name;

            Console.WriteLine($"Command:{command.ToString()} handler unRegister");
            return string.Empty;
        }

        private ICommandHandler<ICommand> GenerateCommandHandler(string commandHandlerName)
        {
            IEnumerable<Type> handlers = _handlerHelper.GetCommandHandlerTypes().ToList();
            Type handlerType = handlers.FirstOrDefault(q => q.Name.Equals(commandHandlerName));
            return handlerType == null ? null : CreateCommandHandler(handlerType);
        }

        private ICommandHandler<ICommand> CreateCommandHandler(Type handlerType)
        {
            return (ICommandHandler<ICommand>) ActivatorUtilities.CreateInstance(_serviceProvider, handlerType);
        }
    }
}