using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassApiRates : ClassNotify
    {
        private long _timestamp;
        private Dictionary<string, double> _Rates;
        private string _newTimestamp;

        public ClassApiRates()
        {
            timestamp = 0;
            Rates = new Dictionary<string, double>();
            newTimestamp = "";
        }

        public string newTimestamp
        {
            get { return _newTimestamp; }
            set
            {
                if (_newTimestamp != value)
                {
                    _newTimestamp = value;
                }
                Notify("newTimestamp");
            }
        }


        public Dictionary<string, double> Rates
        {
            get { return _Rates; }
            set
            {
                if (_Rates != value)
                {
                    _Rates = value;
                }
                Notify("Rates");
            }
        }


        public long timestamp
        {
            get { return _timestamp; }
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                }
                Notify("timestamp");
            }
        }

    }
}
