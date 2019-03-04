using System.Collections.Generic;

namespace MessageForwarding.CommandHandlerPool
{
    /// <summary>
    /// Command Handler Pool Config
    /// </summary>
    public class CommandHandlerPoolConfig
    {
        public Dictionary<string, int> CommandHandlerNameAndPoolCapacity { get; set; } = new Dictionary<string, int>();
    }
}