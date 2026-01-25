using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    internal class User
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { if (value > -1) id = value; else throw new Exception("ID ist negativ!"); }
        }
        public string Name {  get; set; }
        public string Password { get; set; }
        public User() { }
    }
}
