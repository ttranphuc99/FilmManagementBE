using System;
using System.Collections.Generic;

namespace FilmManagement_BE.Models
{
    public partial class EquipmentImage
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public long? EquipmentId { get; set; }

        public virtual Equipment Equipment { get; set; }
    }
}
