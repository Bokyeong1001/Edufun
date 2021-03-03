using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edufun_2
{
    class Instructor
    {
        public int idx { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Address { get; set; }
        public string Department1 { get; set; }
        public string Department2 { get; set; }
        public string Ship_Address1 { get; set; }
        public string Ship_Address2 { get; set; }
        public string Ship_Method1 { get; set; }
        public string Ship_Method2 { get; set; }

        private static List<Instructor> instance;

        public static List<Instructor> GetInstance()
        {
            if (instance == null)
                instance = new List<Instructor>();

            return instance;
        }
    }
}
