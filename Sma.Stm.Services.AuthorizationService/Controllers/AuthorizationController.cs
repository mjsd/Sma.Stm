using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.Common.DocumentDb;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.AuthorizationServiceService.Models;
using Sma.Stm.Services.AuthorizationServiceService.IntegrationEvents.Events;

namespace Sma.Stm.Services.AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly DocumentDbRepository<AuthorizationList> _authorizationRepository;

        public AuthorizationController(DocumentDbRepository<AuthorizationList> authorizationRepository, IEventBus eventBus)
        {
            _authorizationRepository = authorizationRepository;
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpGet]
        [Route("Check")]
        public async Task<ActionResult> CheckAuthorization(string orgId, string dataId)
        {
            try
            {
                var acl = await _authorizationRepository.GetItemAsync(orgId);
                if (acl != null && acl.Authorizations.FirstOrDefault(x=>x.DataId == dataId) != null)
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
        public async Task<ActionResult> Get()
        {
            try
            {
                var items = await _authorizationRepository.GetItemsAsync(x => x != null);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            try
            {
                var item = await _authorizationRepository.GetItemAsync(id);
                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthorizationItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            try
            {
                var acl = await _authorizationRepository.GetItemAsync(item.Id);
                if (acl == null)
                {
                    acl = new AuthorizationList
                    {
                        Id = item.Id,
                        Authorizations = new List<AuthorizationItem>
                        {
                            item
                        }
                    };
                    await _authorizationRepository.CreateItemAsync(acl);
                }
                else
                {
                    var existing = acl.Authorizations.FirstOrDefault(x => x.DataId == item.DataId);
                    if (existing == null)
                    {
                        acl.Authorizations.Add(item);
                    }

                    await _authorizationRepository.UpdateItemAsync(acl.Id, acl);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] AuthorizationItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            try
            {
                var acl = await _authorizationRepository.GetItemAsync(item.Id);
                if (acl == null || acl.Authorizations.FirstOrDefault(x => x.DataId == item.DataId) == null)
                {
                    return Ok("Not found");
                }

                acl.Authorizations.Remove(acl.Authorizations.FirstOrDefault(x => x.DataId == item.DataId));
                await _authorizationRepository.UpdateItemAsync(acl.Id, acl);

                var @event = new AuthorizationRemovedIntegrationEvent
                {
                    DataId = item.DataId,
                    OrgId = item.Id
                };
                _eventBus.Publish(@event);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}