using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniMate_students_server.Models
{
    public class Auth
    {
        [Key]
        [Column("authid")]
        public int AuthId { get; set; }

        [Column("studentid")]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [Column("passwordhash")]
        public string PasswordHash { get; set; }

        [Column("passwordsalt")]
        public string PasswordSalt { get; set; }

        [Column("createdat")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }
}
