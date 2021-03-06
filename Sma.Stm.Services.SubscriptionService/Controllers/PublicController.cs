﻿using System;
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
using Sma.Stm.Services.SubscriptionService.Services;

namespace Sma.Stm.Services.SubscriptionService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PublicController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly SubscriptionDbContext _dbContext;
        private readonly SeaSwimInstanceContextService _seaSwimInstanceContextService;
        private readonly SeaSwimIdentityService _seaSwimIdentityService;

        public PublicController(IEventBus eventBus,
            SubscriptionDbContext dbContext,
            SeaSwimInstanceContextService seaSwimInstanceContextService,
            SeaSwimIdentityService seaSwimIdentityService)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _seaSwimInstanceContextService = seaSwimInstanceContextService ?? throw new ArgumentNullException(nameof(seaSwimInstanceContextService));
            _seaSwimIdentityService = seaSwimIdentityService ?? throw new ArgumentNullException(nameof(seaSwimIdentityService));
        }

        [HttpGet("subscription")]
        public async Task<IActionResult> GetByEndpoint([FromQuery]string callbackEndpoint)
        {
            try
            {
                var items = await _dbContext.Subscriptions.Where(x => 
                    x.OrgId == _seaSwimInstanceContextService.CallerOrgId 
                    && x.CallbackEndpoint.ToLower() == callbackEndpoint.ToLower()).ToListAsync();

                if (!items.Any())
                    return NotFound();

                var response = new List<GetSubscriptionResponseObj>();

                foreach (var item in items)
                {
                    response.Add(new GetSubscriptionResponseObj
                    {
                        DataId = item.DataId,
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
        public async Task<IActionResult> Post([FromQuery]string callbackEndpoint, [FromQuery]string dataId)
        {
            try
            {
                if (!AuthorizationService.CheckAuthentication(_seaSwimInstanceContextService.CallerOrgId, dataId))
                {
                    var orgName = _seaSwimIdentityService.GetIdentityName(_seaSwimInstanceContextService.CallerOrgId);
                    var @event = new NotificationIntegrationEvent
                    {
                        FromOrgId = _seaSwimInstanceContextService.CallerOrgId,
                        FromOrgName = orgName,
                        FromServiceId = _seaSwimInstanceContextService.CallerServiceId,
                        NotificationCreatedAt = DateTime.UtcNow,
                        NotificationType = EnumNotificationType.UnauthorizedRequest,
                        Subject = "Unauthorized subscription request",
                        NotificationSource = EnumNotificationSource.Vis,
                        Body = $"Organization {orgName} {_seaSwimInstanceContextService.CallerOrgId} is not authorized to {dataId}"
                    };
                    _eventBus.Publish(@event);

                    return Unauthorized();
                }

                var items = await _dbContext.Subscriptions.Where(x => 
                    x.OrgId == _seaSwimInstanceContextService.CallerOrgId && x.DataId == dataId
                    && string.Equals(x.CallbackEndpoint, callbackEndpoint, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();

                if (items == null || !items.Any())
                {
                    _dbContext.Subscriptions.Add(new Models.SubscriptionItem
                    {
                        DataId = dataId,
                        OrgId = _seaSwimInstanceContextService.CallerOrgId,
                        ServiceId = _seaSwimInstanceContextService.CallerServiceId,
                        CallbackEndpoint = callbackEndpoint
                    });

                    var newEvent = new NewSubscriptionIntegrationEvent
                    {
                        CallbackEndpoint = callbackEndpoint,
                        DataId = dataId,
                        OrgId = _seaSwimInstanceContextService.CallerOrgId,
                        ServiceId = _seaSwimInstanceContextService.CallerServiceId
                    };

                    _eventBus.Publish(newEvent);
                }
                else
                {
                    return Ok($"Subscription for {_seaSwimInstanceContextService.CallerOrgId} on {dataId} already exists");
                }

                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("subscription")]
        public async Task<IActionResult> Delete([FromQuery]string callbackEndpoint, [FromQuery]string dataId)
        {
            try
            {
                var item = await _dbContext.Subscriptions.FirstOrDefaultAsync(x =>
                    x.OrgId == _seaSwimInstanceContextService.CallerOrgId && x.DataId == dataId
                    && string.Equals(x.CallbackEndpoint, callbackEndpoint, StringComparison.CurrentCultureIgnoreCase));

                if (item == null)
                {
                    return NotFound();
                }

                _dbContext.Subscriptions.Remove(item);
                _dbContext.SaveChanges();

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}