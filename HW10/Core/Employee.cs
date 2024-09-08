namespace HW10
{
    public abstract class Employee()
    {
        private protected Client? Client;

        public void SelectClient(long id)
        {
            Client = Database.GetClient(id);
        }

        public void SelectClient(Client client)
        {
            Client = client;
        }

        public abstract EmployeeType GetEmployeeType();

        public string? ClientName
        {
            get => Client?.Name;
            set
            {
                if (Client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientName),
                    ClientName!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                Client.Name = value!;
            }
        }

        public string? ClientSurname
        {
            get => Client?.Surname;
            set
            {
                if (Client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientSurname),
                    ClientSurname!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                Client.Surname = value!;
            }
        }

        public string? ClientPatronymic
        {
            get => Client?.Patronymic;
            set
            {
                if (Client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientPatronymic),
                    ClientPatronymic!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                Client.Patronymic = value!;
            }
        }

        public string? ClientPhoneNumber
        {
            get => Client?.PhoneNumber;
            set
            {
                if (Client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientPhoneNumber),
                    ClientPhoneNumber!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                Client.PhoneNumber = value!;
            }
        }

        public string? ClientPassport
        {
            get => Client?.Passport;
            set
            {
                if (Client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientPassport),
                    ClientPassport!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                Client.Passport = value!;
            }
        }

        public long? ClientId
        {
            get => Client?.Id;
            set
            {
                if (Client == null) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientId),
                    ClientPhoneNumber!, value?.ToString()!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                Client.Id = (long)value!;
            }
        }
    }
}
