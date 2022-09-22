using Bookkeeping_manager.src.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bookkeeping_manager.src.Clients
{
    public static class ClientManager
    {
        internal static int uid_counter { get; private set; } = 1;
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
        internal static void SetUID_Counter(int counter)
        {
            uid_counter = counter;
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
        /// <summary>
        /// Removes the client with the specified UID and deleted all taskes with there name in the name
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool Delete(int uid)
        {
            DatabaseConnection.DeleteClient(uid);
            return removeClient(uid);
        }
        public static bool Archive(int uid)
        {
            DatabaseConnection.ArchiveClient(uid);
            return removeClient(uid);
        }

        private static bool removeClient(int uid)
        {
            Client client = GetClient(uid);
            if (client is null)
            {
                return false;
            }
            TaskManager.DeleteTasksWhere((t) => t.Name.Contains(client.Name));
            return AllClients.RemoveAll((c) => c.UID == uid) == 1;
        }

        public static void AddClient(Client client, out int uid)
        {
            client.UID = uid_counter;
            uid = uid_counter++;
            AllClients.Add(client);
            DatabaseConnection.AddClient(uid);
        }

        internal static void SetClients(List<Client> clients)
        {
            AllClients = clients;
        }
    }
}
