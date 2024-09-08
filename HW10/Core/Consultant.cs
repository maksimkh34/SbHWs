namespace HW10
{
    public class Consultant : Employee
    {
        public new string? ClientPassport
        {
            get => base.ClientPassport is null ? null : "*********";
            set => _ = value;
        }

    public override EmployeeType GetEmployeeType() => EmployeeType.Consultant;

        public new string? ClientName
        {
            get => base.ClientName;
            // set { }
            set => _ = value;
        }
        public new string? ClientSurname
        {
            get => base.ClientSurname;
            set => _ = value;
        }
        public new string? ClientPatronymic
        {
            get => base.ClientPatronymic;
            set => _ = value;
        }
        // Возможно, консультанту не нужно иметь права на изменение ID клиентов. Тогда достаточно раскомментировать эту строку
        // public new long? ClientId => base.ClientId;

        public new string? ClientPhoneNumber
        {
            get => base.ClientPhoneNumber;
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                base.ClientPhoneNumber = value;
            }
        }
    }
}
