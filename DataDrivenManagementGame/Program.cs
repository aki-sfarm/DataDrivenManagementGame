using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace CreateSalesData

{
    class Program
    {

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            //var connectionString = configuration.GetConnectionString("DefaultConnection");

            string dbServerIP = configuration.GetConnectionString("DBServerIP");
            string databaseName = configuration.GetConnectionString("DatabaseName");
            string userId = configuration.GetConnectionString("Id");

            string TrustServerCertificate = "True";


            var kvUri = configuration.GetConnectionString("KeyVaultURL");
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret("devSQLDBpassword");

            string password = secret.Value;

            string connectionString = "Server=" + dbServerIP + ";Database=" + databaseName + ";User Id=" + userId + ";Password=" + password + ";TrustServerCertificate=" + TrustServerCertificate + ";";


            var dbConnection = new SqlServerHelper(connectionString);


            dbConnection.CreateConnection();

            // Execute the query
            DataTable customerTable = dbConnection.ExecuteQuery("SELECT * FROM CustomerList");

            // Print out the customer IDs
            foreach (DataRow row in customerTable.Rows)
            {
                Console.WriteLine(row["CustomerID"]);
            }

            dbConnection.DisposeConnection();


        }
    }
}
