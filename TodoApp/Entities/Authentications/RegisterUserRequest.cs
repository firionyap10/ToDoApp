﻿namespace TodoApp.API.Entities.Authentications
{
    public class RegisterUserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Password { get; set; }
    }
}
