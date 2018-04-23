using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForTeacher
{
    class Common
    {
        public static string who_am_i = "";
        public static string garden = "";
        public static bool isConectet = false;
        public static user[] gardenChildren = null;
        public static int counter_child = 0;

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
                    await storageFolder.GetFileAsync("userTeacher.txt");
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
                    who_am_i = fileResult[0];
                    garden = fileResult[2];
                    isConectet = true;
                    return true;
                }
            }

            catch
            {
                return false;
            }
        }

        public static async Task<symbol[]> GetUserCounterAsync(string symbolName, string childEmail)
        {
            string child_email = childEmail;
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
                    res = await db.GetUserAllCountersAsync(child_email);
                }
                else
                {
                    res = await db.GetUserCounterAsync(symbol);
                }
            }
            return res;
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

        public static async Task GetGardenChildrenAsync()
        {
            ConnectDB db = new ConnectDB();
            var res = await db.GetGardenChildren(garden);
            if (res != null)
            {
                Common.gardenChildren = res;
                Common.counter_child = res.Length;
            }
        }
    }
}