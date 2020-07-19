using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneApi.Models.Responses
{
    public class BooleanResponse
    {
        public bool Data { get; set; }

        public string Error { get; set; }
    }
}