using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheHockeyGame.Data
{
    public class DataBase
    {
        private string _connectionString;

        public DataBase(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public void CreateGame(Gamer gamer)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO GameAdmin (pplamount, gametime) " +
                                  "VALUES (@pplamount, @gametime) SELECT SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("@pplamount", gamer.Pplamount);
                cmd.Parameters.AddWithValue("@gametime", gamer.GameTime);
                con.Open();
                gamer.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public IEnumerable<Gamer> GetAllGames()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"select g.*, count(p.GameId) as pplcount from GameAdmin g
                                    left join Players p on p.GameId = g.Id 
                                    group by g.Id, g.GameTime, g.PplAmount";
                List<Gamer> list = new List<Gamer>();
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Gamer gamer = new Gamer
                    {
                        Id = (int)reader["Id"],
                        Pplamount = (int)reader["pplamount"],
                        GameTime = (DateTime)reader["gametime"],
                        pplcount = (int)reader["pplcount"]
                    };
                    list.Add(gamer);
                }
                return list;
            }
        }

        public void AddPlayer(Players player)
        {
            Gamer gamer = new Gamer();
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "select top 1 * from GameAdmin " +
                                  "where GameTime > GETDATE() order by GameTime";
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                gamer.Id = (int)reader["id"];
                con.Close();
                con.Dispose();
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Players (firstname, lastname, email, gameid) " +
                                  "VALUES (@firstname, @lastname, @email, @gameid) SELECT SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("@firstname", player.FirstName);
                cmd.Parameters.AddWithValue("@lastname", player.LastName);
                cmd.Parameters.AddWithValue("@email", player.Email);
                cmd.Parameters.AddWithValue("@gameid", gamer.Id);
                con.Open();
                player.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public Gamer GetNextGame()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "select top 1 * from GameAdmin " +
                                  "where GameTime > GETDATE() order by GameTime";
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Gamer gamer = new Gamer
                    {
                        Id = (int)reader["id"],
                        Pplamount = (int)reader["pplamount"],
                        GameTime = (DateTime)reader["gametime"]
                    };
                    return gamer;
                }
                return null;
            }
        }

        public IEnumerable<Players> PlayersByGame(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "select * from Players where GameId = @id";
                cmd.Parameters.AddWithValue("@id", id);
                List<Players> list = new List<Players>();
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Players players = new Players
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Email = (string)reader["Email"],
                        GameId = (int)reader["gameid"]
                    };
                    list.Add(players);
                }
                return list;
            }
        }

        public int CountPplPerGame(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "select COUNT(*) from players where gameid = @id";
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}