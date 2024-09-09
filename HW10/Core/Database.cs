using System.Collections.ObjectModel;
using System.Text.Json;

namespace HW10
{
    public static class Database
    {
        public static Employee? ActiveEmployee;
        public static List<Client> Clients = [];
        public static readonly Stack<DataChangedArgs> Changes = new();

        public static bool ClientDatabasesEquals(IEnumerable<Client> list1, IEnumerable<Client> list2)
        {
            IEnumerable<Client> clients1 = list1.ToList();
            IEnumerable<Client> clients2 = list2.ToList();

            if (clients1.Count() != clients2.Count()) return false;
            for (var i = 0; i < clients1.Count(); i++)
            {
                if (!clients1.ElementAt(i).Equals(clients2.ElementAt(i)))
                    return false;
            }
            return true;
        }
        
        private const string ClientsDatabaseFilePath = "clients.json";

        static Database()
        {
            List<Client> clientsBase;
            try
            {
                clientsBase = LoadClients();
            }
            catch (FileNotFoundException)
            {
                File.Create(ClientsDatabaseFilePath).Close();
                File.WriteAllText(ClientsDatabaseFilePath, "[]");
                clientsBase = LoadClients();
            }
            Clients = clientsBase.Count == 0
                ?
                [
                    new Client("John", "Wu", "", "+375-29-777-55-44", "MP11112233", 2935692356),
                    new Client("Sergey", "Ku", "", "+375-29-111-00-33", "MP12341212",999912746),
                    new Client("Alex", "G", "", "+375-29-999-00-99", "MP04759376", 9385748399)
                ]
                : clientsBase;
        }

        public static void SaveDatabase()
        {
            SaveClients();
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
            if (ClientIdExists(id)) id = GetFreeId();
            return id;
        }

        public static void AddClient(Client client)
        {
            if (ClientIdExists(client.Id)) throw new Exception("ID already exists.");
            Clients.Add(client);
        } 

        public static bool ClientIdExists(long id) => Clients.Any(client => client.Id == id);

        private static void SaveClients()
        {
            var jsonString = JsonSerializer.Serialize(Clients, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ClientsDatabaseFilePath, jsonString);
        }

        private static List<Client> LoadClients()
        {
            return JsonSerializer.Deserialize<List<Client>>(File.ReadAllText(ClientsDatabaseFilePath)) ?? [];
        }
    }
}
