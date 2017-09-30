using System;
using NUnit.Framework;
using Shared.Database;
using System.IO;
using MySql.Data.MySqlClient;
using Shared.Models;
using Shared.Objects;

namespace SharedTests
{
    [TestFixture]
    public class DatabaseTests
    {
        public static BaseDatabase DbConnection;

        private static string DbHost = "127.0.0.1";
        private static int DbPort = 3306;
        private static string DbUsername = "root";
        private static string DbPassword = "usbw";
        private static string DbName = "dcmm_test";
        
        [OneTimeSetUp]
        public static void Setup()
        {
            var connStr = $"server={DbHost};user={DbUsername};port={DbPort};password={DbPassword};pooling=true; min pool size=0; max pool size=100; ConvertZeroDateTime=true";
            using (var conn = new MySqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{DbName}`;";
                Assert.AreEqual(1, cmd.ExecuteNonQuery());
            }
            
            DbConnection = new BaseDatabase();
            DbConnection.Init(DbHost, DbPort, DbUsername, DbPassword, DbName);
            // Verify that mysql connection was set-up correctly.
            Assert.IsNotNull(DbConnection.Connection);
            
            var filePath = Utilities.GetTestFile("/../../sql/dcmm.sql");
            // Verify that db sql file exists
            FileAssert.Exists(filePath);
            var script = new MySqlScript(DbConnection.Connection, File.ReadAllText(filePath));
            Assert.AreNotEqual(0, script.Execute());
        }

        [OneTimeTearDown]
        public static void Teardown()
        {
            DbConnection.Connection.Close();
            DbConnection = null;
            
            var connStr = $"server={DbHost};user={DbUsername};port={DbPort};password={DbPassword};pooling=true; min pool size=0; max pool size=100; ConvertZeroDateTime=true";
            using (var conn = new MySqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = $"DROP DATABASE IF EXISTS `{DbName}`;";
                //Assert.AreEqual(1, cmd.ExecuteNonQuery());
                cmd.ExecuteNonQuery();
            }
        }

        [Test]
        public static void Test_RetrieveChar()
        {
            var uid = AccountModel.CreateAccount(DbConnection.Connection, "127.0.0.1", "admin", "admin");
            var character = new Character
            {
                Uid = (ulong)uid,
                Name = "GigaToni",
                Avatar = 1,
            };

            CharacterModel.CreateCharacter(DbConnection.Connection, ref character);

            character = null;
            character = CharacterModel.Retrieve(DbConnection.Connection, "GigaToni");
            Assert.IsNotNull(character);
            Console.WriteLine(character.Name);
        }
    }
}