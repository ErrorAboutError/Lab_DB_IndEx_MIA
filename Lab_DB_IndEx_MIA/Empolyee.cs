using System;
using System.Collections.Generic;

#nullable disable

namespace Lab_DB_IndEx_MIA
{
    public partial class Empolyee
    {
        public Empolyee()
        {
            Criminals = new HashSet<Criminal>();
        }

        public long Id { get; set; }
        public string FullName { get; set; }
        public long? Age { get; set; }
        public string Male { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Pasport { get; set; }
        public long? IdPost { get; set; }
        public long? IdTitle { get; set; }
        public DateTime dateTime { get; set; }

        public virtual Post IdPostNavigation { get; set; }
        public virtual Title IdTitleNavigation { get; set; }
        public virtual ICollection<Criminal> Criminals { get; set; }
    }
}
