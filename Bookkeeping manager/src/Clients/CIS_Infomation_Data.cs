using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{
    public class CIS_Infomation_Data : ClientData
    {
        private readonly string parentName;
        private TextBox withheld_TB, suffered_TB;
        private Style normal, readonly_;

        public void Initalize(TextBox withheld, TextBox suffered, Style norm, Style ro)
        {
            withheld_TB = withheld;
            suffered_TB = suffered;
            normal = norm;
            readonly_ = ro;

            WithheldEnabled = !WithheldEnabled;
            WithheldEnabled = !WithheldEnabled;

            SufferedEnabled = !SufferedEnabled;
            SufferedEnabled= !SufferedEnabled;
        }

        public CIS_Infomation_Data(string name) : base(name)
        {
        }
        private bool withheldEnabled;
        public bool WithheldEnabled
        {
            get => withheldEnabled;
            set
            {
                if(withheldEnabled != value)
                {
                    withheldEnabled = value;
                    if (value)
                    {
                        withheld_TB.Style = normal;
                    }
                    else
                    {
                        withheld_TB.Style = readonly_;
                    }
                }
            }
        }
        public string CIS_Withheld { get; set; }
        private bool sufferedEnabled;
        public bool SufferedEnabled
        {
            get => sufferedEnabled;
            set
            {
                if (sufferedEnabled != value)
                {
                    sufferedEnabled = value;
                    if (value)
                    {
                        suffered_TB.Style = normal;
                    }
                    else
                    {
                        suffered_TB.Style = readonly_;
                    }
                }
            }
        }
        public string CIS_Suffered { get; set; }
    }
}
