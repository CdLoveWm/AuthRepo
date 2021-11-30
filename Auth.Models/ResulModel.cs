using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Models
{
    public class ResulModel<T>
    {
        /// <summary>
        /// 0：正常， 其他：不正常
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }
    }
}
