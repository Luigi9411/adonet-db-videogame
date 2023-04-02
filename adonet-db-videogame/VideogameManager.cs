using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet_db_videogame
{
    public class VideogameManager
    {
        private string connectionString;
        
        public VideogameManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Videogame? GetById(long id)
        {
            using var conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                using var cmd = new SqlCommand(VideogameQueries.SelectById, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var name = reader.GetString(1);
                    var overview = reader.GetString(2);
                    var releaseDate = reader.GetDateTime(3);
                    var softwareHouseId = reader.GetInt64(4);

                    return new Videogame(id, name, overview, releaseDate, softwareHouseId);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Videogame> GetByName(string name)
        {
            using var conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                using var cmd = new SqlCommand(VideogameQueries.SelectByName, conn);
                cmd.Parameters.AddWithValue("@Name", "%" + name + "%");

                var reader = cmd.ExecuteReader();
                var videogames = new List<Videogame>();

                while (reader.Read())
                {
                    var id = reader.GetInt64(0);
                    var gameName = reader.GetString(1);
                    var overview = reader.GetString(2);
                    var releaseDate = reader.GetDateTime(3);
                    var softwareHouseId = reader.GetInt64(4);

                    var videogame = new Videogame(id, gameName, overview, releaseDate, softwareHouseId);
                    videogames.Add(videogame);
                }
               
                return videogames;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        public bool InsertVideogame(Videogame videogame)
        {
            using var conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                using var cmd = new SqlCommand(VideogameQueries.Insert, conn);

                cmd.Parameters.AddWithValue("@Name", videogame.Name);
                cmd.Parameters.AddWithValue("@Overview", videogame.Overview);
                cmd.Parameters.AddWithValue("@ReleaseDate", videogame.ReleaseDate);
                cmd.Parameters.AddWithValue("@SoftwareHouseId", videogame.SoftwareHouseId);

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DeleteVideogame(long id)
        {
            using var conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                using var cmd = new SqlCommand(VideogameQueries.DeleteById, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
   public static class VideogameQueries
    {
        public const string Insert = "INSERT INTO videogames (name, overview, release_date, software_house_id) VALUES (@Name, @Overview, @ReleaseDate, @SoftwareHouseId)";
        public const string SelectById = "SELECT id, name, overview, release_date, software_house_id FROM videogames WHERE Id = @Id";
        public const string SelectByName = "SELECT id, name, overview, release_date, software_house_id FROM videogames WHERE Name LIKE @Name";
        public const string DeleteById = "DELETE FROM videogames WHERE id = @Id";
    }
}

