using Newtonsoft.Json;
using Sma.Stm.Common.Web;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using Sma.Stm.Ssc.Contract;

namespace Sma.Stm.Ssc
{
    public class SeaSwimIdentityService
    {
        private List<Organization> _organizations;

        public SeaSwimIdentityService()
        {
            LoadIdentities();

            var updateTimer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = 60000 * 15
            };
            updateTimer.Elapsed += _updateTimer_Elapsed;
            updateTimer.Start();
        }

        public string GetIdentityName(string identityId)
        {
            var org = _organizations.FirstOrDefault(x => x.Mrn == identityId);
            return org != null ? org.Name : "Unknown";
        }

        private void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LoadIdentities();
        }

        private void LoadIdentities()
        {
            var response = WebRequestHelper.Get("http://sma.stm.ssc.services.private/api/v1/scc/findIdentities");
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Unable to get identities from Id registry");
            }

            try
            {
                var findIdentitiesObject = JsonConvert.DeserializeObject<FindIdentitiesResponseObj>(response.Body);
                _organizations = findIdentitiesObject.Organizations;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get identities from Id registry", ex);
            }
        }
    }
}