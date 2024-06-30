using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniMate_students_server.Models
{
    public class Degree
    {
        [Key]
        public int DegreeId { get; set; }

        [Column("degreename")]
        public string DegreeName { get; set; }

        [Column("degreecode")]
        public string DegreeCode { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("createdat")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }
}
