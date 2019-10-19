using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server.Commands
{
    [Name("API Module")]
    public class APIModule : CommandBase
    {
        [Name("token")]
        [RequireServer]
        public void SetToken(string token)
        {
            Program.SetRegistry("apiKey", token);
            Reply($"Token set to '{token}', restart server to take affect", System.Drawing.Color.Cyan);
        }
    }
}
