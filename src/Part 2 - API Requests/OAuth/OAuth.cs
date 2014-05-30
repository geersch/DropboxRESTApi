using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace OAuth
{
    public class OAuth
    {
        private readonly string _consumerKey ;
        private readonly string _consumerSecret;
        private readonly OAuthBase _oAuthBase;

        public OAuth(string consumerKey, string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _oAuthBase = new OAuthBase();
        }

        public OAuthToken GetRequestToken(Uri baseUri)
        {
            var uri = new Uri(baseUri, "oauth/request_token");

            uri = SignRequest(uri);

            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;

            var response = request.GetResponse();

            var queryString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var parts = queryString.Split('&');
            var token = parts[1].Substring(parts[1].IndexOf('=') + 1);
            var secret = parts[0].Substring(parts[0].IndexOf('=') + 1);

            return new OAuthToken(token, secret);
        }

        public Uri GetAuthorizeUri(Uri baseUri, OAuthToken requestToken)
        {
            var queryString = String.Format("oauth_token_secret={0}&oauth_token={1}", requestToken.Secret, requestToken.Token);
            var authorizeUri = String.Format("{0}{1}?{2}", baseUri, "oauth/authorize", queryString);
            return new Uri(authorizeUri);
        }

        public OAuthToken GetAccessToken(Uri baseUri, OAuthToken requestToken)
        {
            var uri = new Uri(baseUri, "oauth/access_token");

            uri = SignRequest(uri, requestToken);

            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;

            var response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var accessToken = reader.ReadToEnd();

            var parts = accessToken.Split('&');
            var token = parts[1].Substring(parts[1].IndexOf('=') + 1);
            var secret = parts[0].Substring(parts[0].IndexOf('=') + 1);

            return new OAuthToken(token, secret);
        }

        private Uri SignRequest(Uri uri, OAuthToken requestToken = null)
        {
            var nonce = _oAuthBase.GenerateNonce();
            var timestamp = _oAuthBase.GenerateTimeStamp();
            string parameters;
            string normalizedUrl;

            string token = requestToken == null ? String.Empty : requestToken.Token;
            string secret = requestToken == null ? String.Empty : requestToken.Secret;

            var signature = _oAuthBase.GenerateSignature(
                uri, _consumerKey, _consumerSecret,
                token, secret, "GET", timestamp,
                nonce, OAuthBase.SignatureTypes.HMACSHA1,
                out normalizedUrl, out parameters);

            signature = HttpUtility.UrlEncode(signature);

            var requestUri = new StringBuilder(uri.ToString());
            requestUri.AppendFormat("?oauth_consumer_key={0}&", _consumerKey);
            if (!String.IsNullOrEmpty(token))
                requestUri.AppendFormat("oauth_token={0}&", token);
            requestUri.AppendFormat("oauth_nonce={0}&", nonce);
            requestUri.AppendFormat("oauth_timestamp={0}&", timestamp);
            requestUri.AppendFormat("oauth_signature_method={0}&", "HMAC-SHA1");
            requestUri.AppendFormat("oauth_version={0}&", "1.0");
            requestUri.AppendFormat("oauth_signature={0}", signature);

            return new Uri(requestUri.ToString());
        }
    }
}