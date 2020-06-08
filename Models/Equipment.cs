using System;
using System.Collections.Generic;

namespace FilmManagement_BE.Models
{
    public partial class Equipment
    {
        public Equipment()
        {
            EquipmentImage = new HashSet<EquipmentImage>();
            ScenarioEquipmentDetail = new HashSet<ScenarioEquipmentDetail>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public bool? Status { get; set; }
        public int? CreateById { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual Account CreateBy { get; set; }
        public virtual Account LastModifiedBy { get; set; }
        public virtual ICollection<EquipmentImage> EquipmentImage { get; set; }
        public virtual ICollection<ScenarioEquipmentDetail> ScenarioEquipmentDetail { get; set; }
    }
}
