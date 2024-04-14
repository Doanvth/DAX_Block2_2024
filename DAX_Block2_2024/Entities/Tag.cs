using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            DocumentsTags = new HashSet<DocumentsTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DocumentsTag> DocumentsTags { get; set; }
    }
}
