using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAuthProtocol;

namespace Dropbox.Api
{
    public class DropboxApi
    {
        private readonly OAuthToken _accessToken;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;

        public DropboxApi(string consumerKey, string consumerSecret, OAuthToken accessToken)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _accessToken = accessToken;
        }

        private string GetResponse(Uri uri)
        {
            var oauth = new OAuth();
            var requestUri = oauth.SignRequest(uri, _consumerKey, _consumerSecret, _accessToken);
            var request = (HttpWebRequest) WebRequest.Create(requestUri);
            request.Method = WebRequestMethods.Http.Get;
            var response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }

        private static T ParseJson<T>(string json) where T : class, new()
        {
            var jobject = JObject.Parse(json);
            return JsonConvert.DeserializeObject<T>(jobject.ToString());
        }

        public Account GetAccountInfo()
        {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), "account/info");
            var json = GetResponse(uri);
            return ParseJson<Account>(json);
        }

        public File GetFiles(string root, string path)
        {
            var uri = new Uri(new Uri(DropboxRestApi.BaseUri), String.Format("metadata/{0}/{1}", root, path));
            var json = GetResponse(uri);
            return ParseJson<File>(json);
        }
    }
}
