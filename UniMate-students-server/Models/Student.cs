using System;
using System.ComponentModel.DataAnnotations;

namespace UniMate_students_server.Models
{
    public class Student
    {
        [Key]
        public int studentid { get; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }   
        public string uniemail { get; set; }
        public string unistudentid { get; }
        public string email { get; set; }
        public DateOnly dob { get; set; }
        public DateTime createdat { get; set; }
    }
}
