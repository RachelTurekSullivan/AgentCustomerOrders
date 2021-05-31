using AgentCustomerOrders.Models;
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

        public AgentController (ILogger<AgentController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var vm = new AgentViewModel();
            vm.AgentList = getAllAgents();
            return View(vm);
        }

        public IActionResult AgentProfile(string id)
        {
            var aCode = id;
            var vm = new AgentDetails();
            vm.agent = getAgentDetails(aCode);
            return View(vm);
        }

        public IActionResult AddAgent()
        {
            return (View());
        }

        [HttpPost]
        public IActionResult AddAgent(AgentModel agent)
        {



            return RedirectToAction("Index");
        }

        //home/agent/a002
        //public IActionResult Book(int? id)
        //{
        //    // TODO: Use the id passed and go get the book data.
        //    // Use that book data to create a new view.
        //    return View();
        //}


        //public void CreateNewAgent(AgentModel agent)
        //{
        //    string connString = _configuration.GetConnectionString("default");
        //    using (var connection = new SqlConnection(connString))
        //    {
        //        connection.Open();

        //        var cmd = new SqlCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "INSERT INTO Books (Title) VALUES (@title)";
        //        cmd.Parameters.Add(new SqlParameter
        //        {
        //            ParameterName = "@title",
        //            Value = book.Title,
        //            SqlDbType = SqlDbType.NVarChar
        //        });

        //        cmd.Connection = connection;

        //        cmd.ExecuteNonQuery();
        //    }
        //}


        public List<AgentModel> getAllAgents ()
        {
            string connectionString = _configuration.GetConnectionString("default");
            var agentListFromDatabase = new List<AgentModel>();

            using (var conn = new SqlConnection(connectionString))
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
            return agentListFromDatabase;
        }

        public AgentModel getAgentDetails(string aCode)
        {
            string connectionString = _configuration.GetConnectionString("default");
            var agentCode = "";
            var agentName = "";
            var workingArea = "";
            double commission = 0;
            var phoneNo = "";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "Select * from dbo.Agents Where AgentCode = '" + aCode + "'";

                var reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    agentCode = Convert.ToString(reader["AgentCode"]);
                    agentName = Convert.ToString(reader["AgentName"]);
                    workingArea= Convert.ToString(reader["WorkingArea"]);
                    commission= Convert.ToDouble(reader["Commission"]);
                    phoneNo = Convert.ToString(reader["PhoneNo"]);
                   
                }

            }
            return new AgentModel(agentCode,agentName,workingArea,commission,phoneNo);

        }

        //need to finish this
        //public void createNewAgent (AgentModel agent)
        //{
        //    string connectionString = _configuration.GetConnectionString("default");
        //    var agentCode = getNextAgentCode();
    

        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();

        //        var cmd = new SqlCommand();
        //        cmd.Connection = conn;
        //        cmd.CommandType = System.Data.CommandType.Text;

        //        cmd.CommandText = "INSERT INTO dbo.Agents " +
        //            "(AgentCode, AgentName, WorkingArea, Commission, PhoneNo) " +
        //            "Values (@agentCode, @agent.AgentName, @agent.WorkingArea, @agent.Commission, @agent.PhoneNo)";

        //        cmd.Parameters.Add(new SqlParameter
        //        {
        //            ParameterName = "@AgentCode",
        //            Value = agentCode,
        //            SqlDbType = System.Data.SqlDbType.NVarChar
        //        }); 

               
        //    }
          

        //}

        public string getNextAgentCode()
        {
            int numAgents = getAllAgents().Count;
            string newCode = "";

            if (numAgents < 9)
            {
                newCode = "a00" + numAgents.ToString();
            }
            else if (numAgents < 99)
            {
                newCode = "a0" + numAgents.ToString();
            }
            else { newCode = "a" + numAgents.ToString(); }


            return newCode;
        }



    }
}
