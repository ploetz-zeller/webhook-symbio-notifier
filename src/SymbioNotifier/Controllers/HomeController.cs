using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SymbioNotifier.Models;
using SymbioNotifier.Services;

namespace SymbioNotifier.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotificationService _notificationService;

        public HomeController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            IndexModel model = null;
            if (User.Identity.IsAuthenticated)
            {
                model = new IndexModel
                {
                    IsSubscribed = _notificationService?.IsSubscribed(User) ?? false
                };

                model.Messages.AddRange(_notificationService?.GetCollectedMessages(User) ?? new string[0]);
            }

            return View(model);
        }

        public IActionResult Subscribe()
        {
            _notificationService?.Subscribe(User);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Unsubscribe()
        {
            _notificationService?.Unsubscribe(User);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
