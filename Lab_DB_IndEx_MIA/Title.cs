using System;
using System.Collections.Generic;

#nullable disable

namespace Lab_DB_IndEx_MIA
{
    public partial class Title
    {
        public Title()
        {
            Empolyees = new HashSet<Empolyee>();
        }

        public long IdTitle { get; set; }
        public string NameTitle { get; set; }
        public double? Surcharge { get; set; }
        public string Responsibilities { get; set; }
        public string Requirements { get; set; }

        public virtual ICollection<Empolyee> Empolyees { get; set; }
    }
}
