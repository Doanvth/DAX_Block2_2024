using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class DocumentsCategory
    {
        public int Id { get; set; }
        public int CategoriesId { get; set; }
        public int DocumentId { get; set; }

        public virtual Category Categories { get; set; }
        public virtual Document Document { get; set; }
    }
}
