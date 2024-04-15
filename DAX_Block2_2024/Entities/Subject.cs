using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class Subject
    {
        public Subject()
        {
            SubjectsNews = new HashSet<SubjectsNews>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubjectsNews> SubjectsNews { get; set; }
    }
}
