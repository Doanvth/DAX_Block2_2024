using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class News
    {
        public News()
        {
            CommentNews = new HashSet<CommentNews>();
            SubjectsNews = new HashSet<SubjectsNews>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool? Status { get; set; }

        public virtual User CreateByNavigation { get; set; }
        public virtual ICollection<CommentNews> CommentNews { get; set; }
        public virtual ICollection<SubjectsNews> SubjectsNews { get; set; }
    }
}
