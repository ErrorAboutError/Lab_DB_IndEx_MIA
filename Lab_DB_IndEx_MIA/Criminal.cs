using System;
using System.Collections.Generic;

#nullable disable

namespace Lab_DB_IndEx_MIA
{
    public partial class Criminal
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string DateofBirth { get; set; }
        public string Male { get; set; }
        public string Address { get; set; }
        public long? IdTypeofCrime { get; set; }
        public long? IdVictim { get; set; }
        public string Status { get; set; }
        public long? IdEmpolyees { get; set; }

        public virtual Empolyee IdEmpolyeesNavigation { get; set; }
        public virtual TypesCrime IdTypeofCrimeNavigation { get; set; }
        public virtual Victim IdVictimNavigation { get; set; }
    }
}
