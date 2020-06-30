using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.ViewModels
{
    public class EquipmentImageVModel
    {
        public long? Id { get; set; }
        public string Url { get; set; }
        public EquipmentVModel Equipment { get; set; }
    }
}
