using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edufun_2
{
    class Class_Instructor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string School { get; set; }
        public string Subject { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Day { get; set; }
        public int Time { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int Student_count { get; set; }
        public string Department1 { get; set; }
        public string Department2 { get; set; }

        private static List<Class_Instructor> instance;

        public static List<Class_Instructor> GetInstance()
        {
            if (instance == null)
                instance = new List<Class_Instructor>();

            return instance;
        }
    }
}
