using Newtonsoft.Json;
using Sma.Stm.Common.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Linq;

namespace Sma.Stm.Ssc
{
    public class SeaSwimIdentityService
    {
        private List<Organization> _organizations;
        private Timer _updateTimer;

        public SeaSwimIdentityService()
        {
            LoadIdentities();

            _updateTimer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = 60000 * 15
            };
            _updateTimer.Elapsed += _updateTimer_Elapsed;
            _updateTimer.Start();
        }

        public string GetIdentityName(string identityId)
        {
            var org = _organizations.FirstOrDefault(x => x.Mrn == identityId);
            if (org != null)
                return org.Name;

            return "Unknown";
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