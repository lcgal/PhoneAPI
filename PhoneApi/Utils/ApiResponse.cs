using System;

namespace PhoneAPI.Utils
{
    public class ApiResponse<T>
    {
        public Boolean Result { get; set; }
        public T ReturnData { get; set; }
        public String Error { get; set; }
    }
}
