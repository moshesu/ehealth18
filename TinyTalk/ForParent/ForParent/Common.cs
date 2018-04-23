using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace ForParent
{
     class Common
    {
        public static string who_am_i = "";
        public static string myChild = "";
        public static bool isConectet = false;

        public static async Task<bool> GetUserFromFileAsync()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
                if(storageFolder == null)
                {
                    return false;
                }
                Windows.Storage.StorageFile userFile =
                    await storageFolder.GetFileAsync("userParent.txt");
                if (userFile == null)
                {
                    return false;
                }
                string text = await Windows.Storage.FileIO.ReadTextAsync(userFile);
                if (String.IsNullOrEmpty(text) ||String.IsNullOrWhiteSpace(text) || text.Equals(""))
                {
                    return false;
                }
                else
                {
                    string[] fileResult = text.Split('+');
                    who_am_i = fileResult[0];
                    if (fileResult.Length >= 3)
                    {
                        myChild = fileResult[2];
                    }
                    isConectet = true;
                    return true;
                }
            }
            
            catch
            {
                return false;
            }
        }

        public static async Task<bool> DeleteFileAsync(string fileName)
        {
            //replace exisiting file with empty file 
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                        Windows.Storage.ApplicationData.Current.LocalFolder;
                if (storageFolder == null)
                {
                    return false;
                }
                Windows.Storage.StorageFile userFile =
                    await storageFolder.CreateFileAsync(fileName,
                        Windows.Storage.CreationCollisionOption.ReplaceExisting);
                if (userFile == null)
                {
                    return false;
                }
            }

            catch
            {
                return false;
            }

            return true;
        }

        //in the sentence allreay see who send the message
        public static async Task<bool> WriteConversation(string sentence)
        {
            try
            {
                 Windows.Storage.StorageFolder storageFolder =
                       Windows.Storage.ApplicationData.Current.LocalFolder;
                 if (storageFolder == null)
                 {
                     return false;
                 }
                 Windows.Storage.StorageFile userFile =
                     await storageFolder.GetFileAsync("chatWithChild.txt");
                 if (userFile == null)
                 {
                     return false;
                 }
                 IEnumerable<string> toAdd = new List<string>() { sentence };
                 await Windows.Storage.FileIO.AppendLinesAsync(userFile, toAdd);

            }
            //if cant appand to file try to create the file
            catch
            {
                try
                {
                    Windows.Storage.StorageFolder storageFolder =
                            Windows.Storage.ApplicationData.Current.LocalFolder;
                    Windows.Storage.StorageFile userFile =
                        await storageFolder.CreateFileAsync("chatWithChild.txt",
                            Windows.Storage.CreationCollisionOption.ReplaceExisting);
                    IEnumerable<string> toAdd = new List<string>() { sentence };
                    await Windows.Storage.FileIO.AppendLinesAsync(userFile, toAdd);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static async Task<string> ReadConversation(string filename)
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile userFile =
                    await storageFolder.GetFileAsync(filename);

                string text = await Windows.Storage.FileIO.ReadTextAsync(userFile);

                return text;    
            }

            catch
            {
                return "";
            }
        }

        public static async Task<symbol[]> GetUserCounterAsync(string symbolName)
        {
            string child_email = myChild;
            symbol[] res = null;
            ConnectDB db = new ConnectDB();
            
            if (!who_am_i.Equals("") && !child_email.Equals(""))
            {
                symbol symbol = new symbol
                {
                    email = child_email,
                    symbolName = symbolName,
                    date = DateTime.Today
                };
                if (symbolName.Equals("allsymbols"))
                {
                    res = await db.GetUserAllCountersfromDBAsync(child_email);
                }
                else {
                    res = await db.GetUserCounterformDBAsync(symbol);
                }
            }
            return res;
        }

        public static async Task<string> GetParentContactAsync()
        {
            string email = who_am_i;
            string res = "";
            ConnectDB db = new ConnectDB();
            
            if (!email.Equals(""))
            {
                res = await db.GetParentContactAsync(email);
                if(res == null)
                {
                    //The child don't have this parent as his contact
                    res = "";
                }
            }
            else
            {
                //Parent is not connected
                res = "";
            }
            return res;
        }

        public static async Task<OutTable[]> GetMsgAsync(string contact)
        {
            string completeUri = "https://function-queue-connect.azurewebsites.net/api/HttpGET-outTable-CSharp1?code=smvhBz/DBsmNUDqf7/TIhjZ1IMBSo77LwpSbhG2I9CsGCw1D6sNLkg==&&name="
                + who_am_i + "$" + contact ;

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
                OutTable[] msg = JsonConvert.DeserializeObject<OutTable[]>(httpResponseBody);
                return msg;
            }

            catch (Exception ex)
            {
                return null;
            }           
        }

        public static async void sendMsg(string message, string addressee)
        {
            String final_msg = message + "$" + Common.who_am_i + "$";
            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();
            Uri requestUri = new Uri("https://function-queue-connect.azurewebsites.net/api/HttpTriggerCSharp1-send?code=c4TP96qDiVU6X5Zd6HNmAOCOIp35R52MB0MZnL6GRjY8ldfF2GqZ3A==&&name=" + final_msg + addressee);
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
            }
        }

       public static async Task<bool> markAsDeleteMsg(OutTable obj)
        {
            if(obj ==null)
            {
                return false;
            }
            string completeUri = "https://function-queue-connect.azurewebsites.net/api/HttpPUT-CRUD-CSharp2?code=E3cZTihW7ZvJjfPgbWITpZbKApX8OHKj4BNwwKz3Sjur5HhRk3zrkA==";
            outTableChange new_msg = new outTableChange() ;
            new_msg.Message_Recive = "0";
            new_msg.Message_Send = "0";
            new_msg.RowKey = obj.RowKey;
            new_msg.PartitionKey = obj.PartitionKey;
            //string json = ConnectDB.WriteFromObject(obj);
            string json = JsonConvert.SerializeObject(new_msg);

            try
            {
                //Send the PUT request
                Windows.Web.Http.HttpStringContent stringContent = new Windows.Web.Http.HttpStringContent(json.ToString());
                System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod("PUT"), completeUri);
                request.Content = new StringContent(json,
                Encoding.UTF8,
                "application/json");//CONTENT-TYPE header
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage response = await client.SendAsync(request);  //I know I should have used async/await here!
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
