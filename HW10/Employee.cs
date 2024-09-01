namespace HW10
{
    internal abstract class Employee(long employeeId)
    {
        private Client? _client;
        private long _employeeId = employeeId;

        public void SelectClient(long id)
        {
            _client = Database.GetClient(id);
        }

        public string? ClientName
        {
            get => _client?.Name;
            set
            {
                if (_client != null) _client.Name = value!;
            }
        }

        public string? ClientSurname
        {
            get => _client?.Surname;
            set
            {
                if (_client != null) _client.Surname = value!;
            }
        }

        public string? ClientPatronymic
        {
            get => _client?.Patronymic;
            set
            {
                if (_client != null) _client.Patronymic = value!;
            }
        }

        public string? ClientPhoneNumber
        {
            get => _client?.PhoneNumber;
            set
            {
                if (_client != null) _client.PhoneNumber = value!;
            }
        }

        public string? ClientPassport
        {
            get => _client?.Passport;
            set
            {
                if (_client != null) _client.Passport = value!;
            }
        }

        public long? ClientId
        {
            get => _client?.Id;
            set
            {
                if (_client != null) _client.Id = (long)value!;
            }
        }

    }
}
