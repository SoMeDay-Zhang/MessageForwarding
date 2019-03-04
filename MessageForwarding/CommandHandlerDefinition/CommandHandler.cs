using System;
using System.Threading.Tasks;
using MessageForwarding.CommandDefinition;

namespace MessageForwarding.CommandHandlerDefinition
{
    public abstract class CommandHandler<T> : ICommandHandler<T> where T : ICommand
    {
        private bool _disposed;

        public async Task ExecuteAsync(T command)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override bool Equals(object obj)
        {
            return obj != null && ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            _disposed = true;
        }
    }
}