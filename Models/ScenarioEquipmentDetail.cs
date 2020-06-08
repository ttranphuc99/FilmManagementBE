using System;
using System.Collections.Generic;

namespace FilmManagement_BE.Models
{
    public partial class ScenarioEquipmentDetail
    {
        public long ScenarioId { get; set; }
        public long EquipmentId { get; set; }
        public int? Quantity { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? LastModified { get; set; }
        public int? LastModifiedById { get; set; }

        public virtual Account CreatedBy { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Account LastModifiedBy { get; set; }
        public virtual Scenario Scenario { get; set; }
    }
}
