using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmManagement_BE.ViewModels
{
    public class EquipmentVModel
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public bool? Status { get; set; }
        public AccountVModel CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public AccountVModel LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public ICollection<EquipmentImageVModel> ListImages { get; set; }
    }
}
