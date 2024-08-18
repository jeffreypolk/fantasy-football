using FF.Backend.Repositories.Framework;
using FF.Backend.Results;
using io = System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FF.Backend.Services
{
    public class StatsService : IStatsService
    {
        private readonly ISettings _settings;
        
        public StatsService(
            ISettings settings
        ) 
        {
            _settings = settings;
        }

        public Result<DTO.Stats.StatInfo> GetStats(int leagueId, int startYear, int endYear, bool includeActive)
        {
            var stats = new DTO.Stats.StatInfo();
            var sql = new DataLayer.StoredProcedureExecutor(_settings.ConnectionString);
            sql.AddParam("@LeagueId", leagueId);
            sql.AddParam("@StartYear", startYear);
            sql.AddParam("@EndYear", endYear);
            sql.AddParam("@Managers", includeActive ? "Active" : "");
            var ds = sql.ExecuteDataSet("uspManagerStats");

            // rings
            foreach(System.Data.DataRow row in ds.Tables[0].Rows)
            {
                stats.Rings.Add(new DTO.Stats.Ring()
                {
                    Name = row["Name"].ToString(),
                    Rings = int.Parse(row["Rings"].ToString()),
                    Years = int.Parse(row["Years"].ToString()),
                    RingPercentage = decimal.Parse(row["RingPercentage"].ToString()),
                });
            }

            // record
            foreach (System.Data.DataRow row in ds.Tables[1].Rows)
            {
                stats.Records.Add(new DTO.Stats.Record()
                {
                    Name = row["Name"].ToString(),
                    Wins = int.Parse(row["Wins"].ToString()),
                    Ties = int.Parse(row["Ties"].ToString()),
                    Losses = int.Parse(row["Losses"].ToString()),
                    Finish = int.Parse(row["Finish"].ToString()),
                    WinPercentage = decimal.Parse(row["WinPercentage"].ToString()),
                    Years = int.Parse(row["Years"].ToString())
                });
            }

            // points for
            foreach (System.Data.DataRow row in ds.Tables[2].Rows)
            {
                stats.PointsFor.Add(new DTO.Stats.PointsFor()
                {
                    Name = row["Name"].ToString(),
                    Points = decimal.Parse(row["PointsFor"].ToString()),
                    Years = int.Parse(row["Years"].ToString())
                });
            }

            // points against
            foreach (System.Data.DataRow row in ds.Tables[3].Rows)
            {
                stats.PointsAgainst.Add(new DTO.Stats.PointsAgainst()
                {
                    Name = row["Name"].ToString(),
                    Points = decimal.Parse(row["PointsAgainst"].ToString()),
                    Years = int.Parse(row["Years"].ToString())
                });
            }

            // moves
            foreach (System.Data.DataRow row in ds.Tables[4].Rows)
            {
                stats.Moves.Add(new DTO.Stats.Move()
                {
                    Name = row["Name"].ToString(),
                    Moves = int.Parse(row["Moves"].ToString())
                });
            }

            // playoffs
            foreach (System.Data.DataRow row in ds.Tables[5].Rows)
            {
                stats.Playoffs.Add(new DTO.Stats.Playoff()
                {
                    Name = row["Name"].ToString(),
                    Playoffs = int.Parse(row["Playoffs"].ToString()),
                    PlayoffPercentage = decimal.Parse(row["PlayoffPercentage"].ToString()),
                    Years = int.Parse(row["Years"].ToString())
                });
            }

            // playoff misses
            foreach (System.Data.DataRow row in ds.Tables[6].Rows)
            {
                stats.PlayoffMisses.Add(new DTO.Stats.PlayoffMiss()
                {
                    Name = row["Name"].ToString(),
                    PlayoffMisses = int.Parse(row["PlayoffMisses"].ToString()),
                    PlayoffMissPercentage = decimal.Parse(row["PlayoffMissPercentage"].ToString()),
                    Years = int.Parse(row["Years"].ToString())
                });
            }

            // money won
            foreach (System.Data.DataRow row in ds.Tables[7].Rows)
            {
                stats.Money.Add(new DTO.Stats.Money()
                {
                    Name = row["Name"].ToString(),
                    MoneyWon = decimal.Parse(row["MoneyWon"].ToString()),
                    BuyIn = decimal.Parse(row["BuyIn"].ToString()),
                    Years = int.Parse(row["Years"].ToString())
                });
            }

            // BFLs
            foreach (System.Data.DataRow row in ds.Tables[8].Rows)
            {
                stats.BFLs.Add(new DTO.Stats.BFL()
                {
                    Name = row["Name"].ToString(),
                    BFLs = int.Parse(row["BFLs"].ToString()),
                    BFLPercentage = decimal.Parse(row["BFLPercentage"].ToString()),
                    Years = int.Parse(row["Years"].ToString())
                });
            }

            // Double digit wins
            foreach (System.Data.DataRow row in ds.Tables[9].Rows)
            {
                stats.DoubleDigitWins.Add(new DTO.Stats.DoubleDigitWin()
                {
                    Name = row["Name"].ToString(),
                    Year = int.Parse(row["Year"].ToString()),
                    Wins = int.Parse(row["Wins"].ToString())
                });
            }

            // Double digit losses
            foreach (System.Data.DataRow row in ds.Tables[10].Rows)
            {
                stats.DoubleDigitLosses.Add(new DTO.Stats.DoubleDigitLoss()
                {
                    Name = row["Name"].ToString(),
                    Year = int.Parse(row["Year"].ToString()),
                    Losses = int.Parse(row["Losses"].ToString())
                });
            }

            return new Result<DTO.Stats.StatInfo>(stats);
        }

    }
    public interface IStatsService
    {
        Result<DTO.Stats.StatInfo> GetStats(int leagueId, int startYear, int endYear, bool includeActive);
    }
}
