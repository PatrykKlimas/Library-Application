using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClassLibrary
{
    public class User
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginStr { get; set; }
        public string spassword { get; set; }
        public string Email { get; set; }
        public bool bAdmin { get; set; }
        public string Display
        {
            get
            {
                string output;
                output = FirstName + ", " + LastName + ", " + LoginStr;
                return output;
            }
        }
    }
}
