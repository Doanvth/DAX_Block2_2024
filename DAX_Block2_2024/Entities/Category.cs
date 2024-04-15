using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class Category
    {
        public Category()
        {
            DocumentsCategories = new HashSet<DocumentsCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DocumentsCategory> DocumentsCategories { get; set; }
    }
}
