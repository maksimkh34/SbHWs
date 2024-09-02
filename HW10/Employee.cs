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

        public abstract EmployeeType GetEmployeeType();

        public string? ClientName
        {
            get => _client?.Name;
            set
            {
                if (_client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientName),
                    ClientName!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                _client.Name = value!;
            }
        }

        public string? ClientSurname
        {
            get => _client?.Surname;
            set
            {
                if (_client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientSurname),
                    ClientSurname!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                _client.Surname = value!;
            }
        }

        public string? ClientPatronymic
        {
            get => _client?.Patronymic;
            set
            {
                if (_client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientPatronymic),
                    ClientPatronymic!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                _client.Patronymic = value!;
            }
        }

        public string? ClientPhoneNumber
        {
            get => _client?.PhoneNumber;
            set
            {
                if (_client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientPhoneNumber),
                    ClientPhoneNumber!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                _client.PhoneNumber = value!;
            }
        }

        public string? ClientPassport
        {
            get => _client?.Passport;
            set
            {
                if (_client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientPassport),
                    ClientPassport!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                _client.Passport = value!;
            }
        }

        public long? ClientId
        {
            get => _client?.Id;
            set
            {
                if (_client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientId),
                    ClientPhoneNumber!, value?.ToString()!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                _client.Id = (long)value!;
            }
        }

    }
}
