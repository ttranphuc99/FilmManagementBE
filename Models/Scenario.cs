using System;
using System.Collections.Generic;

namespace FilmManagement_BE.Models
{
    public partial class Scenario
    {
        public Scenario()
        {
            ScenarioAccountDetail = new HashSet<ScenarioAccountDetail>();
            ScenarioEquipmentDetail = new HashSet<ScenarioEquipmentDetail>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? RecordQuantity { get; set; }
        public string Script { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateById { get; set; }
        public DateTime? LastModified { get; set; }
        public int? LastModifiedById { get; set; }

        public virtual Account CreateBy { get; set; }
        public virtual Account LastModifiedBy { get; set; }
        public virtual ICollection<ScenarioAccountDetail> ScenarioAccountDetail { get; set; }
        public virtual ICollection<ScenarioEquipmentDetail> ScenarioEquipmentDetail { get; set; }
    }
}
