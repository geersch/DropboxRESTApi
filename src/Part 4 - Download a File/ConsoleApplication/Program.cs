﻿using System;
using Dropbox.Api;
using System.Diagnostics;
using System.Threading;
using OAuthProtocol;
using System.Web;

namespace ConsoleApplication
{
    class Program
    {
        private const string ConsumerKey = "Your application key";
        private const string ConsumerSecret = "Your application secret";

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
            var file = api.DownloadFile("dropbox", "Public/YourFileName.ext");
            file.Save(@"C:\YourFileName.ext");

            Console.WriteLine();
            Console.WriteLine("Done. Press any key to continue...");
            Console.ReadKey();
        }
    }
}
