using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.ViewModels
{
    public class ScenarioVModel
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? RecordQuantity { get; set; }
        public string Script { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public AccountVModel CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public AccountVModel LastModifiedBy { get; set; }
    }
}
