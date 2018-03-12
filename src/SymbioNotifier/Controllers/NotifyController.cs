using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SymbioNotifier.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SymbioNotifier.Controllers
{
    [Route("api/[controller]")]
    public class NotifyController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotifyController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public object Get()
        {
            if (Request.Headers["$secret"] == "ThisReallyIsSymbio")
            {
                var message = Request.Headers["$message"];

                _notificationService?.SendNotificationAsync(message);

                Response.StatusCode = 200; // OK
                return new { success = true };
            }

            Response.StatusCode = 401; // Unauthorized
            return new { success = false };
        }
    }
}
