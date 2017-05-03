using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        public AppController(IMailService mailService, IConfigurationRoot config)
        {
            _mailService = mailService;
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
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
