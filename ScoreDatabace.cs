using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;

namespace SlimeFight.DataBase
{
    public class ScoreDatabase
    {
        private SQLiteConnection db;

        public ScoreDatabase()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string dbPath = Path.Combine(folderPath, "score.db3");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            db = new SQLiteConnection(dbPath);
            db.CreateTable<PlayerScore>();
        }

        public void SaveScore(int score)
        {
            var scoreEntry = new PlayerScore { Id = 1, Score = score };
            db.InsertOrReplace(scoreEntry);
        }

        public int LoadScore()
        {
            var scoreEntry = db.Find<PlayerScore>(1);
            return scoreEntry?.Score ?? 0;
        }
    }

    public class PlayerScore
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int Score { get; set; }
    }
}
