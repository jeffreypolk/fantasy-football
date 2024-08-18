/*
    bookmarklet:
    javascript:(function() {var script = document.createElement("script");script.src = 'https://s3.amazonaws.com/jeff0401-ff/Inject.js?id=' + new Date().getTime();var head = document.getElementsByTagName("head")[0];head.appendChild(script);}())
    */

String.prototype.replaceAll = function (search, replacement) {
  var target = this;
  return target.split(search).join(replacement);
};

var PAF = (function () {
  var internal = {
    dataId: "paf-data-textarea",

    showData: function (data) {
      navigator.clipboard.writeText(data).then(
        function () {
          console.log("Async: Copying to clipboard was successful!");
        },
        function (err) {
          console.error("Async: Could not copy text: ", err);
        }
      );
      if ($("#" + internal.dataId).length) {
        $("#" + internal.dataId).val(data);
      } else {
        //debugger;
        $("body").append(
          '<div id="' +
            internal.dataId +
            '-wrap" style="position:absolute;left:0px;top:0px;z-index:99999999"><textarea id="' +
            internal.dataId +
            '" rows="25" cols="100">' +
            data +
            "</textarea></div>"
        );
        $("body").click(function () {
          $("#" + internal.dataId + "-wrap").hide();
        });
        $("#" + internal.dataId + "-wrap").click(function (event) {
          event.stopPropagation();
        });
      }
      $("#" + internal.dataId + "-wrap").show();
      $("#" + internal.dataId).select();
    },
  };

  var external = {
    yahoo: {
      getPoints: function (year, type) {
        var sql = [];
        if (!year) {
          year = prompt(
            "What year are these points for?",
            new Date().getFullYear()
          );
        }
        if (!type) {
          type = prompt("Are these projected (P) or actual (A) points?", "P");
        }

        $(".ysf-player-name").each(function () {
          //console.log($($(this).parent().parent().parent().nextAll()[2]).find('div').html());
          var name = $(this).find("a").html();
          //debugger;
          var points = parseFloat(
            $(this).parent().parent().parent().parent().find(".Fw-b").html()
          );

          sql.push("UPDATE Player SET ");
          sql.push(
            type == "P" ? "ProjectedPoints" : "ActualPoints",
            " = ",
            points,
            " "
          );
          sql.push(
            "WHERE Year = ",
            year,
            " AND Name = '",
            name.replace(/'/g, "''"),
            "';\n"
          );
        });
        //console.log(sql);
        internal.showData(sql.join(""));
      },

      getPlayers: function () {
        var sql = [];
        var year = new Date().getFullYear();

        var players = [];
        var table = $(
          ".Table.Ta-start.Fz-xs.Table-mid.Table-px-sm.Table-interactive"
        );

        table.find("tbody tr").each(function () {
          var tr = $(this);

          var name = tr.find(".ysf-player-name a").text();
          var team = tr
            .find(".ysf-player-name span")
            .text()
            .split(" - ")[0]
            .toString()
            .toUpperCase();
          var pos = tr.find(".ysf-player-name span").text().split(" - ")[1];
          var points = $(tr.find("td")[7]).find("span").text();
          if (!points) {
            points = 0;
          }
          sql.push(
            "INSERT INTO Player ([Name], [NFLTeam], [Position], [Age], [Experience], [ADP], [DepthChart], [Year], [ActualPoints], [ProjectedPoints], [Odds])"
          );
          sql.push("\n");
          sql.push(
            "VALUES ('",
            name.replaceAll("'", "''"),
            "', '",
            team,
            "', '",
            pos,
            "', 0, 0, 0, 1, year(getdate()), 0, ",
            points,
            ", 0)"
          );
          sql.push("\n");
        });
        //console.log(sql);
        internal.showData(sql.join(""));
      },

      getStats: function () {
        var sql = [];
        var year = $("#seasonspec option:selected").text();
        var table = $("#standingstable");
        var rows = table.find("tbody tr");

        rows.each(function () {
          var tr = $(this);
          var tds = tr.find("td");
          //console.log(tds);
          var pos = external.yahoo.statPositions;

          if (tr.hasClass("division") === false) {
            var name = $(tds[pos.name]).find("a").text();
            var record = $(tds[pos.record]).text().toString().split("-");
            var wins = record[0];
            var losses = record[1];
            var ties = record[2];
            var pointsFor = $(tds[pos.pointsFor]).text();
            var pointsAgainst = $(tds[pos.pointsAgainst]).text();
            var moves = $(tds[pos.moves]).text();
            var finish = $(tds[pos.finish])
              .text()
              .toString()
              .replaceAll("*", "")
              .replaceAll(".", "");
            var isPlayoffs = false;
            var isConsolation = false;
            var isBottom = false;

            name = name ? name : "name";
            wins = wins ? wins : 0;
            losses = losses ? losses : 0;
            ties = ties ? ties : 0;
            pointsFor = pointsFor ? pointsFor : 0;
            pointsAgainst = pointsAgainst ? pointsAgainst : 0;
            moves = moves ? moves : 323;
            if (finish <= 4) {
              isPlayoffs = true;
            } else if (finish >= 5 && finish <= 8) {
              isConsolation = true;
            } else {
              isBottom = true;
            }

            sql.push("UPDATE FF.Team SET ");
            sql.push("Wins = ", wins, ", ");
            sql.push("Losses = ", losses, ", ");
            sql.push("Ties = ", ties, ", ");
            sql.push("PointsFor = ", pointsFor, ", ");
            sql.push("PointsAgainst = ", pointsAgainst, ", ");
            sql.push("Finish = ", finish, ", ");
            sql.push("IsPlayoffs = ", isPlayoffs ? 1 : 0, ", ");
            sql.push("IsConsolation = ", isConsolation ? 1 : 0, ", ");
            sql.push("IsBottom = ", isBottom ? 1 : 0, ", ");
            sql.push("Moves = ", moves, " ");
            sql.push(
              "WHERE [Year] = ",
              year,
              " AND [Name] = '",
              name.toString().replaceAll("'", "''"),
              "'"
            );
            sql.push("\n");
          }
        });
        //console.log(sql);
        internal.showData(sql.join(""));
      },

      statPositions: {
        name: 1,
        finish: 0,
        record: 2,
        pointsFor: 5,
        pointsAgainst: 6,
        moves: 9,
      },
    },

    cbs: {
      getADP: function () {
        var sql = [];
        $(".TableBase-table tbody tr").each(function () {
          var name = $(this)
            .find("td:nth-child(2)")
            .find(".CellPlayerName--long a")
            .html();
          var adp = parseFloat($(this).find("td:nth-child(4)").html());

          sql.push("UPDATE Player SET ");
          sql.push("ADP = ", adp, " ");
          sql.push(
            "WHERE Year = ",
            new Date().getFullYear(),
            " AND Name = '",
            name.replace(/'/g, "''"),
            "';\n"
          );
        });

        internal.showData(sql.join(""));
      },
    },
  };

  var init = (function () {
    // include jquery
    javascript: (function () {
      var el = document.createElement("div"),
        b = document.getElementsByTagName("body")[0],
        otherlib = !1,
        msg = "";
      (el.style.position = "fixed"),
        (el.style.height = "32px"),
        (el.style.width = "220px"),
        (el.style.marginLeft = "-110px"),
        (el.style.top = "0"),
        (el.style.left = "50%"),
        (el.style.padding = "5px 10px"),
        (el.style.zIndex = 9999999),
        (el.style.fontSize = "12px"),
        (el.style.color = "#222"),
        (el.style.backgroundColor = "#f99");
      function showMsg() {
        var txt = document.createTextNode(msg);
        el.appendChild(txt),
          b.appendChild(el),
          window.setTimeout(function () {
            (txt = null),
              typeof jQuery == "undefined"
                ? b.removeChild(el)
                : (jQuery(el).fadeOut("slow", function () {
                    jQuery(this).remove();
                  }),
                  otherlib && (window.$jq = jQuery.noConflict()));
          }, 2500);
      }
      if (typeof jQuery != "undefined")
        return (
          (msg = "This page already using jQuery v" + jQuery.fn.jquery),
          showMsg()
        );
      typeof $ == "function" && (otherlib = !0);
      function getScript(url, success) {
        var script = document.createElement("script");
        script.src = url;
        var head = document.getElementsByTagName("head")[0],
          done = !1;
        (script.onload = script.onreadystatechange =
          function () {
            !done &&
              (!this.readyState ||
                this.readyState == "loaded" ||
                this.readyState == "complete") &&
              ((done = !0),
              success(),
              (script.onload = script.onreadystatechange = null),
              head.removeChild(script));
          }),
          head.appendChild(script);
      }
      getScript("https://code.jquery.com/jquery-3.2.1.min.js", function () {
        return (
          typeof jQuery == "undefined"
            ? (msg = "Sorry, but jQuery was not able to load")
            : ((msg = "This page is now jQuerified with v" + jQuery.fn.jquery),
              otherlib && (msg += " and noConflict(). Use $jq(), not $().")),
          showMsg()
        );
      });
    })();
  })();

  return external;
})();

// PAF.yahoo.getPlayers();
