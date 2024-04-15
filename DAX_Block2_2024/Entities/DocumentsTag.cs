using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class DocumentsTag
    {
        public int Id { get; set; }
        public int TagsId { get; set; }
        public int DocumentId { get; set; }

        public virtual Document Document { get; set; }
        public virtual Tag Tags { get; set; }
    }
}
