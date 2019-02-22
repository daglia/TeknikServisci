﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServisci.Models.Abstracts;
using TeknikServisci.Models.Enums;
using TeknikServisci.Models.IdentityModels;

namespace TeknikServisci.Models.Entities
{
    public class Operation : BaseEntity2<int,int>
    {
        public string Message { get; set; }
        public IdentityRoles FromWhom { get; set; }

        public string ClientId { get; set; }
        public int FailureId { get; set; }

        [ForeignKey("ClientId")]
        public virtual User Client { get; set; }
        [ForeignKey("FailureId")]
        public virtual Failure Failure { get; set; }
    }
}
