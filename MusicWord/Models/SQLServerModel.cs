using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace MusicWord.Models
{
    class SQLServerModel
    {
        public static void connectSQLServer(string sqlCommand)
        {
            MySqlConnection connection = new MySqlConnection(Globals.connectionString);
            connection.Open();
            MySqlCommand insertCmd = new MySqlCommand(sqlCommand, connection);
            insertCmd.ExecuteNonQuery();
            connection.Close();
        }

        public static ICategory getWord(string category)
        {
            PlayerModel player = PlayerModel.Instance;
            MySqlConnection connection = null;

            connection = new MySqlConnection(Globals.connectionString);
            string sqlQuery = $"SELECT * FROM musicword.{category} ORDER BY RAND() LIMIT 1;";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            MySqlCommand queryCmd = new MySqlCommand(sqlQuery, connection);
            connection.Open();

            var reader = queryCmd.ExecuteReader();
            string answer = null;
            ICategory resCategory = null;
            if (reader.HasRows)
            {
                reader.Read();
                switch (player.Category)
                {
                    case "albums":
                        answer = reader.GetString(2);

                        resCategory = new AlbumModel(reader.GetInt64(0), answer, reader.GetInt16(1), reader.GetInt64(3));
                        break;
                    case "songs":
                        answer = reader.GetString(1);
                        resCategory = new SongModel(reader.GetInt64(0), answer, reader.GetInt64(2), reader.GetInt64(3));
                        break;
                    case "artists":
                        answer = reader.GetString(1);
                        resCategory = new ArtistModel(reader.GetInt64(0), answer, reader.GetString(4));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            if (!player.isInSet(answer))
            {
                return getWord(sqlQuery);
            }
            reader.Close();
            connection.Close();
            return resCategory;
        }

        public static string getClueString(string connectionString, string sqlQuery)
        {
            MySqlConnection connection = null;
            connection = new MySqlConnection(connectionString);
            
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            MySqlCommand queryCmd = new MySqlCommand(sqlQuery, connection);
            connection.Open();

            var reader = queryCmd.ExecuteReader();
            string answer = null;
            if (reader.HasRows)
            {
                reader.Read();
                if (sqlQuery.Contains("year"))
                {
                    answer = reader.GetInt16(0).ToString();
                }
                else
                {
                    answer = reader.GetString(0);
                }
            }
            if (answer == null)
            {
                return getClueString(connectionString, sqlQuery);
            }
            return answer;
        }
    }

}
