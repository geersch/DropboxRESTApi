using System;
using Dropbox.Api;
using System.Diagnostics;
using System.Threading;
using OAuthProtocol;
using System.Web;

namespace ConsoleApplication
{
    class Program
    {
        private const string ConsumerKey = "your API key";
        private const string ConsumerSecret = "your API secret";

        private static OAuthToken GetAccessToken()
        {
            var oauth = new OAuth();

            var requestToken = oauth.GetRequestToken(new Uri(DropboxRestApi.BaseUri), ConsumerKey, ConsumerSecret);

            var authorizeUri = oauth.GetAuthorizeUri(new Uri(DropboxRestApi.AuthorizeBaseUri), requestToken);
            Process.Start(authorizeUri.AbsoluteUri);
            Thread.Sleep(5000); // Leave some time for the authorization step to complete

            return oauth.GetAccessToken(new Uri(DropboxRestApi.BaseUri), ConsumerKey, ConsumerSecret, requestToken);
        }

        static void Main()
        {
            // Uncomment the following line or manually provide a valid token so that you
            // don't have to go through the authorization process each time.
            // var accessToken = GetAccessToken();
            var accessToken = new OAuthToken("token", "secret");

            var api = new DropboxApi(ConsumerKey, ConsumerSecret, accessToken);

            var account = api.GetAccountInfo();
            Console.WriteLine(account.DisplayName);
            Console.WriteLine(account.Email);

            var total = account.Quota.Total / (1024 * 1024);
            var used = (account.Quota.Normal + account.Quota.Shared) / (1024 * 1024);

            Console.WriteLine(String.Format("Dropbox: {0}/{1} Mb used", used, total));
            Console.WriteLine();

            // Create a folder
            var folder = api.CreateFolder("dropbox", "/test");
            Console.WriteLine("Folder created.");
            Console.WriteLine(String.Format("Root: {0}", folder.Root));
            Console.WriteLine(String.Format("Path: {0}", folder.Path));
            Console.WriteLine(String.Format("Modified: {0}", folder.Modified));                                    

            // Move a folder
            folder = api.Move("dropbox", "/test", "/temp");

            // Delete a folder
            folder = api.Delete("dropbox", "/temp");

            Console.WriteLine();
            Console.WriteLine("Done. Press any key to continue...");
            Console.ReadKey();
        }
    }
}
