using System;
using System.Runtime.InteropServices;
using Npgsql;

namespace DisposableExample
{
    public class ConnectionAndMemory : IDisposable
    {
        public static long TotalFreed { get; private set; }
        public static long TotalAllocated { get; private set; }

        private NpgsqlConnection _connection;
        private IntPtr _chunkHandle;
        private int _chunkSize;
        private bool _isFreed;

        public ConnectionAndMemory(int chunkSize)
        {
            _connection = Demo.GetConnection();
            _connection.Open();

            _chunkSize = chunkSize;
            _chunkHandle = Marshal.AllocHGlobal(chunkSize);
            TotalAllocated += chunkSize;
        }

        private void ReleaseUnmanagedResources()
        {
            if (_isFreed) return;
            
            Marshal.FreeHGlobal(_chunkHandle);
            TotalFreed += _chunkSize;
            _isFreed = true;
        }

        public void DoWork() { } // Фиктивный метод. Подразумевается, что здесь вы работаете с ресурсами.

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _connection.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}