namespace HW10
{
    internal class Manager() : Employee(), ICanAddClient
    {
        public override EmployeeType GetEmployeeType() => EmployeeType.Manager;

        public void AddClient(string name, string surname, string patronymic, string phoneNumber, string passport)
        {
            Database.AddClient(new Client(name, surname, patronymic, phoneNumber, passport, Database.GetFreeId()));
        }
    }
}
