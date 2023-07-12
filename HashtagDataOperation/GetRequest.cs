using System.Net;

namespace HashtagDataOperation
{
    public class GetRequest
    {
        HttpWebRequest _request;
        string _adress;
        public string Responce { get; set; }
        public GetRequest(string adress)
        {
            _adress = adress;
        }
        public void Run()
        {
            _request = (HttpWebRequest)WebRequest.Create(_adress);
            _request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null)  Responce = new StreamReader(stream).ReadToEnd();
            }
            catch { }
        }
    }
}
