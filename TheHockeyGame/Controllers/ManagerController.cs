using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheHockeyGame.Data;
using TheHockeyGame.Models;
using TheHockeyGame.Properties;

namespace TheHockeyGame.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult AlwaysIn()
        {
            return View();
        }

        public ActionResult WeeklyGuy()
        {
            return Redirect("/");
        }
        
        public ActionResult History()
        {
            DataBase db = new DataBase(Settings.Default.ConStr);
            return View(db.GetAllGames());
        }

        [HttpPost]
        public ActionResult JoinGame(Players player)
        {
            DataBase db = new DataBase(Settings.Default.ConStr);
            db.AddPlayer(player);
            TempData["player-added"] = "player was added";
            return Redirect("/");
        }

        public ActionResult GameView(int id)
        {
            DataBase db = new DataBase(Settings.Default.ConStr);
            return View(db.PlayersByGame(id));
        }
    }
}