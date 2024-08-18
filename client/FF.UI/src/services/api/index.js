import axios from "axios";
import * as config from "../../config/config.json";

let baseUrl = config.apiRoot;
if (!baseUrl) {
  baseUrl = window.location.origin + "/static/api";
}

const apiGet = async (url) => {
  const result = await axios.get(url);
  return result.data;
};

const apiPost = async (url, data) => {
  const result = await axios.post(url, data);
  return result.data;
};

const getFullUrl = (staticUrl, dynamicUrl) => {
  let fullUrl = baseUrl + "/";
  if (config.apiStatic) {
    fullUrl = fullUrl + staticUrl + ".json?cc=" + config.apiCacheControl;
  } else {
    fullUrl = fullUrl + dynamicUrl;
  }
  return fullUrl;
};

const api = {
  urlRoot: baseUrl,
  getLeagues: async () => {
    const url = getFullUrl("leagues/all", "leagues");
    return await apiGet(url);
  },

  getManagers: async (managerId) => {
    let url = getFullUrl("managers/all", "managers");
    if (managerId) {
      url = getFullUrl("managers/" + managerId, "managers/" + managerId);
    }
    return await apiGet(url);
  },

  getPlayers: async (year) => {
    const url = getFullUrl("players/" + year, "players?year=" + year);
    return await apiGet(url);
  },

  getDraft: async (leagueId, year) => {
    const url = getFullUrl(
      "drafts/" + leagueId + "/" + year,
      "draft/board?leagueId=" + leagueId + "&year=" + year
    );
    return await apiGet(url);
  },

  getDraftTeams: async (leagueId, year) => {
    const url = getFullUrl(
      null,
      "draft/teams?leagueId=" + leagueId + "&year=" + year
    );
    return await apiGet(url);
  },

  getDraftPlayers: async (leagueId, year) => {
    const url = getFullUrl(
      null,
      "draft/players?leagueId=" + leagueId + "&year=" + year
    );
    return await apiGet(url);
  },

  getDraftPositions: async (leagueId, year) => {
    const url = getFullUrl(
      null,
      "draft/positions?leagueId=" + leagueId + "&year=" + year
    );
    return await apiGet(url);
  },

  postDraftPick: async (pick) => {
    const url = getFullUrl(null, "draft/pick");
    return await apiPost(url, pick);
  },

  getStats: async (leagueId, year, managerType) => {
    const url = getFullUrl(
      "stats/" + leagueId + "/" + year + "_" + managerType,
      "stats?leagueId=" +
        leagueId +
        "&year=" +
        year +
        "&managerType=" +
        managerType
    );
    return await apiGet(url);
  },
};

export default api;
