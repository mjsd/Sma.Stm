using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.Common.DocumentDb;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.AuthorizationServiceService.Models;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Sma.Stm.Services.AuthorizationService.Controllers
{
    [Route("api/[controller]")]
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
        [Route("Check")]
        public async Task<ActionResult> CheckAuthorization(string orgId, string dataId)
        {
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
        public async Task<ActionResult> Get()
        {
            try
            {
                var acl = await _dbCOntext.Authorizations.ToListAsync();
                return Ok(acl);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var item = await _dbCOntext.Authorizations.Where(x => x.Id2 == id).ToListAsync();
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
                var acl = await _dbCOntext.Authorizations.FirstOrDefaultAsync(x => x.Id2 == item.Id2);
                if (acl == null)
                {
                    _dbCOntext.Authorizations.Add(item);
                }
                else
                {
                    acl.Id2 = item.Id2;
                    acl.DataId = item.DataId;
                    _dbCOntext.Authorizations.Update(acl);
                }


                await _dbCOntext.SaveChangesAsync();
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
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}