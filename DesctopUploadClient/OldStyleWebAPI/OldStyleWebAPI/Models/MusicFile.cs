using Npgsql;
using System;

namespace OldStyleWebAPI.Models
{
    public class MusicFile
    {
        public Guid id { get; set; }
        public string fileName { get; set; }
        public string path { get; set; }
        public Guid userId { get; set; }

        internal void Save()
        {
            var connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=1;Database=musicplayer;";
            var sqlCommand = "INSERT INTO public.track(id, userid, name, path) VALUES('"+ id + "', '" + userId + "', '" + fileName + "', '" + path + "'); ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(sqlCommand, npgSqlConnection);
            var result = npgSqlCommand.ExecuteNonQuery();
        }
    }
}