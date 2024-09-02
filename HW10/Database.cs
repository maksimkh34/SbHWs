using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW10
{
    internal static class Database
    {
        public static Employee? ActiveEmployee;
        public static List<Client> Clients = new();

        public static void LoadDatabase()
        {
            // грузим клиентов из бд
            Clients = new List<Client>
            {
                new("John", "Wu", "", "+375-29-777-55-44", "MP11112233", 2935692356),
                new("Alex", "Ku", "", "+375-29-111-00-33", "MP12341212", 9385748399),
                new("Sergey", "G", "", "+375-29-999-00-99", "MP04759376", 999912746)
            };
        }

        public static Client GetClient(long id)
        {
            foreach (var client in Clients.Where(client => client.Id == id))
            {
                return client;
            }
            throw new KeyNotFoundException("No client with provided id found!");
        }

        public static long GetFreeId()
        {
            var id = new Random().NextInt64();
            if (IdExists(id)) id = GetFreeId();
            return id;
        }

        public static void AddClient(Client client)
        {
            if (IdExists(client.Id)) throw new Exception("ID already exists.");
            Clients.Add(client);
        } 

        public static bool IdExists(long id) => Clients.Any(client => client.Id == id);

        public static Stack<DataChangedArgs> Changes = new();
    }
}
