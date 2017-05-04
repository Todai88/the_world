using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IWorldRepository _repository;
        private ILogger _logger;
        public AppController(IMailService mailService, IConfigurationRoot config, IWorldRepository context, ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            //try
            //{
            //    var data = _repository.GetAllTrips();

            //    return View(data);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Failed to get trips in Index page : { ex.Message}");
            //    return Redirect("/error");
            //}

            return View();
            
        }

        [Authorize]
        public IActionResult Trips()
        {
            try
            {
                var trips = _repository.GetAllTrips();

                return View(trips);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get trips in Index page : { ex.Message}");
                return Redirect("/error");
            }
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com")) ModelState.AddModelError("Email", "We don't support AOL addresses. Try another address!");

            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Name, "Some subject", model.Message);
                ViewBag.UserMessage = "Message sent";
                ModelState.Clear();
            }
            
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
