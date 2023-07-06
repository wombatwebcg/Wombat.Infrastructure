using System;
using System.Collections.Generic;
using System.Text;

namespace Wombat.Infrastructure
{
    /// <summary>
    /// InfluxDb配置信息
    /// </summary>
    public class InfluxDbOptions
    {

        /// <summary>
        /// 发行者
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 受众
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int AtInterval { get; set; }
        /// <summary>
        /// 刷新时间
        /// </summary>
        public string DbName { get; set; }
    }
}
