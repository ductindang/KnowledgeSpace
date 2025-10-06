using KnowledgeSpace.BackendServer.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    public class Report : IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? KnowledgeBaseId { get; set; }
        public int? CommentId { get; set; }

        [MaxLength(500)]
        public string Content { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string ReportUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }



        // 🔗 Navigation properties
        public KnowledgeBase KnowledgeBase { get; set; }
        public Comment Comment { get; set; }
        public User ReportUser { get; set; }
    }
}
