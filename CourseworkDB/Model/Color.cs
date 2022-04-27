using System;
using System.Collections.Generic;

namespace CourseworkDB
{
    public partial class Color
    {
        public Color()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
