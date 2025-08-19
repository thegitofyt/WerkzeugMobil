using Dotmim.Sync.Sqlite;
using Dotmim.Sync.SqlServer;
using Dotmim.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dotmim.Sync;
using Dotmim.Sync.SqlServer;
using Dotmim.Sync.Sqlite;
using System;
using System.Threading.Tasks;

namespace WerkzeugMobil
{
    public class SyncService
    {
        private string serverConnectionString = @"Data Source=YOUR_SQL_SERVER;Initial Catalog=YourDb;Integrated Security=true;";
        private string clientDbPath;

        private SqlSyncProvider serverProvider;
        private SqliteSyncProvider clientProvider;
        private SyncAgent agent;

        public SyncService()
        {
            // Path to SQLite client database
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbDirectory = System.IO.Path.Combine(localAppData, "WerkzeugMobil");
            clientDbPath = System.IO.Path.Combine(dbDirectory, "WerkzeugMobilDb.sqlite");

            serverProvider = new SqlSyncProvider(serverConnectionString);
            clientProvider = new SqliteSyncProvider(clientDbPath);

            agent = new SyncAgent(clientProvider, serverProvider);
        }

        public async Task SynchronizeAsync()
        {
            try
            {
                // Add "Benutzer" to the list of tables to sync
                var setup = new SyncSetup(new string[] { "Werkzeuge", "Projekte", "Tools", "Benutzer" });
                var result = await agent.SynchronizeAsync(setup);

                Console.WriteLine($"✅ Sync completed.");
                Console.WriteLine($"⬇️  Downloaded: {result.TotalChangesDownloadedFromServer}");
                Console.WriteLine($"⬆️  Uploaded: {result.TotalChangesUploadedToServer}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Sync failed: " + ex.Message);
            }
        }
    }
}
