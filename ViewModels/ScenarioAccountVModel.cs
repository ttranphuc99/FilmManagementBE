using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.ViewModels
{
    public class ScenarioAccountVModel
    {
        public ScenarioVModel Scenario { get; set; }
        public AccountVModel Account { get; set; }
        public DateTime CreateTime { get; set; }
        public AccountVModel CreateBy { get; set; }
        public DateTime LastModified { get; set; }
        public AccountVModel LastModifiedBy { get; set; }
        public string Characters { get; set; }
    }
}
