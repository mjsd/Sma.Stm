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

namespace Sma.Stm.Services.SubscriptionService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PublicController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly SubscriptionDbContext _dbContext;
        private readonly SeaSwimInstanceContextService _seaSwimInstanceContextService;

        public PublicController(IEventBus eventBus,
            SubscriptionDbContext dbContext,
            SeaSwimInstanceContextService seaSwimInstanceContextService)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _seaSwimInstanceContextService = seaSwimInstanceContextService ?? throw new ArgumentNullException(nameof(seaSwimInstanceContextService));
        }

        [HttpGet("Subscription")]
        public async Task<IActionResult> GetByEndpoint([FromQuery]string callbackEndpoint)
        {
            try
            {
                var items = await _dbContext.Subscriptios.Where(x => 
                    x.OrgId == _seaSwimInstanceContextService.CallerOrgId 
                    && x.CallbackEndpoint.ToLower() == callbackEndpoint.ToLower()).ToListAsync();

                if (items.Count() == 0)
                    return NotFound();

                var response = new List<GetSubscriptionResponseObj>();

                foreach (var item in items)
                {
                    response.Add(new GetSubscriptionResponseObj
                    {
                        DataId = item.DataId
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("Subscription")]
        public async Task<IActionResult> Post([FromQuery]string callbackEndpoint, [FromQuery]string uvid)
        {
            try
            {
                // TODO: check authorization

                var items = await _dbContext.Subscriptios.Where(x => 
                    x.OrgId == _seaSwimInstanceContextService.CallerOrgId && x.DataId == uvid
                    && x.CallbackEndpoint.ToLower() == callbackEndpoint.ToLower()).ToListAsync();

                if (items == null || items.Count() == 0)
                {
                    _dbContext.Subscriptios.Add(new Models.SubscriptionItem
                    {
                        DataId = uvid,
                        OrgId = _seaSwimInstanceContextService.CallerOrgId,
                        CallbackEndpoint = callbackEndpoint
                    });
                }
                else
                {
                    return Ok($"Subscription for {_seaSwimInstanceContextService.CallerOrgId} on {uvid} already exists");
                }

                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Subscription")]
        public async Task<IActionResult> Delete([FromQuery]string callbackEndpoint, [FromQuery]string uvid)
        {
            try
            {
                var item = await _dbContext.Subscriptios.FirstOrDefaultAsync(x =>
                    x.OrgId == _seaSwimInstanceContextService.CallerOrgId && x.DataId == uvid
                    && x.CallbackEndpoint.ToLower() == callbackEndpoint.ToLower());

                if (item == null)
                {
                    return NotFound();
                }

                _dbContext.Subscriptios.Remove(item);
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