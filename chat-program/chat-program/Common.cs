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

        public static Dictionary<uint, Message> Messages = new Dictionary<uint, Message>();

        public static Dictionary<uint, Image> Images = new Dictionary<uint, Image>();

        static uint _imageIdNoTouchy = 20;

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
        static uint _messageIdNoTouchy = 1;
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

        public static uint IterateImageId()
        {
            lock(IDLOCK)
            {
                _imageIdNoTouchy += 1;
                return _imageIdNoTouchy - 1;
            }
        }

        public static bool TryGetImage(uint id, out Image image)
        {
            lock (IDLOCK)
            {
                return Images.TryGetValue(id, out image);
            }
        }

        public static void AddImage(Image image)
        {
            lock(IDLOCK)
            {
                Images[image.Id] = image;
            }
        }
        static User getUserWhenNotCached(uint id)
        {
            if(!Program.IsServer) // server should populate above list, so should always be able to get it
            {
            }


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
                return Users.Values.LastOrDefault(x => x.UserName == name);
            }
        }

        public static void AddMessage(Message msg)
        {
            lock(IDLOCK)
            {
                Messages[msg.Id] = msg;
            }
        }

        public static bool TryGetMessage(uint id, out Message msg)
        {
            lock(IDLOCK)
            {
                return Messages.TryGetValue(id, out msg);
            }
        }
    }
}
