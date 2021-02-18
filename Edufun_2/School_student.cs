using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edufun_2
{
    class School_student
    {
        public int ID { get; set; }
        public string School { get; set; }
        public string Day { get; set; }
        public int Year { get; set; }
        public int q1t1 { get; set; }
        public int q1t2 { get; set; }
        public int q1sum { get; set; }
        public int q2t1 { get; set; }
        public int q2t2 { get; set; }
        public int q2sum { get; set; }
        public int q3t1 { get; set; }
        public int q3t2 { get; set; }
        public int q3sum { get; set; }
        public int q4t1 { get; set; }
        public int q4t2 { get; set; }
        public int q4sum { get; set; }

        private static List<School_student> instance;
        public static List<School_student> GetInstance()
        {
            if (instance == null)
                instance = new List<School_student>();

            return instance;
        }
    }
}
