using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ModelLayer.Model
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public T Data { get; set; } = default(T);
    }
}
