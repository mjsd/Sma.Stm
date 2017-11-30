using System.Collections.Generic;

namespace Sma.Stm.Common.PlugIns.ExtendedValidation
{
    public interface IExtendedValidator
    {
        IEnumerable<string> Validate(string xml);
    }
}