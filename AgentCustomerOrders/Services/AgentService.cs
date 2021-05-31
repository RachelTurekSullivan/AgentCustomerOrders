using AgentCustomerOrders.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AgentCustomerOrders.Services
{
    public class AgentService
    {
        private readonly IConfiguration _configuration;

        public AgentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<AgentModel> getAllAgents()
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
                    workingArea = Convert.ToString(reader["WorkingArea"]);
                    commission = Convert.ToDouble(reader["Commission"]);
                    phoneNo = Convert.ToString(reader["PhoneNo"]);

                }

            }
            return new AgentModel(agentCode, agentName, workingArea, commission, phoneNo);
        }

        public void CreateNewAgent(AgentModel agent)
        {
            string connectionString = _configuration.GetConnectionString("default");

            
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();


                var getAgentCount = new SqlCommand();
                getAgentCount.Connection = conn;
                getAgentCount.CommandType = System.Data.CommandType.Text;
                getAgentCount.CommandText = "SELECT count(AgentCode) FROM dbo.Agents";

                var numAgents = (Int32)getAgentCount.ExecuteScalar();
                var newAgentCode = getNextAgentCode(numAgents);


                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.Agents (AgentCode, AgentName, WorkingArea, Commission, PhoneNo) " +
                                    "VALUES (@newAgentCode, @agent._AgentName, @agent._WorkingArea, @agent._Commission, @agents._PhoneNo)";
               cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@AgentCode",
                    Value = newAgentCode, 
                   SqlDbType = SqlDbType.NVarChar
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@AgentName",
                    Value = agent._AgentName,
                    SqlDbType = SqlDbType.NVarChar
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@WorkingArea",
                    Value = agent._WorkingArea,
                    SqlDbType = SqlDbType.NVarChar
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Commission",
                    Value = agent._Commission,
                    SqlDbType = SqlDbType.Int
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PhoneNo",
                    Value = agent._PhoneNo,
                    SqlDbType = SqlDbType.NVarChar
                });

                cmd.ExecuteNonQuery();
            }
        }



        public string getNextAgentCode(int count)
        {
            int numAgents = count;
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
