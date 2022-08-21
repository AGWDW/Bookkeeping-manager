using System.Collections.Generic;

namespace Bookkeeping_manager.src.Clients
{
    internal static class ClientManager
    {
        private static int uid_counter = 1;
        public static List<Client> AllClients { get; set; } = new List<Client>();
        /// <summary>
        /// Gets the clinet null if not found
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static Client GetClient(int uid)
        {
            foreach (Client client in AllClients)
            {
                if (client.UID == uid)
                {
                    return client;
                }
            }
            return null;
        }
        public static bool Rename(int uid, string name)
        {
            Client c = GetClient(uid);
            if (c is null)
            {
                return false;
            }
            c.Name = name;
            return true;
        }
        public static bool Delete(int uid)
        {
            return AllClients.RemoveAll((c) => c.UID == uid) == 1;
        }

        public static void AddClient(Client client, out int uid)
        {
            client.UID = uid_counter;
            uid = uid_counter++;
            AllClients.Add(client);
        }
    }
}
