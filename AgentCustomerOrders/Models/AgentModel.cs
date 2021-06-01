using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AgentCustomerOrders.Models
{
    public class AgentModel
    {
        //[BindProperty]
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string WorkingArea { get; set; }
        public double Commission { get; set; }
        public string PhoneNo { get; set; }

        public AgentModel() { }

        //spelled commission wrong and now have to have it like this for binding to work 
        //or I can go change the database and then fix it everywhere...
        public AgentModel(string agentCode, string agentName, string workingArea, double commision, string phoneNo)
        {
            AgentCode = agentCode;
            AgentName = agentName;
            WorkingArea = workingArea;
            Commission = commision;
            PhoneNo = phoneNo;
        }

         //this is for creating a new agent.  The ID will be generated elsewhere       

    }


}


