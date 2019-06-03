using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheHockeyGame.Data;
using TheHockeyGame.Properties;
using TheHockeyGame.Models;

namespace TheHockeyGame.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewModel vm = new ViewModel();
            if(TempData["game-create"] != null)
            {
                vm.message = (string)TempData["game-create"];
            }
            if(TempData["player-added"] != null)
            {
                vm.message = (string)TempData["player-added"];
            }
            return View(vm);
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            DataBase db = new DataBase(Settings.Default.ConStr);
            var games = db.GetNextGame();
            SignInViewModel svm = new SignInViewModel
            {
                Gamer = games,
                PeopleCount = db.CountPplPerGame(games.Id)
            };
            return View(svm);
        }

        [HttpPost]
        public ActionResult AddGame(Gamer gamer)
        {
            DataBase db = new DataBase(Settings.Default.ConStr);
            db.CreateGame(gamer);
            TempData["game-create"] = "game was created";
            return Redirect("/");
        }

    }
}

//On the home page, display a welcome message and two links
//"Sign up for the next upcoming game" and "Sign up for the weekly notification". 
//When the user clicks on the first link, they should be 
//taken to the next upcoming game signup sheet.HOWEVER, at this point, there are three
//possible outcomes: 
//1) Game has not been posted yet, in which case display a message "Game has not been posted yet". 
//2) Game has been posted but it's full in which case display a message "Game is full". 
//3) Game has been posted and is not full, in which case display a form where
//they can enter their first name, last name, and email address. 
//(As an added bonus, store their information in cookies so you can prefill this
//form in the future).

//On the second home page link ("Sign up for the weekly notification"), 
//they should be taken to a page that has a form that allows them
//to fill out their first name, last name and email. 
//This will be used for when the admin creates a new game, to notify all the people via email
//that a new game has been posted.

//On top, in the nav bar, there should be two more links for Admin. 
//1) Create Event, and 2) View Event History.When the Create Event link
//is clicked, they should get taken to a page where they can create a new game.
//They should select the max amount of people, as well as 
//the date of the game. When the View Event History link is hit, 
//they should get taken to a page that display a summary of all games.Each
//row should show the date of the game, the amount of people that signed up, 
//the max amount allowed for that game, as well as a link
//that says "View Details". When that's clicked, they should get taken to a 
//page that shows all the people that signed up for that one game.