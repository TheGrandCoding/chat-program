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

        public static Random RND = new Random(DateTime.Now.Millisecond);

        static uint _userIdNoTouchy = 0;
        public static uint USER_ID 
        {  
            get
            {
                lock(IDLOCK)
                {
                    return _userIdNoTouchy;
                }
            } set
            {
                lock(IDLOCK)
                {
                    _userIdNoTouchy = value;
                }
            }
        }

        static object IDLOCK = new object();
        static uint _messageIdNoTouchy = 0;
        public static uint MESSAGE_ID {  get
            {
                lock(IDLOCK)
                {
                    return _messageIdNoTouchy;
                }
            } set
            {
                lock(IDLOCK)
                {
                    _messageIdNoTouchy = value;
                }
            }
        }

        public static uint IterateMessageId()
        {
            lock(IDLOCK)
            {
                _messageIdNoTouchy += 1;
                return _messageIdNoTouchy - 1;
            }
        }

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

        public static User GetUser(string name)
        {
            lock(Users)
            {
                return Users.Values.LastOrDefault(x => x.Name == name);
            }
            return null;
        }


    }
}
