using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW10
{
    internal static class Database
    {
        public static Client GetClient(long id)
        {
            return new Client("John", "Ku", "Igorevich", "375297774433", "MP4555090", 1038569);
        }
    }
}
