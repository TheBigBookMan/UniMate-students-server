using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniMate_students_server.Models
{
    public class University
    {
        [Key]
        [Column("universityid")]
        public int UniversityId { get; set; }

        [Column("db_id")]
        public string db_id { get; set; }

        [Column("universityname")]
        public string UniversityName { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("contactemail")]
        public string ContactEmail  { get; set; }

        [Column("contactphone")]
        public string ContactPhone { get; set; }
    }
}
