using System;
using Dropbox.Api;
using System.Diagnostics;
using System.Threading;
using OAuthProtocol;

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
            var accessToken = GetAccessToken();

            var api = new DropboxApi(ConsumerKey, ConsumerSecret, accessToken);

            var account = api.GetAccountInfo();
            Console.WriteLine(account.DisplayName);
            Console.WriteLine(account.Email);

            var total = account.Quota.Total / (1024 * 1024);
            var used = (account.Quota.Normal + account.Quota.Shared) / (1024 * 1024);

            Console.WriteLine(String.Format("Dropbox: {0}/{1} Mb used", used, total));

            var publicFolder = api.GetFiles("dropbox", "Public");
            foreach (var file in publicFolder.Contents)
            {
                Console.WriteLine(file.Path);
            }

            Console.WriteLine();
            Console.WriteLine("Done. Press any key to continue...");
            Console.ReadKey();
        }
    }
}
