using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClassLibrary
{
    public class Book
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int Type { get; set; }
        public int Section { get; set; }
        public string sDescription { get; set; }
        public int UserID { get; set; } = 0;
        public string ISBN { get; set; }

        public string Display
        {
            get
            {
                string output;
                output = Title.Length > 25 ? Title.PadLeft(25) + "... " : Title + " ";
                output = output + Author.PadLeft(1);
                return output;
            }
        }
    }
}
