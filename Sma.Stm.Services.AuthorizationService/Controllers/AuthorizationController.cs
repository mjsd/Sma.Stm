using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.AuthorizationServiceService.Models;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Sma.Stm.Common.Swagger;

namespace Sma.Stm.Services.AuthorizationService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly AuthorizationDbContext _dbCOntext;

        public AuthorizationController(AuthorizationDbContext dbCOntext, IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbCOntext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
        }

        [HttpGet]
        [Route("check")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        public async Task<IActionResult> CheckAuthorization(string orgId, string dataId)
        {
            var text = System.IO.File.ReadAllText("./appsettings.json");
            text = text.Replace("EventBusConnection", "MattiasWasHere");
            System.IO.File.WriteAllText("./appsettings.json", text);

            try
            {
                var acl = await _dbCOntext.Authorizations.FirstOrDefaultAsync(x => x.OrgId == orgId && x.DataId == dataId);

                if (acl != null)
                {
                    return Ok(true);
                }

                return Ok(false);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        public async Task<IActionResult> Get([FromQuery]string dataId)
        {
            try
            {
                var acl = await _dbCOntext.Authorizations.Where(x => x.DataId == dataId).ToListAsync();

                var response = new List<IdentityDescriptionObject>();
                foreach (var item in acl)
                {
                    response.Add(new IdentityDescriptionObject
                    {
                        IdentityId = item.OrgId,
                        IdentityName = ""
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        public async Task<IActionResult> Post([FromBody]List<IdentityDescriptionObject> items, [FromQuery]string dataId)
        {
            if (items == null)
            {
                return BadRequest();
            }

            try
            {
                foreach (var item in items)
                {
                    var acl = await _dbCOntext.Authorizations.FirstOrDefaultAsync(x =>
                        x.DataId == dataId && x.OrgId == item.IdentityId);

                    if (acl == null)
                    {
                        _dbCOntext.Authorizations.Add(new AuthorizationItem
                        {
                            DataId = dataId,
                            OrgId = item.IdentityId
                        });
                        await _dbCOntext.SaveChangesAsync();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        public async Task<IActionResult> Delete([FromBody]List<IdentityDescriptionObject> items, [FromQuery]string dataId)
        {
            if (items == null)
            {
                return BadRequest();
            }

            try
            {
                foreach (var item in items)
                {
                    var acl = await _dbCOntext.Authorizations.FirstOrDefaultAsync(x =>
                        x.DataId == dataId && x.OrgId == item.IdentityId);

                    if (acl != null)
                    {
                        _dbCOntext.Authorizations.Remove(acl);
                        await _dbCOntext.SaveChangesAsync();

                        var newEvent = new AuthorizationRemovedIntegrationEvent
                        {
                            DataId = acl.DataId,
                            OrgId = acl.OrgId
                        };

                        _eventBus.Publish(newEvent);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}