using System;
using System.Collections.Generic;

namespace CourseworkDB
{
    public partial class Advert
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public int VehicleId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
