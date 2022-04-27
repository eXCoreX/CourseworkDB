using System;
using System.Collections.Generic;

namespace CourseworkDB
{
    public partial class Vehicle
    {
        public int Id { get; set; }
        public int Mileage { get; set; }
        public int ProductionYear { get; set; }
        public float Power { get; set; }
        public float Displacement { get; set; }
        public int ModelId { get; set; }
        public int TransmissionId { get; set; }
        public int BodyId { get; set; }
        public int FuelTypeId { get; set; }
        public int ColorId { get; set; }

        public virtual Body Body { get; set; } = null!;
        public virtual Color Color { get; set; } = null!;
        public virtual FuelType FuelType { get; set; } = null!;
        public virtual Model Model { get; set; } = null!;
        public virtual Transmission Transmission { get; set; } = null!;
        public virtual Advert Advert { get; set; } = null!;
    }
}
