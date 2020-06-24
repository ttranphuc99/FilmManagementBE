using System;
using System.Collections.Generic;

namespace FilmManagement_BE.Models
{
    public partial class ScenarioAccountDetail
    {
        public long ScenarioId { get; set; }
        public int AccountId { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateById { get; set; }
        public DateTime? LastModified { get; set; }
        public int? LastModifiedById { get; set; }
        public string Characters { get; set; }

        public virtual Account Account { get; set; }
        public virtual Account LastModifiedBy { get; set; }
        public virtual Scenario Scenario { get; set; }
    }
}
