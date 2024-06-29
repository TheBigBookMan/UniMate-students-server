namespace UniMate_students_server.DTOs.AuthDTO
{
    public class LoginResponse : BaseResponse
    {
        public string UniEmail { get; set; }
        public string UniStudentId { get; set; }
    }
}
