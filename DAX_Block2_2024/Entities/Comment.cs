using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class Comment
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Content { get; set; }
        public int DocumentId { get; set; }
        public int UsersId { get; set; }

        public virtual Document Document { get; set; }
        public virtual User Users { get; set; }
    }
}
