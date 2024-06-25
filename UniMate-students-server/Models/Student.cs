namespace UniMate_students_server.Models
{
    public class Student
    {
        public int studentId { get; set; }

        public string username { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string email { get; set; }

        public string uniemail { get; set; }

        public string unistudentid { get; set; }

        public int dob { get; set; }

        public DateTime createdat { get; set; }
    }
}
