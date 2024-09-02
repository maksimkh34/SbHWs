using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW10
{
    internal interface ICanAddClient
    {
        public void AddClient(string name, string surname, string patronymic, string phoneNumber, string passport);
    }
}
