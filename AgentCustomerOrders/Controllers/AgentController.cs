using System;
using System.Collections.Generic;
using AgentCustomerOrders.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Diagnostics;
using AgentCustomerOrders.Models;

namespace AgentCustomerOrders.Controllers
{
    public class AgentController : Controller
    {
        private readonly ILogger<AgentController> _logger;

        public AgentController (ILogger<AgentController> logger){
            var _logger = logger;
        }
        public IActionResult Index()
        {
            var agentListFromDatabase = new List<AgentModel>();
            string connectionString = "Server=.;Database=ACO;Trusted_Connection=True;";

            
            using (var conn = new  SqlConnection(connectionString))
            {
                conn.Open();
                
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT * FROM dbo.Agents";

                var reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    var AgentCode = Convert.ToString(reader["AgentCode"]);
                    var AgentName = Convert.ToString(reader["AgentName"]);
                    var WorkingArea = Convert.ToString(reader["WorkingArea"]);
                    var Comission = Convert.ToDouble(reader["Commission"]);
                    var PhoneNo = Convert.ToString(reader["PhoneNo"]);
                    agentListFromDatabase.Add(new AgentModel(AgentCode, AgentName, WorkingArea, Comission, PhoneNo));
                }

            }

            var vm = new AgentViewModel();
            
            vm.AgentList = agentListFromDatabase;

            return View(vm);
        }


           
    }
}
