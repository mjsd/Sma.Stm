using System;
using System.Collections.Generic;
using System.Text;

namespace Sma.Stm.Common.PlugIns.ExtendedValidation
{
    public interface IExtendedValidator
    {
        List<string> Validate(string xml);
    }
}