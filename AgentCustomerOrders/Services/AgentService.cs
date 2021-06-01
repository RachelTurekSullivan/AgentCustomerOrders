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
                var code = getNextAgentCode(numAgents);
                agent.AgentCode = code.ToString();

                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.Agents (AgentCode, AgentName, WorkingArea, Commission, PhoneNo) " +
                                    "VALUES (@AgentCode, @AgentName, @WorkingArea, @Commission, @PhoneNo)";
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@AgentCode",
                    Value = agent.AgentCode,
                    SqlDbType = SqlDbType.NVarChar
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@AgentName",
                    Value = agent.AgentName,
                    SqlDbType = SqlDbType.NVarChar
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@WorkingArea",
                    Value = agent.WorkingArea,
                    SqlDbType = SqlDbType.NVarChar
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Commission",
                    Value = agent.Commission,
                    SqlDbType = SqlDbType.Decimal
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PhoneNo",
                    Value = agent.PhoneNo,
                    SqlDbType = SqlDbType.NVarChar
                });

                cmd.ExecuteNonQuery();
            }
        }


        public void editExistingAgent(AgentModel agent){
            string connectionString = _configuration.GetConnectionString("default");


            using (var conn = new SqlConnection(connectionString))
            {
            conn.Open();

             var getAgentCount = new SqlCommand();
            getAgentCount.Connection = conn;
            getAgentCount.CommandType = System.Data.CommandType.Text;
            getAgentCount.CommandText = "SELECT count(AgentCode) FROM dbo.Agents";

            var numAgents = (Int32)getAgentCount.ExecuteScalar();
            var code = getNextAgentCode(numAgents);
            agent.AgentCode = code.ToString();

            var cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "UPDATE dbo.Agents SET AgentName=@AgentName, WorkingArea = @WorkingArea, Commision= @Comission, PhoneNo = @PhoneNo) " +
                               "WHERE AgentCode=@AgentCode";
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@AgentCode",
                Value = agent.AgentCode,
                SqlDbType = SqlDbType.NVarChar
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@AgentName",
                Value = agent.AgentName,
                SqlDbType = SqlDbType.NVarChar
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@WorkingArea",
                Value = agent.WorkingArea,
                SqlDbType = SqlDbType.NVarChar
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Commission",
                Value = agent.Commission,
                SqlDbType = SqlDbType.Decimal
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@PhoneNo",
                Value = agent.PhoneNo,
                SqlDbType = SqlDbType.NVarChar
            });

            cmd.ExecuteNonQuery();
        }
    }



        public string getNextAgentCode(int count)
        {
            int numAgents = count+1;
            string newCode = "";
            if (numAgents < 9)
            {
                newCode = "A00" + numAgents.ToString();
            }
            else if (numAgents < 99)
            {
                newCode = "A0" + numAgents.ToString();
            }
            else { newCode = "A" + numAgents.ToString(); }
            return newCode;
        }


    }
}
