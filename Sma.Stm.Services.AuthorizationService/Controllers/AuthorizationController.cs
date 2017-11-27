using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.AuthorizationService.Models;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Sma.Stm.Common.Swagger;
using Sma.Stm.Ssc;

namespace Sma.Stm.Services.AuthorizationService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly AuthorizationDbContext _dbContext;
        private readonly SeaSwimIdentityService _seaSwimIdentityService;

        public AuthorizationController(AuthorizationDbContext dbCOntext, 
            IEventBus eventBus,
            SeaSwimIdentityService seaSwimIdentityService)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
            _seaSwimIdentityService = seaSwimIdentityService ?? throw new ArgumentNullException(nameof(seaSwimIdentityService));
        }

        [HttpGet]
        [Route("check")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        public async Task<IActionResult> CheckAuthorization(string orgId, string dataId)
        {
            try
            {
                var acl = await _dbContext.Authorizations.FirstOrDefaultAsync(x => x.OrgId == orgId && x.DataId == dataId);

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
                var acl = await _dbContext.Authorizations.Where(x => x.DataId == dataId).ToListAsync();

                var response = new List<IdentityDescriptionObject>();
                foreach (var item in acl)
                {
                    var orgName = _seaSwimIdentityService.GetIdentityName(item.OrgId);

                    response.Add(new IdentityDescriptionObject
                    {
                        IdentityId = item.OrgId,
                        IdentityName = orgName
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
                    var acl = await _dbContext.Authorizations.FirstOrDefaultAsync(x =>
                        x.DataId == dataId && x.OrgId == item.IdentityId);

                    if (acl == null)
                    {
                        _dbContext.Authorizations.Add(new AuthorizationItem
                        {
                            DataId = dataId,
                            OrgId = item.IdentityId
                        });
                        await _dbContext.SaveChangesAsync();
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
                    var acl = await _dbContext.Authorizations.FirstOrDefaultAsync(x =>
                        x.DataId == dataId && x.OrgId == item.IdentityId);

                    if (acl != null)
                    {
                        _dbContext.Authorizations.Remove(acl);
                        await _dbContext.SaveChangesAsync();

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