using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace UniMate_students_server.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("uniemail")]
        public string UniEmail { get; set; }

        [Column("unistudentid")]
        public string UniStudentId { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("dob")]
        public DateTime Dob { get; set; }

        [Column("createdat")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

    }
}
