using FF.Backend.Repositories.Framework;
using FF.Backend.Results;
using io = System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FF.Backend.Services
{
    public class GenerationService : IGenerationService
    {
        private readonly ILeagueService _leagueService;
        private readonly IManagerService _managerService;
        private readonly IPlayerService _playerService;
        private readonly ITeamService _teamService;
        private readonly ISettings _settings;
        private readonly IDraftService _draftService;
        private readonly IStatsService _statsService;

        private IUnitOfWork _unitOfWork { get; }

        // use this when building/deploying new web or stats
        private string _outputPath = @"C:\Code\jeffreypolk\fantasy-football\client\FF.UI\build\static\api";
        
        // use this for local testing
        //private string _outputPath = @"C:\my\FantasyFootball\FF.LocalApi\api";
        private int _startYear = 2003;

        public GenerationService(
            IUnitOfWork unitOfWork,
            ILeagueService leagueService,
            IManagerService managerService,
            IPlayerService playerService,
            ITeamService teamService,
            IDraftService draftService,
            ISettings settings,
            IStatsService statsService
            ) 
        {
            _unitOfWork = unitOfWork;
            _leagueService = leagueService;
            _managerService = managerService;
            _playerService = playerService;
            _teamService = teamService;
            _settings = settings;
            _draftService = draftService;
            _statsService = statsService;
        }

        public Result GenerateDataFiles()
        {
            InitOutput();
            GenerateLeagues();
            GenerateManagers();
            //GeneratePlayers();
            GenerateDrafts();
            GenerateStats();
            return new Result();
        }

        private void GenerateLeagues()
        {
            var leagues = _leagueService.GetAll().Data;
            CreateFolder("leagues");
            CreateJsonFile("leagues\\all.json", leagues);
            foreach (var league in leagues)
            {
                CreateJsonFile($"leagues\\{league.Id}.json", league);
            }
        }

        private void GenerateManagers()
        {
            var managers = _managerService.GetAll().Data;
            CreateFolder("managers");
            CreateJsonFile("managers\\all.json", managers);
            foreach (var manager in managers)
            {
                var data = _managerService.GetById(manager.Id).Data;
                data.Teams = data.Teams.OrderByDescending(t => t.Year).ToList();
                CreateJsonFile($"managers\\{manager.Id}.json", data);
            }
        }

        private void GeneratePlayers()
        {
            CreateFolder("players");
            var allPlayers = new List<Domain.Player>();

            for(var year = _startYear; year <= DateTime.Now.Year; year++)
            {
                var players = _playerService.GetByYear(year).Data.OrderByDescending(p => p.ActualPoints == 0 ? p.ProjectedPoints : p.ActualPoints).ToList();
                allPlayers.AddRange(players);
                if (players.Count > 0)
                {
                    var csv = BuildPlayersCsv(players);
                    CreateFile($"players\\{year}.csv", csv.ToString());
                    CreateJsonFile($"players\\{year}.json", players);
                }
            }
            var allCsv = BuildPlayersCsv(allPlayers.OrderByDescending(p=>p.Year).ToList());
            CreateFile($"players\\all.csv", allCsv.ToString());
            CreateJsonFile($"players\\all.json", allPlayers);
        }

        
        private System.Text.StringBuilder BuildPlayersCsv(List<Domain.Player> players)
        {
            var ret = new System.Text.StringBuilder();

            ret.AppendLine("Name,Year,Position,Team,ADP,ProjectedPoints,ActualPoints");

            foreach (var player in players)
            {
                ret.AppendLine($"{player.Name},{player.Year},{player.Position},{player.NFLTeam},{player.ADP},{player.ProjectedPoints},{player.ActualPoints}");
            }
            return ret;
        }

        private void GenerateDrafts()
        {
            CreateFolder("drafts");
            var leagues = _leagueService.GetAll().Data;
            foreach(var league in leagues)
            {
                CreateFolder($"drafts\\{league.Id}");
                for (var year = league.Established; year <= DateTime.Now.Year; year++)
                {
                    var board = _draftService.GetBoard(league.Id, year);
                    if (board.Data != null)
                    {
                        CreateJsonFile($"drafts\\{league.Id}\\{year}.json", board.Data);
                    }
                }
            }
            
        }

        private void GenerateStats()
        {
            DTO.Stats.StatInfo stats;

            CreateFolder("stats");
            var leagues = _leagueService.GetAll().Data;
            foreach (var league in leagues)
            {
                CreateFolder($"stats\\{league.Id}");
                for (var year = league.Established; year <= DateTime.Now.Year; year++)
                {
                    stats = _statsService.GetStats(league.Id, year, year, true).Data;
                    CreateJsonFile(GetStatFileName(league.Id, year.ToString(), true), stats);
                    stats = _statsService.GetStats(league.Id, year, year, false).Data;
                    CreateJsonFile(GetStatFileName(league.Id, year.ToString(), false), stats);
                }

                // dont include current year if the stats aren't entered
                var finalYear = GetFinalYearOfStats(league.Id);

                // last 5 years
                stats = _statsService.GetStats(league.Id, finalYear - 4, finalYear, true).Data;
                CreateJsonFile(GetStatFileName(league.Id, "LAST5", true), stats);
                stats = _statsService.GetStats(league.Id, finalYear - 4, finalYear, false).Data;
                CreateJsonFile(GetStatFileName(league.Id, "LAST5", false), stats);
                // last 10 years
                stats = _statsService.GetStats(league.Id, finalYear - 9, finalYear, true).Data;
                CreateJsonFile(GetStatFileName(league.Id, "LAST10", true), stats);
                stats = _statsService.GetStats(league.Id, finalYear - 9, finalYear, false).Data;
                CreateJsonFile(GetStatFileName(league.Id, "LAST10", false), stats);
                // all years
                stats = _statsService.GetStats(league.Id, league.Established, DateTime.Now.Year, true).Data;
                CreateJsonFile(GetStatFileName(league.Id, "ALL", true), stats);
                stats = _statsService.GetStats(league.Id, league.Established, DateTime.Now.Year, false).Data;
                CreateJsonFile(GetStatFileName(league.Id, "ALL", false), stats);
            }

        }

        private int GetFinalYearOfStats(int leagueId)
        {
            var sql = new DataLayer.TextExecutor(_settings.ConnectionString);
            sql.AddParam("@LeagueId", leagueId);
            var year = sql.ExecuteScalarInt("select max(Year) from Team where LeagueId = @LeagueId and Finish > 0");
            return year;
        }

        private string GetStatFileName(int leagueId, string year, bool includeActive)
        {
            var managerType = includeActive ? "ACTIVE" : "ALL";
            return $"stats\\{leagueId}\\{year}_{managerType}.json";
        }

        private void InitOutput()
        {
            if (io.Directory.Exists(_outputPath))
            {
                foreach(var folder in io.Directory.GetDirectories(_outputPath))
                {
                    io.Directory.Delete(folder, true);
                }
            }
            else
            {
                io.Directory.CreateDirectory(_outputPath);
            }
        }

        private void CreateFolder(string path)
        {
            var fullPath = $"{_outputPath}\\{path}";
            if (!io.Directory.Exists(fullPath))
            {
                io.Directory.CreateDirectory(fullPath);
            }
        }

        private void CreateFile(string path, string data)
        {
            var fullPath = $"{_outputPath}\\{path}";
            io.File.WriteAllText(fullPath, data);
        }

        private void CreateJsonFile(string path, object data)
        {
            var objToSerialize = new
            {
                Data = data,
                StatusCode = "200",
                Message = (string)null,
                Succeeded = true
            };
            var json = System.Text.Json.JsonSerializer.Serialize(objToSerialize, new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            CreateFile(path, json);
        }
    }
    public interface IGenerationService 
    {
        Result GenerateDataFiles();
    }
}
