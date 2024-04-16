using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class CommentNews
    {
        public int Id { get; set; }
        public DateTime? CommentDate { get; set; }
        public string Content { get; set; }
        public int? NewsId { get; set; }
        public int? UsersId { get; set; }

        public virtual News News { get; set; }
        public virtual User Users { get; set; }
    }
}
