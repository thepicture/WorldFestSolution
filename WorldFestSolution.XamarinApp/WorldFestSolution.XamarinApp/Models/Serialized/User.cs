﻿namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int UserTypeId { get; set; }
        public byte[] Image { get; set; }
        public double Rating { get; set; }
    }
}
