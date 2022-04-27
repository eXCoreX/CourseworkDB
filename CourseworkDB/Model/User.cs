using System;
using System.Collections.Generic;

namespace CourseworkDB
{
    public partial class User
    {
        public User()
        {
            Adverts = new HashSet<Advert>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<Advert> Adverts { get; set; }
    }
}
