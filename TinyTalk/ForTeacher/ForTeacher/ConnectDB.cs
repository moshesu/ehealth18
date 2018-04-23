using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Windows.Data.Json;
using Windows.Web.Http;
using System.IO;
using Newtonsoft.Json;

namespace ForTeacher
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
                user getUser = ReadToObject(httpResponseBody);
                return getUser;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> CreateUserAsync(user user)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/users";
            string json = WriteFromObject(user);

            try
            {
                //Send the POSR request
                HttpStringContent stringContent = new HttpStringContent(json.ToString());

                System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, completeUri);
                request.Content = new System.Net.Http.StringContent(json,
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

        public static user ReadToObject(string json)
        {
            user deserializedUser = new user();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as user;
            return deserializedUser;
        }

        public static string WriteFromObject(user user)
        {
            //Create a stream to serialize the object to.
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(user));
            ser.WriteObject(ms, user);
            byte[] json = ms.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public async Task<string> ValidateUserAsync(string email, string firstName, string lastName, string childYear,
           string gardenName, string password)
        {
            string msg = "success";

            if (email == "" || firstName == "" || lastName == ""
           || childYear == "" || gardenName == "" || password == "")
            {
                msg = "Invalid input";
            }
            if (email.IndexOf("@") == -1)
            {
                msg = "Non valid email";
            }
            else
            {
                user res = await GetUserByMailAsync(email);
                if (res != null)
                {
                    msg = "User already exist";
                }
            }
            //check if year is int 
            var isNumeric = int.TryParse(childYear, out var n);
            if (isNumeric == false)
            {
                msg = "Invalid child birth year";
            }
            return msg;
        }

        public async Task<user[]> GetGardenChildren(string garden)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/users?garden=" + garden;

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
                user[] gardenUsers = JsonConvert.DeserializeObject<user[]>(httpResponseBody);

                return gardenUsers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<symbol[]> GetUserAllCountersAsync(string email)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/counters?email="
                + email;
            
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

                symbol[] userSymbolUsage = JsonConvert.DeserializeObject<symbol[]>(httpResponseBody);

                return userSymbolUsage;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<symbol[]> GetUserCounterAsync(symbol userSymbol)
        {
            string completeUri = "http://childappapiservice.azurewebsites.net/api/counters?email="
                + userSymbol.email + "&symbolName=" + userSymbol.symbolName;
            
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

                symbol[] userSymbolUsage = JsonConvert.DeserializeObject<symbol[]>(httpResponseBody);

                return userSymbolUsage;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }
}