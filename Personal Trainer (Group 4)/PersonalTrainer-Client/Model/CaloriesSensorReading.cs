using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PersonalTrainer_Client
{
    [DataContract]
    public class CaloriesSensorReading : ViewModel
    {

        long _calories;

        [DataMember]
        public long Calories
        {
            get { return _calories; }
            set
            {
                SetValue(ref _calories, value, "Calories");
            }
        }

    }
}
