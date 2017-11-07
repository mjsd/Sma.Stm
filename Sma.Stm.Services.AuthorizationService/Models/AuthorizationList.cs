using Sma.Stm.Common.DocumentDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.AuthorizationServiceService.Models
{
    public class AuthorizationList : DocumentBase
    {
        public List<AuthorizationItem> Authorizations { get; set; }
    }
}