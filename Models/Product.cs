﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quality_Control_EF.Models
{
    public partial class Product
    {
        public long Id { get; set; }
        public long LabbookId { get; set; }
        public string Name { get; set; }
        public string HpIndex { get; set; }
        public string Description { get; set; }
        public bool? IsDanger { get; set; }
        public bool? IsArchive { get; set; }
        public bool? IsExperimetPhase { get; set; }
        public long ProductPriceId { get; set; }
        public long ProductTypeId { get; set; }
        public long ProductGlossId { get; set; }
        public DateTime Created { get; set; }
        public long LoginId { get; set; }
        public virtual User User { get; set; }

        public string ActiveFields { get; set; }
        public bool Modified { get; set; } = false;
    }
}
