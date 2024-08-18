using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Sleeper
{
    public class Players
    {
        public void GetPlayers()
        {
            var rest = new RestSharp.RestClient("https://api.sleeper.app/v1");
            var request = new RestSharp.RestRequest("players/nfl", RestSharp.Method.Get);
            var response = rest.Execute(request);
            System.IO.File.WriteAllText(@"C:\my\FantasyFootball\players.json", response.Content);

            var json = System.IO.File.ReadAllText(@"C:\my\FantasyFootball\players.json");
            var players = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Player>>(json);
            var output = new System.Text.StringBuilder();
            foreach (var player in players)
            {
                if (!string.IsNullOrEmpty(player.Value.team))
                {
                    if (player.Value.position == "QB" || player.Value.position == "WR" || player.Value.position == "RB" || player.Value.position == "TE")
                    {
                        output.AppendLine("insert into Player (Name, NFLTeam, Position, Age, Experience, DepthChart, Year)");
                        output.Append($"values (");
                        output.Append($"'{player.Value.first_name.Replace("'", "''")} {player.Value.last_name.Replace("'", "''")}', ");
                        output.Append($"'{player.Value.team}', ");
                        output.Append($"'{player.Value.position}', ");
                        output.Append(player.Value.age.HasValue ? $"{player.Value.age}, " : "null, ");
                        output.Append(player.Value.years_exp.HasValue ? $"{player.Value.years_exp}, " : "null, ");
                        output.Append(player.Value.depth_chart_order.HasValue ? $"{player.Value.depth_chart_order}, " : "null, ");
                        output.Append($"2022)");
                        output.AppendLine();
                    }
                }
            }

            System.IO.File.WriteAllText(@"C:\my\FantasyFootball\players.sql", output.ToString());
        }
    }
}
