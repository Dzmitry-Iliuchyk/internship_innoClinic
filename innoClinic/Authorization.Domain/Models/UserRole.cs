﻿namespace Authorrization.Api.Models {
    public class UserRole {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
