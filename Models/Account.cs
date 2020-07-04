using System;
using System.Collections.Generic;

namespace FilmManagement_BE.Models
{
    public partial class Account
    {
        public Account()
        {
            EquipmentCreateBy = new HashSet<Equipment>();
            EquipmentLastModifiedBy = new HashSet<Equipment>();
            ScenarioAccountDetailAccount = new HashSet<ScenarioAccountDetail>();
            ScenarioAccountDetailCreateBy = new HashSet<ScenarioAccountDetail>();
            ScenarioAccountDetailLastModifiedBy = new HashSet<ScenarioAccountDetail>();
            ScenarioCreateBy = new HashSet<Scenario>();
            ScenarioEquipmentDetailCreatedBy = new HashSet<ScenarioEquipmentDetail>();
            ScenarioEquipmentDetailLastModifiedBy = new HashSet<ScenarioEquipmentDetail>();
            ScenarioLastModifiedBy = new HashSet<Scenario>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DeviceToken { get; set; }
        public int? Role { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Equipment> EquipmentCreateBy { get; set; }
        public virtual ICollection<Equipment> EquipmentLastModifiedBy { get; set; }
        public virtual ICollection<ScenarioAccountDetail> ScenarioAccountDetailAccount { get; set; }
        public virtual ICollection<ScenarioAccountDetail> ScenarioAccountDetailCreateBy { get; set; }
        public virtual ICollection<ScenarioAccountDetail> ScenarioAccountDetailLastModifiedBy { get; set; }
        public virtual ICollection<Scenario> ScenarioCreateBy { get; set; }
        public virtual ICollection<ScenarioEquipmentDetail> ScenarioEquipmentDetailCreatedBy { get; set; }
        public virtual ICollection<ScenarioEquipmentDetail> ScenarioEquipmentDetailLastModifiedBy { get; set; }
        public virtual ICollection<Scenario> ScenarioLastModifiedBy { get; set; }
    }
}
