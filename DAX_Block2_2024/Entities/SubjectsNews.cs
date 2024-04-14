using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class SubjectsNews
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int NewsId { get; set; }

        public virtual News News { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
