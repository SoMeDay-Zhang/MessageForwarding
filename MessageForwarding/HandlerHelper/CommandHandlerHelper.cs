using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageForwarding.HandlerHelper
{
    public sealed class CommandHandlerHelper : ICommandHandlerHelper
    {
        private readonly List<Type> _commandTypes;

        public CommandHandlerHelper(List<Type> types)
        {
            _commandTypes = types;
        }

        public IEnumerable<Type> GetCommandHandlerTypes()
        {
            return _commandTypes.ToList();
        }
    }
}