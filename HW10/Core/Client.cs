﻿namespace HW10
{
    public class Client(string name, string surname, string patronymic, string phoneNumber, string passport, long id)
    {
        public string Name { get; set; } = name;
        public string Surname { get; set; } = surname;
        public string Patronymic { get; set; } = patronymic;
        public string PhoneNumber { get; set; } = phoneNumber;
        public string Passport { get; set; } = passport;
        public long Id { get; set; } = id;

        public Client(string name, string surname, string patronymic, string phoneNumber, string passport) : this(name,
            surname, patronymic, phoneNumber, passport, Database.GetFreeId()) { }

        public Client() : this("", "", "", "", "") { }
    }
}