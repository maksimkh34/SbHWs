namespace HW10
{
    internal class Consultant() : Employee()
    {
        public new string? ClientPassport => base.ClientPassport is null ? null : "*********";

        public override EmployeeType GetEmployeeType() => EmployeeType.Consultant;

        public new string? ClientName => base.ClientName;
        public new string? ClientSurname => base.ClientSurname;
        public new string? ClientPatronymic => base.ClientPatronymic;
        // Возможно, консультанту не нужно иметь права на изменение ID клиентов. Тогда достаточно раскомментировать эту строку
        // public new long? ClientId => base.ClientId;

        public new string? ClientPhoneNumber
        {
            get => base.ClientPhoneNumber;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                Database.Changes.Push(new DataChangedArgs(nameof(ClientPhoneNumber),
                    ClientPhoneNumber!, value!,
                    (EmployeeType)Database.ActiveEmployee?.GetEmployeeType()!));
                base.ClientPhoneNumber = value;
            }
        }
    }
}
