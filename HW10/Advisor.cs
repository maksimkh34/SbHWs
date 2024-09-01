using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW10
{
    internal class Advisor(long employeeId) : Employee(employeeId)
    {
        public new string? ClientPassport => base.ClientPassport is null ? null : "*********";
        public new string? ClientName => base.ClientName;
        public new string? ClientSurname => base.ClientSurname;
        public new string? ClientPatronymic => base.ClientPatronymic;

        public new string? ClientPhoneNumber
        {
            get => base.ClientPhoneNumber;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    base.ClientPhoneNumber = value;
                }
            }
        }
    }
}
