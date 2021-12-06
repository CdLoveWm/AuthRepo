using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Models.Jwt
{
    /// <summary>
    /// JWT配置文件
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// token是谁颁发的
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// token可以给哪些客户端使用
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 对称加密key（SecretKey必须大于16）
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// 多少分钟后过期
        /// </summary>
        public int Expire { get; set; }
    }
}
