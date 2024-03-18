using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitualProject
{
    interface ICloseWindow
    {
        Action Close { get; set; }
        bool CanClose();
    }
}
