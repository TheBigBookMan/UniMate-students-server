using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniMate_students_server.Models
{
    public class Database
    {
        [Key]
        [Column("databaseid")]
        public int DatabaseId { get; set;  }

        public string DatabaseName { get; set; }

        public string? DatabaseHash { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CreatedAt { get; } = DateTime.UtcNow.ToString("o");

        public string db_id { get; set; }
    }
}
