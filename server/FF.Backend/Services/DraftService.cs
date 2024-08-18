using FF.Backend.Repositories;
using FF.Backend.Repositories.Framework;
using FF.Backend.Results;
using FF.Backend.Services.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FF.Backend.Services
{
    public class DraftService : IDraftService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerTeamRepository _playerTeamRepository;
        private readonly ITeamService _teamService;
        private readonly ISettings _settings;

        public DraftService(
            ISettings settings,
            ITeamRepository TeamRepository, 
            IPlayerRepository playerRepository, 
            IPlayerTeamRepository playerTeamRepository,
            ITeamService teamService
        ) 
        {
            _settings = settings;
            _teamRepository = TeamRepository;
            _playerRepository = playerRepository;
            _playerTeamRepository = playerTeamRepository;
            _teamService = teamService;
        }

        public Result<IEnumerable<Domain.Team>> GetTeams(int leagueId, int year)
        {
            var ret = new Result<IEnumerable<Domain.Team>>()
            {
                Data = _teamRepository.Get()
                .Include(t => t.Manager)
                .Include(t => t.Players).ThenInclude(p => p.Player)
                .Where(t => t.LeagueId == leagueId && t.Year == year)
                .OrderBy(t => t.DraftOrder)
                .ToList()
            };

            return ret;
        }

        public Result<List<DTO.Draft.PickPosition>> GetPickPositions(int leagueId, int year)
        {
            var ret = new Result<List<DTO.Draft.PickPosition>>(new List<DTO.Draft.PickPosition>());

            var sql = new DataLayer.StoredProcedureExecutor(_settings.ConnectionString);
            sql.AddParam("@LeagueId", leagueId);
            sql.AddParam("@Year", year);
            var dt = sql.ExecuteDataTable("uspDraftPickPositionsGet");
            foreach(System.Data.DataRow row in dt.Rows)
            {
                ret.Data.Add(new DTO.Draft.PickPosition()
                {
                    Overall = int.Parse(row["Pick"].ToString()),
                    Round = int.Parse(row["Round"].ToString()),
                    TeamPosition = int.Parse(row["Team"].ToString())
                });
            }
            return ret;
        }

        public Result<IEnumerable<Domain.Player>> GetPlayers(int leagueId, int year)
        {
            var data = _playerRepository.Get()
                .Include(p => p.Teams).ThenInclude(t => t.Team)
                .Where(p => p.Year == year)
                .ToList();

            var dataToReturn = new List<Domain.Player>();

            foreach (var player in data)
            {
                if (player.Teams.Where(t => t.Team.LeagueId == leagueId).Any())
                {
                    // this player already selected for this league
                    // dont add it to output
                }
                else
                {
                    // still available
                    player.Teams.Clear();
                    dataToReturn.Add(player);
                }
            }
            
            var ret = new Result<IEnumerable<Domain.Player>>()
            {
                Data = dataToReturn
            };

            
            return ret;
        }

        public Result MakePick(DTO.Draft.Pick info)
        {
            var ret = new Result();

            var entity = new Domain.PlayerTeam()
            {
                IsKeeper = info.IsKeeper,
                Overall = info.Overall,
                PlayerId = info.PlayerId,
                Round = info.Round,
                TeamId = info.TeamId
            };
            _playerTeamRepository.Insert(entity);

            return ret;
        }

        public Result<DTO.Draft.Board> GetBoard(int leagueId, int year)
        {
            var ret = new Result<DTO.Draft.Board>();

            var teams = _teamService.GetByLeagueYear(leagueId, year).Data.OrderBy(t => t.DraftOrder).ToList();
            var teamCount = teams.Count;
            var roundCount = 0;
            var finalTeams = new List<Domain.Team>();
            var finalPicks = new List<Domain.PlayerTeam>();
            var finalLayout = new List<List<int>>();
            var bestPickInfo = "";
            var worstPickInfo = "";
            foreach (var team in teams)
            {
                var teamInfo = _teamService.GetById(team.Id).Data;
                if (roundCount == 0)
                {
                    roundCount = teamInfo.Players.Count;
                }
                finalPicks.AddRange(teamInfo.Players);
                teamInfo.Players.Clear();
                finalTeams.Add(teamInfo);
            }
            if (finalTeams.Count > 0)
            {
                var sortedPicks = finalPicks;
                if (roundCount > 0)
                {
                    sortedPicks = SortDraftPicks(finalPicks, teamCount, roundCount);
                    finalLayout = BuildDraftLayout(teamCount, roundCount);

                    var bestPick = sortedPicks
                        .Where(p => !p.IsKeeper && p.Player.ADP > 0 && p.Player.ADP < 999)
                        .OrderByDescending(p => p.Overall - p.Player.ADP)
                        .FirstOrDefault();
                    if (bestPick != null)
                    {
                        bestPickInfo = $"{bestPick.Player.Name} ({bestPick.Round}-{bestPick.Overall})";
                    }

                    var worstPick = sortedPicks
                        .Where(p => !p.IsKeeper && p.Player.ADP > 0 && p.Player.ADP < 999)
                        .OrderBy(p => p.Overall - p.Player.ADP)
                        .FirstOrDefault();
                    if (worstPick != null)
                    {
                        worstPickInfo = $"{worstPick.Player.Name} ({worstPick.Round}-{worstPick.Overall})";
                    }
                }
                var finalInfo = new DTO.Draft.Board()
                {
                    Rounds = roundCount,
                    Teams = finalTeams,
                    Picks = sortedPicks,
                    Layout = finalLayout,
                    BestPick = bestPickInfo,
                    WorstPick = worstPickInfo
                };
                //finalInfo.Teams.AddRange(finalTeams);
                //finalInfo.Picks.AddRange(sortedPicks);
                ret.Data = finalInfo;
                
            }

            if (ret.Data != null)
            {
                ret.Data.Analysis = GetAnalysis(leagueId, year);
            }
            
            return ret;
        }

        private DTO.Draft.Analysis GetAnalysis(int leagueId, int year)
        {
            var ret = new DTO.Draft.Analysis();

            var sql = new DataLayer.StoredProcedureExecutor(_settings.ConnectionString);
            sql.AddParam("@LeagueId", leagueId);
            sql.AddParam("@Year", year);
            var ds = sql.ExecuteDataSet("uspDraftScorecard");

            foreach (System.Data.DataRow row in ds.Tables[1].Rows)
            {
                var teamId = int.Parse(row["TeamId"].ToString());

                ret.Teams.Add(new DTO.Draft.AnalysisTeam()
                {
                    Name = row["Name"].ToString(),
                    ActualFinish = int.Parse(row["ActualFinish"].ToString()),
                    ActualLineup = GetAnalysisPlayers(ds.Tables[0], teamId, "A"),
                    ActualPoints = decimal.Parse(row["Actual"].ToString()),
                    Finish = int.Parse(row["Finish"].ToString()),
                    ProjectedFinish = int.Parse(row["ProjectedFinish"].ToString()),
                    ProjectedLineup = GetAnalysisPlayers(ds.Tables[0], teamId, "P"),
                    ProjectedPoints = decimal.Parse(row["Projected"].ToString())
                });
            }
            return ret;
        }

        private List<DTO.Draft.AnalysisPlayer> GetAnalysisPlayers(System.Data.DataTable dt, int teamId, string pointType)
        {
            var ret = new List<DTO.Draft.AnalysisPlayer>();

            foreach (System.Data.DataRow row in dt.Rows)
            {
                if (int.Parse(row["TeamId"].ToString()) == teamId && row["PointType"].ToString() == pointType)
                {
                    ret.Add(new DTO.Draft.AnalysisPlayer()
                    {
                        Name = row["PlayerName"].ToString(),
                        PlayerPosition = row["PlayerPosition"].ToString(),
                        TeamPosition = row["Position"].ToString(),
                        Points = decimal.Parse(row["Points"].ToString()),
                        Pick = int.Parse(row["Pick"].ToString())
                    });
                }
            }

            return ret;
        }

        private List<Domain.PlayerTeam> SortDraftPicks(List<Domain.PlayerTeam> picks, int teamCount, int roundCount)
        {
            var sortedPicks = new List<Domain.PlayerTeam>();

            var sql = new DataLayer.TextExecutor(_settings.ConnectionString);
            sql.AddParam("@MaxTeams", teamCount);
            sql.AddParam("@MaxRounds", roundCount);
            var dt = sql.ExecuteDataTable("SELECT Pick FROM DraftPickIndex WHERE MaxTeams = @MaxTeams AND MaxRounds = @MaxRounds ORDER BY DraftboardSortValue");
            if (dt.Rows.Count == 0)
            {
                throw new Exception("No draft sort rows");
            }
            foreach (System.Data.DataRow row in dt.Rows)
            {
                var pick = picks.Where(p => p.Overall == int.Parse(row["Pick"].ToString())).FirstOrDefault();
                sortedPicks.Add(pick);
            }
            return sortedPicks;
        }

        private List<List<int>> BuildDraftLayout(int teamCount, int roundCount)
        {
            var layout = new List<List<int>>();
            var pickIndex = 0;

            for (var roundIndex = 1; roundIndex <= roundCount; roundIndex++)
            {
                var round = new List<int>();
                for (var teamIndex = 1; teamIndex <= teamCount; teamIndex++)
                {
                    pickIndex++;
                    round.Add(pickIndex);
                }
                layout.Add(round);
            }

            return layout;
        }
    }
    public interface IDraftService
    {
        Result<IEnumerable<Domain.Team>> GetTeams(int leagueId, int year);

        Result<IEnumerable<Domain.Player>> GetPlayers(int leagueId, int year);

        Result MakePick(DTO.Draft.Pick info);

        Result<DTO.Draft.Board> GetBoard(int leagueId, int year);

        Result<List<DTO.Draft.PickPosition>> GetPickPositions(int leagueId, int year);

    }
}
