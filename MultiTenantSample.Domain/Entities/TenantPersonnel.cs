using MultiTenantSample.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultiTenantSample.Domain.Entities
{
    public class TenantPersonnel : Entity
    {
        public int? TenantId { get; set; }
        public virtual string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public DateTime DOB { get; set; }
        public bool Active { get; set; }
        public int PrefixId { get; set; }
        public Gender GenderFk { get; set; }
    }
}
