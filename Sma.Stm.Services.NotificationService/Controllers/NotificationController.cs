using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using Sma.Stm.Common.Swagger;
using Microsoft.EntityFrameworkCore;

namespace Sma.Stm.Services.NotificationService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NotificationController : Controller
    {
        private readonly IEventBus _eventBus;
        private readonly NotificationDbContext _dbContext;

        public NotificationController(NotificationDbContext dbCOntext, IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbCOntext ?? throw new ArgumentNullException(nameof(dbCOntext));
        }

        [HttpGet]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var notifications = await _dbContext.Notifications.Where(x => !x.Fetched).ToListAsync();

                notifications.ForEach(x => x.Fetched = true);
                _dbContext.SaveChanges();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}