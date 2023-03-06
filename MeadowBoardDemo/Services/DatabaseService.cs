using System;
using System.IO;
using System.Threading.Tasks;
using Meadow;
using MeadowBoardDemo.Entitties;
using SQLite;

namespace MeadowBoardDemo.Services
{
    public class DatabaseService : IDisposable
    {
        private readonly AppSecrets _appSecrets;
        public SQLiteConnection Db { get; set; } = null!;

        public DatabaseService(AppSecrets appSecrets)
        {
            _appSecrets = appSecrets;
        }

        public void Initialze()
        {
            var databasePath = Path.Combine(MeadowOS.FileSystem.DataDirectory, $"{_appSecrets.DbName}.db");

            Db = new SQLiteConnection(databasePath);

            Db.CreateTable<MailboxLog>();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
