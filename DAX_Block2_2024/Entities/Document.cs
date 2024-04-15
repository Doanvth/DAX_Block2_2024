using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class Document
    {
        public Document()
        {
            Comments = new HashSet<Comment>();
            DocumentsCategories = new HashSet<DocumentsCategory>();
            DocumentsTags = new HashSet<DocumentsTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime DatePost { get; set; }
        public int PostBy { get; set; }
        public string Image { get; set; }
        public string FilePath { get; set; }
        public double? Rate { get; set; }
        public bool Status { get; set; }
        public double Price { get; set; }
        public int TagsId { get; set; }
        public int CategoriesId { get; set; }

        public virtual User PostByNavigation { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<DocumentsCategory> DocumentsCategories { get; set; }
        public virtual ICollection<DocumentsTag> DocumentsTags { get; set; }
    }
}
