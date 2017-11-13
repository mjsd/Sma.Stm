using Sma.Stm.Common.DocumentDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.AuthorizationServiceService.Models
{
    public class AuthorizationItem
    {
        public string Id { get; set; }
        public int Id2 { get; set; }

        public string OrgId { get; set; }

        public string DataId { get; set; }
    }
}