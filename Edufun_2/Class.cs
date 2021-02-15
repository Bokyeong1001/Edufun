using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edufun_2
{
    class Class
    {
        public int ID { get; set; }
        public int Instructor_ID { get; set; }
        public string School { get; set; }
        public string Day { get; set; }
        public int Time { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int Student_count { get; set; }
        public int Sum { get; set; }

        private static List<Class> instance;

        public static List<Class> GetInstance()
        {
            if (instance == null)
                instance = new List<Class>();

            return instance;
        }
    }
}
