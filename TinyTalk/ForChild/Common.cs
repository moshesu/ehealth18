using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;

namespace ForChild
{
    static class Common
    {
        public static string who_am_i = "";
        public static string myFather = "";
        public static string myMother = "";
        public static string myFriend = "";
        public static string myTeacher = "";
        public static int my_num_of_msg = 0;
        public static bool isConectet = false;
        public static bool iStarted = true;



        // Input: message and addresse
        // Output: send the message to azure storage Queue 
        public static async void sendMsg(string message,string sendtoAddress)
        {
            String final_msg = message + "$" + Common.who_am_i.ToLower() + "$";
            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();
            Uri requestUri = new Uri("https://function-queue-connect.azurewebsites.net/api/HttpTriggerCSharp1-send?code=c4TP96qDiVU6X5Zd6HNmAOCOIp35R52MB0MZnL6GRjY8ldfF2GqZ3A==&&name=" + final_msg + sendtoAddress);

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

        public static void UpdateCounterAsync(string symbolName)
        {
            ConnectDB db = new ConnectDB();
            if (!(who_am_i.Equals("") || symbolName.Equals("")))
            {
                symbol symbol = new symbol
                {
                    email = who_am_i.ToLower(),
                    symbolName = symbolName,
                    date = DateTime.Today
                };
                db.UpdateUserCounterAsync(symbol);
            }
        }
        
        public static async Task GetUserContactsAsync()
        {
            string email = who_am_i.ToLower();
            ConnectDB db = new ConnectDB();

            if (!who_am_i.Equals(""))
            {
                userContacts contacts = await db.GetUserContactsAsync(email);
                if (contacts != null)
                {
                    myFather = contacts.father.ToLower();
                    myMother = contacts.mother.ToLower();
                    myFriend = contacts.friend.ToLower();
                    myTeacher = await db.GetGardenTeacher(who_am_i,"Teacher");
                }
                else
                {
                    Frame toAddUsersForChat = Window.Current.Content as Frame;
                    toAddUsersForChat.Navigate(typeof(AddUsersForChat));
                }
            }
        }

        //updation user contacts update all the for contacts
        public static async Task<string> AddUserChatContact(string[] emails)
        {
            string res = "success";
            if (who_am_i.Equals(""))
            {
                res = "You need to login before";
                return res;
            }
            user[] users = new user[3];
            ConnectDB db = new ConnectDB();
            for (int i = 0; i < users.Length; i++)
            {
                users[i] = await db.GetUserByMailAsync(emails[i + 2]);
                if (users[i] == null)
                {
                    res = "The user " + emails[i + 2] + " does not exist";
                    return res;
                }
            }
            if (users[0].type.Equals("Parent") && users[1].type.Equals("Parent")
                 && users[2].type.Equals("Child"))
            {
                bool x = await db.AddUserContactsAsync(emails);
                if (x == true)
                {
                    myFather = users[0].email;
                    myMother = users[1].email;
                    myFriend = users[2].email;
                    myTeacher = await db.GetGardenTeacher(who_am_i, "Teacher");
                    return res;
                }
                res = "Cant add the users to db, try again later";
                return res;
            }
            else
            {
                res = "The users you trying to add are not from the proper type";
                return res;
            }
        }
        
        public static async Task<OutTable[]> GetMsgAsync(string contact)
        {
            string completeUri = "https://function-queue-connect.azurewebsites.net/api/HttpGET-outTable-CSharp1?code=smvhBz/DBsmNUDqf7/TIhjZ1IMBSo77LwpSbhG2I9CsGCw1D6sNLkg==&&name="
                + who_am_i + "$" + contact;

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

        public static async Task<bool> CheckForSimilarFriendAsync()
        {
            ConnectDB db = new ConnectDB();

            if (!(who_am_i.Equals("") || myFriend.Equals("")))
            {
                userContacts contacts = await db.GetUserContactsAsync(myFriend);
                if (contacts != null && contacts.friend != null)
                {
                    if (contacts.friend.ToLower().Equals(who_am_i.ToLower()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static async Task<bool> GetUserFromFileAsync()
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
                    await storageFolder.GetFileAsync("userChild.txt");
                if (userFile == null)
                {
                    return false;
                }
                string text = await Windows.Storage.FileIO.ReadTextAsync(userFile);
                if (String.IsNullOrEmpty(text) || String.IsNullOrWhiteSpace(text) || text.Equals(""))
                {
                    return false;
                }
                else
                {
                    string[] fileResult = text.Split('+');
                    foreach (string per in fileResult)
                    {
                        string[] per_arr = per.Split(':');
                        switch (per_arr[0])
                        {
                            case "email":
                                who_am_i = per_arr[1];
                                break;
                            case "father":
                                myFather = per_arr[1];
                                break;
                            case "mother":
                                myMother = per_arr[1];
                                break;
                            case "teacher":
                                myTeacher = per_arr[1];
                                break;
                            case "friend":
                                myFriend = per_arr[1];
                                break;
                        }
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
        
        //in the sentence allreay see who send the message
        public static async Task<bool> WriteConversation(string sentence, string FileName)
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
                    await storageFolder.GetFileAsync(FileName);
                if (userFile == null)
                {
                    return false;
                }
                IEnumerable<string> toAdd = new List<string>() { sentence };
                await Windows.Storage.FileIO.AppendLinesAsync(userFile, toAdd);

            }
            //trying to create the file
            catch
            {
                try
                {
                    Windows.Storage.StorageFolder storageFolder =
                            Windows.Storage.ApplicationData.Current.LocalFolder;
                    Windows.Storage.StorageFile userFile =
                        await storageFolder.CreateFileAsync(FileName,
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
    }
}

