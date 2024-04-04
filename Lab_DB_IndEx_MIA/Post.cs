using System;
using System.Collections.Generic;

#nullable disable

namespace Lab_DB_IndEx_MIA
{
    public partial class Post
    {
        public Post()
        {
            Empolyees = new HashSet<Empolyee>();
        }

        public long IdPost { get; set; }
        public string NamePost { get; set; }
        public double? Salary { get; set; }
        public string Responsibilities { get; set; }
        public string Requirements { get; set; }

        public virtual ICollection<Empolyee> Empolyees { get; set; }
    }
}
