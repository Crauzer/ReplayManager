using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClientAPI
{
    internal sealed class APIRequest
    {
        [Serializable]
        private struct AuthorizationIdToken
        {
            public int expiry;
            public string token;
        }

        private static readonly APIRequest _instance = new APIRequest();
        private NetworkCredential _networkCredentials;
        private int _port;
        public String IDToken { private set; get; }

        public static APIRequest Instance
        {
            get { return _instance; }
        }

        public bool Setup(String username, String password, int port)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _networkCredentials = new NetworkCredential(username, password);
            _port = port;
            String response;
            if (Get("/rso-auth/v1/authorization/id-token", out response))
            {
                //TODO: Handle Expiry but its always 24 hours after initial log in
                AuthorizationIdToken auth = JsonHandler.LoadJson<AuthorizationIdToken>(response);
                IDToken = auth.token;
                return true;
            }

            //Either not logged in or in login queue
            return false;
        }

        public bool Post(String url, String json, out String resp)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                var postData = json;
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.ServerCertificateValidationCallback += (sender1, certificate, chain, sslPolicyErrors) => true;
                request.Credentials = _networkCredentials;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse) request.GetResponse();

                resp = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return true;
            }
            catch (WebException we)
            {
                var response = we.Response as HttpWebResponse;
                if (response == null)
                    throw;
                resp = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return false;
            }
        }

        public bool Patch(String url, String json, out String resp)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var postData = json;
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "PATCH";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.ServerCertificateValidationCallback += (sender1, certificate, chain, sslPolicyErrors) => true;
                request.Credentials = _networkCredentials;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                resp = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return true;
            }
            catch (WebException we)
            {
                var response = we.Response as HttpWebResponse;
                if (response == null)
                    throw;
                resp = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return false;
            }
        }

        public bool Get(String url, out String resp)
        {
            try
            {
                HttpWebRequest request;
                if (!url.Contains("127.0.0.1"))
                    url = "https://127.0.0.1:" + _port.ToString() + url;
                Uri requestUri = new Uri(url);
                string method = "Get";
                string contentType = @"application/x-www-form-urlencoded";
                request = (HttpWebRequest) WebRequest.Create(requestUri);
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                request.Credentials = _networkCredentials;
                request.Method = method;
                request.ContentType = contentType;
                request.KeepAlive = true;
                resp = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
                return true;
            }
            catch (WebException we)
            {
                var response = we.Response as HttpWebResponse;
                if (response == null)
                    throw;
                resp = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return false;
            }
        }

        public bool Get(String url, out Byte[] resp)
        {
            try
            {
                HttpWebRequest request;
                if (!url.Contains("127.0.0.1"))
                    url = "https://127.0.0.1:" + _port.ToString() + url;
                Uri requestUri = new Uri(url);
                string method = "Get";
                string contentType = @"application/x-www-form-urlencoded";
                request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                request.Credentials = _networkCredentials;
                request.Method = method;
                request.ContentType = contentType;
                request.KeepAlive = true;
                using (var memstream = new MemoryStream())
                {
                    new StreamReader(request.GetResponse().GetResponseStream()).BaseStream.CopyTo(memstream);
                    resp = memstream.ToArray();
                }
                return true;
            }
            catch (WebException we)
            {
                var response = we.Response as HttpWebResponse;
                if (response == null)
                    throw;
                using (var memstream = new MemoryStream())
                {
                    new StreamReader(response.GetResponseStream()).BaseStream.CopyTo(memstream);
                    resp = memstream.ToArray();
                }
                return false;
            }
        }
    }
}
