using AgentCustomerOrders.Models;
using AgentCustomerOrders.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AgentCustomerOrders.Controllers
{
    public class AgentController : Controller
    {
        private readonly ILogger<AgentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AgentService _agentService;

        public AgentController (ILogger<AgentController> logger, IConfiguration configuration, AgentService agentService)
        {
            _logger = logger;
            _configuration = configuration;
            _agentService = agentService;
        }
        public IActionResult Index()
        {
            var vm = new AgentViewModel();
            vm.AgentList = _agentService.getAllAgents();
            return View(vm);
        }

        public IActionResult AgentProfile(string id)
        {
            var aCode = id;
            var vm = new AgentDetails();
            vm.agent = _agentService.getAgentDetails(aCode);
            return View(vm);
        }

        [HttpGet]
        public IActionResult AddAgent()
        {
            return (View());
        }

        [HttpPost]
        public IActionResult AddAgent(AgentModel agentModel)
        {
            _agentService.CreateNewAgent(agentModel);
            return RedirectToAction("Index");
        }

    }
}
