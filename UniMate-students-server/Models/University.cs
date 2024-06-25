using System;
using System.ComponentModel.DataAnnotations;

namespace UniMate_students_server.Models
{
    public class University
    {
        [Key]
        public int UniversityId { get; }

        public string UniversityName { get; set; }

        public string DatabaseHash { get; }

        public DateTime CreatedAt { get; }

        public string db_id { get; }
    }
}
