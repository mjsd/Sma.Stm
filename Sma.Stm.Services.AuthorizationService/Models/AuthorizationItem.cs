using Sma.Stm.Common.DocumentDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.AuthorizationServiceService.Models
{
    public class AuthorizationItem : DocumentBase
    {
        public string Name { get; set; }

        public string DataId { get; set; }
    }
}