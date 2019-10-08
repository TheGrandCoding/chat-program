using DesktopNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Client
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("2799fe47-da36-4c3e-8812-4d2c539f7138"), ComVisible(true)]
    public class ClientActivator : NotificationActivator
    {
        public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
        {
            var frm = Menu.Client;
            if (frm != null)
                frm.Show();
        }
    }
}
