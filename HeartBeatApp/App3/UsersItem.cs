
using System.Text;
using System.Runtime.Serialization;
//using Microsoft.Azure;

using Android.App;
using Java.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace HBApp
{
    [Activity(Name = "user.item")]
    /**
     * Represents an item in a ToDo list
     */
    public class Users
    {

        /**
         * User first name
         */
        //[DataMember(Name(Name = "first_name")]
        [JsonProperty(PropertyName = "first_name")]
        private string first_name { get; set; }

        /**
         * User last name
         */
        //[DataMember(Name(Name = "last_name")]
        [JsonProperty(PropertyName = "last_name")]
        private string last_name { get; set; }

        /**
         * User sex: female/male
         */
        //[DataMember(Name(Name = "sex")]
        [JsonProperty(PropertyName = "sex")]
        private string sex { get; set; }

        /**
         * User DOB [dd/mm/yyyy]
         */
        //[DataMember(Name(Name = "dob")]
        [JsonProperty(PropertyName = "dob")]
        private int dob { get; set; }

        /**
         * User height [cm]
         */
        //[DataMember(Name(Name = "height")]
        [JsonProperty(PropertyName = "height")]
        private int height { get; set; }

        /**
         * User weight [kg]
         */
        //[DataMember(Name(Name = "weight")]
        [JsonProperty(PropertyName = "weight")]
        private int weight { get; set; }

        /**
         * User health condition [healthy, diabetic, heart etc.]
         */
        //[DataMember(Name(Name = "health_cond")]
        [JsonProperty(PropertyName = "health_cond")]
        private string health_c { get; set; }

        /**
         * User minimum hr recommendation
         */
        //[DataMember(Name(Name = "min_hr_range")]
        [JsonProperty(PropertyName = "min_hr_range")]
        private int min_hr { get; set; }

        /**
         * User maximum hr recommendation
         */
        //[DataMember(Name(Name = "max_hr_range")]
        [JsonProperty(PropertyName = "max_hr_range")]
        private int max_hr { get; set; }

        /**
         * User low margine
         */
        //[DataMember(Name(Name = "lower_margine")]
        [JsonProperty(PropertyName = "lower_margine")]
        private int low_m { get; set; }

        /**
         * User high margine
         */
        //[DataMember(Name(Name = "upper_margine")]
        [JsonProperty(PropertyName = "upper_margine")]
        private int up_m { get; set; }

        /**
         * User UUID
         */
        //[DataMember(Name(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        private string id { get; set; }


        /**
         * User UUID
         */
        //[DataMember(Name(Name = "password")]
        [JsonProperty(PropertyName = "password")]
        private string password { get; set; }

        /**
         * User UUID
         */
        //[DataMember(Name(Name = "username")]
        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }


        /**
         * User UUID
         */
        //[DataMember(Name(Name = "doctorname")]
        [JsonProperty(PropertyName = "doctorname")]
        private string doctorname { get; set; }

        /**
         * ToDoItem constructor
         */


        /*
         * @param first_name
         * @param last_name
         * @param sex
         * @param dob
         * @param height
         * @param weight
         * @param health_c
         * @param min_hr
         * @param max_hr
         * @param password
         * @param username
         */
        public Users(string first_name,
                         string last_name,
                         string sex,
                         int dob,
                         int height,
                         int weight,
                         string health_c,
                         int min_hr,
                         int max_hr,
                         string password,
                         string username)
        {
            this.password = password;
            this.username = username;
            this.setfirst_name(first_name);
            this.setlast_name(last_name);
            this.setgender(sex);
            this.setdob(dob);
            this.setheight(height);
            this.setweight(weight);
            this.sethealth_c(health_c);
            this.setmin_hr(min_hr);
            this.setmax_hr(max_hr);
            this.setid();
        }
        /*
        @override
        public string tostring()
        {
            return new stringbuilder().append("\nuseritem:\n")
                    .append(string.format("\tid         : %s\n", id.tostring()))
                    .append(string.format("\tusername   : %s\n", username))
                    .append(string.format("\tpassword   : %s\n", password))
                    .append(string.format("\tfirst_name : %s\n", first_name))
                    .append(string.format("\tlast_name  : %s\n", last_name))
                    .append(string.format("\tsex        : %s\n", sex))
                    .append(string.format("\tdob        : %s\n", dob))
                    .append(string.format("\theight     : %d\n", height))
                    .append(string.format("\tweight     : %d\n", weight))
                    .append(string.format("\thealth_c   : %s\n", health_c))
                    .append(string.format("\tmin_hr     : %d\n", min_hr))
                    .append(string.format("\tmax_hr     : %d\n\n", max_hr))
                    .tostring();
        }
        */

        /*
        * @param id
        */
        public void setid()
        {
            //this.id = this.getusername() != "" ? UUID.NameUUIDFromBytes(Encoding.ASCII.GetBytes(getusername().ToString())) : UUID.RandomUUID();
            id =  UUID.NameUUIDFromBytes(Encoding.ASCII.GetBytes(getusername().ToString())).ToString();
        }

        /**
         * @param first_name
         */
        public void setfirst_name(string first_name)
        {
            this.first_name = first_name;
        }

        /**
         *
         * @param last_name
         */
        public void setlast_name(string last_name)
        {
            this.last_name = last_name;
        }

        /**
         *
         * @param sex
         */
        public void setgender(string sex)
        {
            this.sex = sex;
        }

        /**
         *
         * @param dob
         */
        public void setdob(int dob)
        {
            this.dob = dob;
        }

        /**
         *
         * @param height
         */
        public void setheight(int height)
        {
            this.height = height;
        }

        /**
         *
         * @param weight
         */
        public void setweight(int weight)
        {
            this.weight = weight;
        }

        /**
         *
         * @param health_c
         */
        public void sethealth_c(string health_c)
        {
            this.health_c = health_c;
        }

        /**
         *
         * @param min_hr
         */
        public void setmin_hr(int min_hr)
        {
            this.min_hr = min_hr;
        }

        /**
         *
         * @param max_hr
         */
        public void setmax_hr(int max_hr)
        {
            this.max_hr = max_hr;
        }

        /**
         *
         * @param low_m
         */
        public void setlow_m(int low_m)
        {
            this.low_m = low_m;
        }

        /**
         *
         * @param up_m
         */
        public void setup_m(int up_m)
        {
            this.up_m = up_m;
        }

        public void setdoctorname(string doctorname)
        {
            this.doctorname = doctorname;
        }

        /**
         *
         * @param id
         */


        public void setusername(string username)
        {
            this.username = username;
        }

        public void setpassword(string password)
        {
            this.password = password;
        }
        /**
         *
         * @return first_name
         */
        public string getfirst_name()
        {
            return first_name;
        }

        /**
         *
         * @return last_name
         */
        public string getlast_name()
        {
            return last_name;
        }

        /**
         *
         * @return sex
         */
        public string getsex()
        {
            return sex;
        }

        /**
         *
         * @return dob
         */
        public int getdob()
        {
            return dob;
        }

        /**
         *
         * @return height
         */
        public int getheight()
        {
            return height;
        }

        /**
         *
         * @return weight
         */
        public int getweight()
        {
            return weight;
        }

        /**
         *
         * @return health_c
         */
        public string gethealth_c()
        {
            return health_c;
        }

        /**
         *
         * @return min_hr
         */
        public int getmin_hr()
        {
            return min_hr;
        }

        /**
         *
         * @return max_hr
         */
        public int getmax_hr()
        {
            return max_hr;
        }

        /**
         *
         * @return low_m
         */
        public int getlow_m()
        {
            return low_m;
        }

        /**
         *
         * @return up_m
         */
        public int getup_m()
        {
            return up_m;
        }

        /**
         *
         * @return user id
         */
        public string getid()
        {
            return id;
        }



        public string getusername()
        {
            return username;
        }

        public string getpassword()
        {
            return password;
        }


        public string getdoctorname()
        {
            return doctorname;
        }

        public static implicit operator Users(JToken v)
        {
            throw new NotImplementedException();
        }

        /*
        @override
        public system.boolean equals(object o)
        {
            return o instanceof usersitem && ((usersitem)o).id == id;


        }
        */
    }
}