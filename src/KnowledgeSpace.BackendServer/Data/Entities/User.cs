using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        public int? NumberOfKnowledgeBases { get; set; }
        public int? NumberOfVotes { get; set; }
        public int? NumberOfReports { get; set; }


    }
}
