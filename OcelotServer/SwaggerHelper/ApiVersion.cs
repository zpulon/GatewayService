using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotServer.SwaggerHelper
{
    /// <summary>
    /// API版本
    /// </summary>
    public class ApiVersion
    {
        /// <summary>
        /// 版本列表
        /// </summary>
        public List<VersionItem> Versions { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class VersionItem
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public string Name { get; set; }
    }
}
