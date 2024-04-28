using Dapper;
using Microsoft.Data.Sqlite;

namespace HigherOrLowerBot
{
    //Structure of the database
    public class Model
    {
        public string? Name { get; set; }
        public int Num { get; set; }
    }

    public class SqliteDataAccess
    {
        static readonly string DBPath = "DB.db";
        //num most recently accessed through RetrieveNum
        public static int CurrentNum;
        public static IEnumerable<Model>? DB;

        public static async void LoadData(string name, int num)
        {
            await using var connection = new SqliteConnection($"Data Source={DBPath}");
            var sql = $"INSERT INTO Database values(\"{name}1\", {num})";
            try
            {
                var results = await connection.QueryAsync<Model>(sql);
            }
            catch (Exception)
            {
                Console.Write("Already in Database. ");
            }
        }

        public async static void ReadNum(String name)
        {
            await using var connection = new SqliteConnection($"Data Source={DBPath}");
            var sql = $"SELECT Num FROM Database WHERE Name = \"{name}\"";
            try
            {
                CurrentNum = (await connection.QueryAsync<Model>(sql)).ElementAt(0).Num;
            }
            catch (Exception)
            {
                Console.WriteLine("Name is not in table");
            }
            
        }

        public async static void ReadDB()
        {
            await using var connection = new SqliteConnection($"Data Source={DBPath}");
            var sql = $"SELECT * FROM Database";
            DB = (await connection.QueryAsync<Model>(sql));

        }

    }
}
