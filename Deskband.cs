using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Muzyka
{
    [ComVisible(true)]
    [Guid("FB17B6DA-E3D7-4D17-9E43-3416983372A9")]
    [CSDeskBand.CSDeskBandRegistration(Name = "Muzyka", ShowDeskBand = false)]
    public class Deskband : CSDeskBand.CSDeskBandWin
    {
        private static Control _control;

        public Deskband()
        {
            Options.MinHorizontalSize = new Size(150, 30);
            _control = new UserControl1(this);
        }

        protected override Control Control => _control;
    }
}
