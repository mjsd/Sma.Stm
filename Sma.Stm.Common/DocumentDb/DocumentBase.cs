using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sma.Stm.Common.DocumentDb
{
    public abstract class DocumentBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
