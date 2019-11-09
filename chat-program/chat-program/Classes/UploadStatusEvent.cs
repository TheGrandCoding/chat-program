using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Classes
{
    public class UploadStatusEvent : EventArgs
    {
        public int Current { get; }
        public int Maximum { get; }
        public int Remaining => Maximum - Current;
        public UploadStatusEvent(Image e, int slice)
        {
            Current = slice;
            Maximum = e.MaximumSlices;
        }
    }
}
