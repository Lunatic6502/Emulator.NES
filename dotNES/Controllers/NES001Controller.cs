using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace dotNES.Controllers
{
    class NES001Controller : IController
    {
        private int data;
        private int serialData;
        private bool strobing;

        public bool debug;
        // bit:   	 7     6     5     4     3     2     1     0
        // button:	 A B  Select Start  Up Down  Left 

        public enum ControllerKeys
        {
            A,
            B,
            SELECT,
            START,
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        private Dictionary<Keys, int> _keyMapping = new Dictionary<Keys, int>
        {
            {Keys.A, 7},
            {Keys.S, 6},
            {Keys.RShiftKey, 5},
            {Keys.Enter, 4},
            {Keys.Up, 3},
            {Keys.Down, 2},
            {Keys.Left, 1},
            {Keys.Right, 0},
        };

        private List<Keys> KeyBinds = new List<Keys>();

        public void SetKey(ControllerKeys key, Keys bind)
        {
            KeyBinds.Clear();

            foreach (Keys k in _keyMapping.Keys)
            {
                KeyBinds.Add(k);
            }

            switch (key)
            {
                case ControllerKeys.A:
                    Dictionary<Keys, int> save = new Dictionary<Keys, int>();
                    save = _keyMapping;
                    _keyMapping.Clear();
                    _keyMapping.Add(bind, 7);
                    _keyMapping.Add(KeyBinds[1], 6);
                    _keyMapping.Add(KeyBinds[2], 5);
                    _keyMapping.Add(KeyBinds[3], 4);
                    _keyMapping.Add(KeyBinds[4], 3);
                    _keyMapping.Add(KeyBinds[5], 2);
                    _keyMapping.Add(KeyBinds[6], 1);
                    _keyMapping.Add(KeyBinds[7], 0);
                    break;

                case ControllerKeys.B:
                    Dictionary<Keys, int> save2 = new Dictionary<Keys, int>();
                    save2 = _keyMapping;
                    _keyMapping.Clear();
                    _keyMapping.Add(KeyBinds[0], 7);
                    _keyMapping.Add(bind, 6);
                    _keyMapping.Add(KeyBinds[2], 5);
                    _keyMapping.Add(KeyBinds[3], 4);
                    _keyMapping.Add(KeyBinds[4], 3);
                    _keyMapping.Add(KeyBinds[5], 2);
                    _keyMapping.Add(KeyBinds[6], 1);
                    _keyMapping.Add(KeyBinds[7], 0);
                    break;

                case ControllerKeys.SELECT:
                    Dictionary<Keys, int> save3 = new Dictionary<Keys, int>();
                    save3 = _keyMapping;
                    _keyMapping.Clear();
                    _keyMapping.Add(KeyBinds[0], 7);
                    _keyMapping.Add(KeyBinds[1], 6);
                    _keyMapping.Add(bind, 5);
                    _keyMapping.Add(KeyBinds[3], 4);
                    _keyMapping.Add(KeyBinds[4], 3);
                    _keyMapping.Add(KeyBinds[5], 2);
                    _keyMapping.Add(KeyBinds[6], 1);
                    _keyMapping.Add(KeyBinds[7], 0);
                    break;

                case ControllerKeys.START:
                    Dictionary<Keys, int> save4 = new Dictionary<Keys, int>();
                    save4 = _keyMapping;
                    _keyMapping.Clear();
                    _keyMapping.Add(KeyBinds[0], 7);
                    _keyMapping.Add(KeyBinds[1], 6);
                    _keyMapping.Add(KeyBinds[2], 5);
                    _keyMapping.Add(bind, 4);
                    _keyMapping.Add(KeyBinds[4], 3);
                    _keyMapping.Add(KeyBinds[5], 2);
                    _keyMapping.Add(KeyBinds[6], 1);
                    _keyMapping.Add(KeyBinds[7], 0);
                    break;

                case ControllerKeys.UP:
                    Dictionary<Keys, int> save5 = new Dictionary<Keys, int>();
                    save5 = _keyMapping;
                    _keyMapping.Clear();
                    _keyMapping.Add(KeyBinds[0], 7);
                    _keyMapping.Add(KeyBinds[1], 6);
                    _keyMapping.Add(KeyBinds[2], 5);
                    _keyMapping.Add(KeyBinds[3], 4);
                    _keyMapping.Add(bind, 3);
                    _keyMapping.Add(KeyBinds[5], 2);
                    _keyMapping.Add(KeyBinds[6], 1);
                    _keyMapping.Add(KeyBinds[7], 0);
                    break;

                case ControllerKeys.DOWN:
                    Dictionary<Keys, int> save6 = new Dictionary<Keys, int>();
                    save6 = _keyMapping;
                    _keyMapping.Clear();
                    _keyMapping.Add(KeyBinds[0], 7);
                    _keyMapping.Add(KeyBinds[1], 6);
                    _keyMapping.Add(KeyBinds[2], 5);
                    _keyMapping.Add(KeyBinds[3], 4);
                    _keyMapping.Add(KeyBinds[4], 3);
                    _keyMapping.Add(bind, 2);
                    _keyMapping.Add(KeyBinds[6], 1);
                    _keyMapping.Add(KeyBinds[7], 0);
                    break;

                case ControllerKeys.LEFT:
                    Dictionary<Keys, int> save7 = new Dictionary<Keys, int>();
                    save7 = _keyMapping;
                    _keyMapping.Clear();
                    _keyMapping.Add(KeyBinds[0], 7);
                    _keyMapping.Add(KeyBinds[1], 6);
                    _keyMapping.Add(KeyBinds[2], 5);
                    _keyMapping.Add(KeyBinds[3], 4);
                    _keyMapping.Add(KeyBinds[4], 3);
                    _keyMapping.Add(KeyBinds[5], 2);
                    _keyMapping.Add(bind, 1);
                    _keyMapping.Add(KeyBinds[7], 0);
                    break;

                case ControllerKeys.RIGHT:
                    Dictionary<Keys, int> save8 = new Dictionary<Keys, int>();
                    save8 = _keyMapping;
                    _keyMapping.Clear();
                    _keyMapping.Add(KeyBinds[0], 7);
                    _keyMapping.Add(KeyBinds[1], 6);
                    _keyMapping.Add(KeyBinds[2], 5);
                    _keyMapping.Add(KeyBinds[3], 4);
                    _keyMapping.Add(KeyBinds[4], 3);
                    _keyMapping.Add(KeyBinds[5], 2);
                    _keyMapping.Add(KeyBinds[6], 1);
                    _keyMapping.Add(bind, 0);
                    break;
                default:
                    break;
            }
        }

        public void Strobe(bool on)
        {
            serialData = data;
            strobing = on;
        }

        public int ReadState()
        {
            int ret = ((serialData & 0x80) > 0).AsByte();
            if (!strobing)
            {
                serialData <<= 1;
                serialData &= 0xFF;
            }
            return ret;
        }

        public void PressKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P) debug ^= true;
            if (!_keyMapping.ContainsKey(e.KeyCode)) return;
            data |= 1 << _keyMapping[e.KeyCode];
        }

        public void ReleaseKey(KeyEventArgs e)
        {
            if (!_keyMapping.ContainsKey(e.KeyCode)) return;
            data &= ~(1 << _keyMapping[e.KeyCode]);
        }
    }
}
