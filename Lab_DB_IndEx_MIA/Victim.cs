using System;
using System.Collections.Generic;

#nullable disable

namespace Lab_DB_IndEx_MIA
{
    public partial class Victim
    {
        public Victim()
        {
            Criminals = new HashSet<Criminal>();
        }

        public long Id { get; set; }
        public string FullName { get; set; }
        public string DateBirth { get; set; }
        public string Male { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Criminal> Criminals { get; set; }
    }
}
