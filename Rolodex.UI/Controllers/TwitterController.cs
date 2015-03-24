using Rolodex.Domain.Abstract;
using Rolodex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TweetSharp;

namespace Rolodex.UI.Controllers
{
    public class TwitterController : Controller
    {
        IPeopleRepository repo;

        public TwitterController(IPeopleRepository rep)
        {
            repo = rep;
        }

        //
        // GET: /Twitter/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Authorize()
        {
            TwitterService service = new TwitterService("F7IYzxyS2hjDtelTudaxJVs27", "a0SWTlfrRLHOd03F4nOy4KHkbPKo3mKoy6uuF2McPOyb2YfobU");

            OAuthRequestToken requestToken = service.GetRequestToken();
            Uri uri = service.GetAuthorizationUri(requestToken);
            Process.Start(uri.ToString());
            Session.Add("token", requestToken);

            return View("Index");
        }

        public ActionResult AuthorizeCallback(string verifier)
        {

            TwitterService service = new TwitterService("F7IYzxyS2hjDtelTudaxJVs27", "a0SWTlfrRLHOd03F4nOy4KHkbPKo3mKoy6uuF2McPOyb2YfobU");

            OAuthAccessToken access = service.GetAccessToken((OAuthRequestToken)Session["token"], verifier);

            service.AuthenticateWith(access.Token, access.TokenSecret);

            IEnumerable<TwitterUser> users = service.ListFriends(new ListFriendsOptions { UserId = access.UserId });
            this.ProcessContacts(users);
            return RedirectToAction("Index", "People");
        }

        private void ProcessContacts(IEnumerable<TwitterUser> users)
        {
            List<Person> people = new List<Person>();
            foreach (var item in users)
            {
                people.Add(new Person { Name = item.Name, LastName="", Title = item.ScreenName,
                                        Url = item.Url, Description = item.Description, Email="", Phone="" });
            }
            repo.MassiveInsert(people);
        }

    }
}
