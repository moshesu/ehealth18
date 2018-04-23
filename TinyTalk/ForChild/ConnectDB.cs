using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Runtime.Serialization.Json;
using Windows.Web.Http;
using Newtonsoft.Json;

namespace ForChild
{
    class ConnectDB
    {
        public async Task<user> GetUserByMailAsync(string email)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/users?email=" + email;
            Uri requestUri = new Uri(completeUri);

            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                user getUser = ReadToObjectUser(httpResponseBody);
                return getUser;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async void UpdateUserCounterAsync(symbol userSymbol)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/counters";

            string json = WriteFromObject(userSymbol);

            try
            {
                //Send the PATCH request
                HttpStringContent stringContent = new HttpStringContent(json.ToString());

                System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod("PATCH"), completeUri);
                request.Content = new StringContent(json,
                                                    Encoding.UTF8,
                                                    "application/json");//CONTENT-TYPE header

                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage response = await client.SendAsync(request);
            }

            catch (Exception ex)
            {
            }
        }

        //position 0 for user type : Parent or Child
        //position 1 will be for the user email itself
        public async Task<bool> AddUserContactsAsync(string[] emails)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/contacts";
            string json = JsonConvert.SerializeObject(emails);
            try
            {
                //Send the POST request
                HttpStringContent stringContent = new HttpStringContent(json.ToString());
                System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod("POST"), completeUri);
                request.Content = new StringContent(json,
                Encoding.UTF8,
                "application/json");//CONTENT-TYPE header
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage response = await client.SendAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<userContacts> GetUserContactsAsync(string email)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/contacts?email=" + email;
            Uri requestUri = new Uri(completeUri);

            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                userContacts userContacts = JsonConvert.DeserializeObject<userContacts>(httpResponseBody);
                return userContacts;
            }

            catch (Exception ex)
            {
            }

            return null;
        }

        public async Task<string> GetGardenTeacher(string email, string type)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/users?email=" + email
                + "&type=" + type;
            Uri requestUri = new Uri(completeUri);
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                user getUser = ReadToObjectUser(httpResponseBody);
                return getUser.email;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static user ReadToObjectUser(string json)
        {
            user deserializedUser = new user();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as user;
            return deserializedUser;
        }

        public static string WriteFromObject(symbol userSymbol)
        {
            //Create a stream to serialize the object to.
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(symbol));
            ser.WriteObject(ms, userSymbol);
            byte[] json = ms.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
    }
}
