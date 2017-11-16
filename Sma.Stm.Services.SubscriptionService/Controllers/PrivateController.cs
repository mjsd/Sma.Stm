using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.SubscriptionService.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Sma.Stm.Ssc;
using Sma.Stm.Services.SubscriptionService.Models;
using Sma.Stm.Common.Swagger;
using Sma.Stm.Services.SubscriptionService.Services;

namespace Sma.Stm.Services.SubscriptionService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PrivateController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly SubscriptionDbContext _dbContext;

        public PrivateController(IEventBus eventBus,
            SubscriptionDbContext dbContext)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet("subscription")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        public async Task<IActionResult> Get([FromQuery]string dataId)
        {
            try
            {
                var acl = await _dbContext.Subscriptions.Where(x => x.DataId == dataId).ToListAsync();

                var response = new List<SubscriptionObject>();
                foreach (var item in acl)
                {
                    response.Add(new SubscriptionObject
                    {
                        IdentityId = item.OrgId,
                        IdentityName = "",
                        EndpointURL = new Uri(item.CallbackEndpoint)
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("subscription")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        public async Task<IActionResult> Post([FromBody]List<SubscriptionObject> items, [FromQuery]string dataId)
        {
            if (items == null)
            {
                return BadRequest();
            }

            try
            {
                foreach (var item in items)
                {

                    var subscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(x =>
                        x.DataId == dataId && x.OrgId == item.IdentityId && x.CallbackEndpoint == item.EndpointURL.ToString());

                    if (subscription == null && AuthorizationService.CheckAuthentication(item.IdentityId, dataId))
                    {
                        subscription = new SubscriptionItem
                        {
                            DataId = dataId,
                            OrgId = item.IdentityId,
                            ServiceId = "missing",
                            CallbackEndpoint = item.EndpointURL.ToString()
                        };

                        _dbContext.Subscriptions.Add(subscription);
                        await _dbContext.SaveChangesAsync();

                        var newEvent = new NewSubscriptionIntegrationEvent
                        {
                            CallbackEndpoint = subscription.CallbackEndpoint,
                            DataId = subscription.DataId,
                            OrgId = subscription.OrgId,
                            ServiceId = subscription.ServiceId
                        };

                        _eventBus.Publish(newEvent);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("subscription")]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        public async Task<IActionResult> Delete([FromBody]List<SubscriptionObject> items, [FromQuery]string dataId)
        {
            if (items == null)
            {
                return BadRequest();
            }

            try
            {
                foreach (var item in items)
                {
                    var acl = await _dbContext.Subscriptions.FirstOrDefaultAsync(x =>
                        x.DataId == dataId && x.OrgId == item.IdentityId);

                    if (acl != null)
                    {
                        _dbContext.Subscriptions.Remove(acl);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}