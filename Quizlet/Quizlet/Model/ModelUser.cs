using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    internal class ModelUser
    {
        private List<User> users = new List<User>();
        public void UserEinlesen()
        {

        }
        public int CheckUser(string username, string pwd)
        {
            foreach (var userr in users)
            {
                if (username.Equals(userr.Name) && pwd.Equals(userr.Password))
                {
                    return userr.ID;
                }
            }
            return -1;
        }
    }
}
