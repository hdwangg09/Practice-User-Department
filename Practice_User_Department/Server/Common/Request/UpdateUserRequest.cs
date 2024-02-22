namespace Server.Common.Request
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public DateTime? Dob { get; set; }

        public bool? Gender { get; set; }

        public string? Address { get; set; }

        public int? DepartmentId { get; set; }
    }
}
