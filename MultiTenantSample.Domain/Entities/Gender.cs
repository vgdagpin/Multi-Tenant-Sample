using MultiTenantSample.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultiTenantSample.Domain.Entities
{
    public class Gender : Entity
    {
        public string Name { get; set; }
    }
}
