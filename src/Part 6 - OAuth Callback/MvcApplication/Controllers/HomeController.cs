using System;
using System.Web;
using System.Web.Mvc;
using OAuthProtocol;
using Dropbox.Api;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        private const string ConsumerKey = "your application key";
        private const string ConsumerSecret = "your application secret";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthorizeDropbox()
        {            
            var oauth = new OAuth();
            var requestToken = oauth.GetRequestToken(new Uri(DropboxRestApi.BaseUri), ConsumerKey, ConsumerSecret);

            Session["requestToken"] = requestToken;

            var action = Url.Action("Authorized", "Home", null, Request.Url.Scheme);
            var callbackUri = new Uri(action);            

            var authorizeUri = oauth.GetAuthorizeUri(new Uri(DropboxRestApi.AuthorizeBaseUri), requestToken, callbackUri);

            return Redirect(authorizeUri.ToString());
        }

        public ActionResult Authorized(string oauth_token, string uid)
        {
            var requestToken = (OAuthToken) Session["requestToken"];

            var oauth = new OAuth();
            
            var accessToken = oauth.GetAccessToken(new Uri(DropboxRestApi.BaseUri), ConsumerKey, ConsumerSecret, requestToken);

            ViewBag.UserId = uid;
            ViewBag.AccessToken = accessToken;

            return View();
        }
    }
}
