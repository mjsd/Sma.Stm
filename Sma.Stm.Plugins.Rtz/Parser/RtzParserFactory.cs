using System;

namespace Sma.Stm.Plugins.Rtz.Parser
{
    public static class RtzParserFactory
    {
        public static IRtzParser Create(string xml)
        {
            if (xml.Contains("xmlns=\"http://www.cirm.org/RTZ/1/0\""))
            {
                return new Rtz10Parser(xml);
            }

            if (xml.Contains("xmlns=\"http://www.cirm.org/RTZ/1/1\""))
            {
                return new Rtz11Parser(xml);
            }

            throw new Exception("Invalid RTZ");
        }
    }
}