using System;
using System.Collections.Generic;

#nullable disable

namespace DAX_Block2_2024.Entities
{
    public partial class User
    {
        public User()
        {
            CommentNews = new HashSet<CommentNews>();
            Comments = new HashSet<Comment>();
            Documents = new HashSet<Document>();
            News = new HashSet<News>();
            Reports = new HashSet<Report>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDay { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? AccountRate { get; set; }
        public int? AccountPoint { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<CommentNews> CommentNews { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
