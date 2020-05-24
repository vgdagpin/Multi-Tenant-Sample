using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSample.Application.Personnels.Queries.Models
{
    public class PersonnelBO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }

        public string Gender { get; set; }
    }
}
