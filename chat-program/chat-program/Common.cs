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

        public static uint USER_ID = 0;
        public static uint MESSAGE_ID = 0;

        static User getUserWhenNotCached(uint id)
        {
            if(!Program.IsServer) // server should populate above list, so should always be able to get it
            {
            }
            return null;
        }

        public static User GetUser(uint id)
        {
            lock(Users)
            {
                if (Users.TryGetValue(id, out var r))
                    return r;
            }
            return getUserWhenNotCached(id);
        }


    }
}
