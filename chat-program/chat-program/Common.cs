using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram
{
    public static class Common
    {
        public static Dictionary<uint, User> Users = new Dictionary<uint, User>();

        public static User GetUser(uint id)
        {
            lock(Users)
            {
                if (Users.TryGetValue(id, out var r))
                    return r;
                return null;
            }
        }


    }
}
