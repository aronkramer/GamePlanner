using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheHockeyGame.Data;

namespace TheHockeyGame.Models
{
    public class SignInViewModel
    {
        public Gamer Gamer { get; set; }
        public int PeopleCount { get; set; }
    }
}