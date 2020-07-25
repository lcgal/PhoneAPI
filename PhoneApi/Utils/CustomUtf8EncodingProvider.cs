using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PhoneApi.Utils
{
public class CustomUtf8EncodingProvider : EncodingProvider
{
    public override Encoding GetEncoding(string name)
    {
            
        return Encoding.UTF8;
    }

    public override Encoding GetEncoding(int codepage)
    {
        return null;
    }
}
}