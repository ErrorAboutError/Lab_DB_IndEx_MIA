using System;
using System.Collections.Generic;

#nullable disable

namespace Lab_DB_IndEx_MIA
{
    public partial class TypesCrime
    {
        public TypesCrime()
        {
            Criminals = new HashSet<Criminal>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public string Punishment { get; set; }
        public string Term { get; set; }

        public virtual ICollection<Criminal> Criminals { get; set; }
    }
}
