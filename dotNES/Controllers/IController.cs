using System.Windows.Forms;

namespace dotNES.Controllers
{
    interface IController
    {
        void SetKey(NES001Controller.ControllerKeys key, Keys bind);

        void Strobe(bool on);

        int ReadState();

        void PressKey(KeyEventArgs e);

        void ReleaseKey(KeyEventArgs e);
    }
}
