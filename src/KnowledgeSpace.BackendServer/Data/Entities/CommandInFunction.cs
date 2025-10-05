using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    public class CommandInFunction
    {
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [Display(Order = 1)]
        public string CommandId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [Display(Order = 2)]
        public string FunctionId { get; set; }
    }
}
