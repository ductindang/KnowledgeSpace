using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    public class LabelInKnowledgeBase
    {
        public int KnowledgeBaseId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string LabelId { get; set; }

        // 🔗 Navigation properties (quan hệ nhiều-nhiều)
        public KnowledgeBase KnowledgeBase { get; set; }
        public Label Label { get; set; }
    }
}
