namespace HW10
{
    internal class Consultant(long employeeId) : Employee(employeeId)
    {
        public new string? ClientPassport => base.ClientPassport is null ? null : "*********";
        public override EmployeeType GetEmployeeType() => EmployeeType.Consultant;

        public new string? ClientName => base.ClientName;
        public new string? ClientSurname => base.ClientSurname;
        public new string? ClientPatronymic => base.ClientPatronymic;

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
