using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AgentCustomerOrders.Models
{
    public class AgentModel
    {
        [BindProperty]
        public string _AgentCode { get; set; }
        public string _AgentName { get; set; }
        public string _WorkingaArea { get; set; }
        public double _Commission { get; set; }
        public string _PhoneNo { get; set; }

        public AgentModel() { }

        public AgentModel(string agentCode, string agentName, string workingArea, double commision, string phoneNo)
        {
            _AgentCode = agentCode;
            _AgentName = agentName;
            _WorkingaArea = workingArea;
            _Commission = commision;
            _PhoneNo = phoneNo;
        }


    }


}


