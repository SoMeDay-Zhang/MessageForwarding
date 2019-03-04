using System;
using System.Collections.Generic;

namespace MessageForwarding.HandlerHelper
{
    /// <summary>
    /// Command 帮助接口
    /// </summary>
    public interface ICommandHandlerHelper
    {
        /// <summary>
        /// 获取 Command 处理类型
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetCommandHandlerTypes();
    }
}