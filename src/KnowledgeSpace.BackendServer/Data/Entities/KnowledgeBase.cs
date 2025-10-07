using KnowledgeSpace.BackendServer.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    public class KnowledgeBase : IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Range(1, Double.PositiveInfinity)]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        [Column(TypeName = "varchar(500)")]
        public string SeoAllias { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Environment { get; set; }

        [MaxLength(500)]
        public string Problem { get; set; }
        public string StepToReproduce { get; set; }

        [MaxLength(500)]
        public string ErrorMessage { get; set; }

        [MaxLength(500)]
        public string WorkAround { get; set; }

        public string Note { get; set; }

        [Required]
        public string OwnerUserId { get; set; }

        public string Labels { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? NumberOfComments { get; set; }
        public int? NumberOfVotes { get; set; }
        public int? NumberOfReports { get; set; }



        // 🔗 Navigation
        public Category Category { get; set; }
        public User OwnerUser { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<LabelInKnowledgeBase> LabelInKnowledgeBases { get; set; } // Many-to-many
        public ICollection<Report> Reports { get; set; }

    }
}
