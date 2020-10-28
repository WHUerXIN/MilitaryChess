using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WarChess
{
   
    /// <summary>
    /// ������ս�ӿ�
    /// </summary>
    public interface ISocket
    {
        void listen();
        void connect(string ipStr);
        void sendMsg(byte @class, byte flag, string content);
        void receiveMsg(object obj);
        void close();
    }

   
    /// <summary>
    /// ������ս�¼�
    /// </summary>
    public class ChessEvent : EventArgs
    {
        public string Iclass;
        public string content;
        public string flag;
        public ChessEvent(string _class, string _flag, string _content)
        {
            Iclass = _class;
            content = _content;
            flag = _flag;
        }
    }

    /// <summary>
    /// ί��
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ChessEventHander(object sender, ChessEvent e);
}
