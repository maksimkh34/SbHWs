namespace HW10
{
    public class DataChangedArgs(string changedProperty, string oldValue, string newValue, EmployeeType changedEmployeeType)
    {
        public DateTime Date = DateTime.Now;
        public string ChangedProperty = changedProperty;
        public string OldValue = oldValue;
        public string NewValue = newValue;
        public EmployeeType ChangedEmployeeType = changedEmployeeType;
    }

    public enum EmployeeType
    {
        Employee,
        Consultant,
        Manager
    }
}
