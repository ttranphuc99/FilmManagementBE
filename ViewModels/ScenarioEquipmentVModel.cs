using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.ViewModels
{
    public class ScenarioEquipmentVModel
    {
        public ScenarioVModel Scenario { get; set; }
        public EquipmentVModel Equipment { get; set; }
        public int? Quantity { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public AccountVModel CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public AccountVModel LastModifiedBy { get; set; }
    }
}
