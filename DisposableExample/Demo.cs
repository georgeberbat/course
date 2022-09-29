using System;
using System.Configuration;
using Npgsql;

namespace DisposableExample
{
    public class Demo
    {
        private int _connectionsCounter;

        public void StartOpenConnections()
        {
            for (var i = 0; i < 200; i++)
            {
                OpenConnection();
            }
        }

        public void RunConnectionAndMemoryDemo()
        {
            CreateConnectionsAndMemory(100);
            Console.WriteLine($"Total Allocated: {ConnectionAndMemory.TotalAllocated}");
            Console.WriteLine($"Total Freed: {ConnectionAndMemory.TotalFreed}");
        }

        public static NpgsqlConnection GetConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;
            var connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        public void CreateConnectionsAndMemory(int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var chunkSize = random.Next(4096);
                using (var connectionAndMemory = new ConnectionAndMemory(chunkSize))
                {
                    connectionAndMemory.DoWork();
                }
            }
        }

        private void OpenConnection()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                LogOpenedConnectionCount();
            } // здесь будет вызван метод Dispose
        }

        private void LogOpenedConnectionCount()
        {
            _connectionsCounter++;
            Console.WriteLine(_connectionsCounter);
        }
    }
}