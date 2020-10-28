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
    public partial class Form1 : Form
    {
        /// <summary>
        /// bin路径，用于根据地址获取文件
        /// </summary>
        string path = Application.StartupPath;
        /// <summary>
        /// 初始化50个PictureBox
        /// </summary>
        private PictureBox[] ChessPicBox = new PictureBox[50];
        /// <summary>
        /// 棋子索引
        /// </summary>
        private int[] Chess;
        /// <summary>
        /// 逻辑棋盘
        /// </summary>
        private int[,] Map;
        /// <summary>
        /// 迷宫，用于工兵寻径
        /// </summary>
        private int[,] MIGONG = new int[5, 12];
        /// <summary>
        /// 创建一个伪随机数种子
        /// </summary>
        Random rd = new Random();
        /// <summary>
        /// 判断点击的是不是第一个棋子
        /// </summary>
        bool IsFirstChess = true;
        /// <summary>
        /// 设置部分，用于表征音效是否开启
        /// </summary>
        static public bool Music=true;
        /// <summary>
        /// 设置部分，用于表征提示框是否开启
        /// </summary>
        static public bool Message = true;
        /// <summary>
        /// 设置部分，用于表征聊天功能是否开启
        /// </summary>
        static public bool Wechat = true;
        /// <summary>
        /// Map棋盘上抽象信息
        /// </summary>
        int old_x, old_y, idx;
        /// <summary>
        /// 玩家棋子执方枚举
        /// </summary>
        enum PlayerColor { Red, Blue };
        /// <summary>
        /// 棋子轮流方，执行棋为红棋，红棋先行
        /// </summary>
        PlayerColor TurnColor = PlayerColor.Red;
        /// <summary>
        /// 玩家棋子执方
        /// </summary>
        PlayerColor GamerColor;
        /// <summary>
        /// 创建一个网络对战类
        /// </summary>
        private ControlInternet CI = new ControlInternet();
        /// <summary>
        /// 游戏模式：state=1双人本地模式；state=2人机对战模式；state=3网络红棋模式；state=4网络蓝棋模式；
        /// </summary>
        int state;

        /// <summary>
        /// 创建窗口Form1
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 创建窗口Form1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, System.EventArgs e)
        {
            pictureBoxMap.MouseDown += new System.Windows.Forms.MouseEventHandler(bt_MouseDownMap);
            pictureBoxMap.MouseMove += new System.Windows.Forms.MouseEventHandler(bt_MouseMoveMap);
            Chessindex();
            ChessInit();
            MapInit();   
        }
   
        /// <summary>
        /// 用于创建棋子索引index
        /// </summary>
        private void Chessindex()
        {
            Chess = new int[25];
            Chess[0] = 1; //军旗1
            Chess[1] = 2; Chess[2] = 2; Chess[3] = 2;//地雷2
            Chess[4] = 3; Chess[5] = 3;//炸弹3
            Chess[6] = 4; Chess[7] = 4; Chess[8] = 4;//工兵4
            Chess[9] = 5; Chess[10] = 5; Chess[11] = 5;//排长5
            Chess[12] = 6; Chess[13] = 6; Chess[14] = 6;//连长6
            Chess[15] = 7; Chess[16] = 7;//营长7
            Chess[17] = 8; Chess[18] = 8;//团长8
            Chess[19] = 9; Chess[20] = 9;//旅长9
            Chess[21] = 10; Chess[22] = 10;//师长10
            Chess[23] = 11;//军长11
            Chess[24] = 12;//司令12
        }
      
        /// <summary>
        /// 用于PictureBox初始化
        /// </summary>
        private void ChessInit()
        {
            string filename = "";
      
            int i;
            for (i = 0; i < 50; i++)//添加50个棋子
            {
                ChessPicBox[i] = new PictureBox();
                this.Controls.Add(ChessPicBox[i]);
                ChessPicBox[i].Width = 60;
                ChessPicBox[i].Height = 30;
                ChessPicBox[i].Tag = i.ToString();
                ChessPicBox[i].Parent = pictureBoxMap;
                if (i < 25)
                {
                    filename = path + "\\bmp\\R" + Chess[i].ToString() + ".bmp";
                    ChessPicBox[i].Name = "R" + i.ToString();
                }
                if (i >= 25 && i < 50)
                {
                    filename = path + "\\bmp\\B" + Chess[i % 25].ToString() + ".bmp";
                    ChessPicBox[i].Name = "B" + i.ToString();
                }
                ChessPicBox[i].Image = Image.FromFile(filename);
                ChessPicBox[i].Visible = false;
                ChessPicBox[i].MouseDown += new System.Windows.Forms.MouseEventHandler(bt_MouseDown);
                ChessPicBox[i].MouseMove += new System.Windows.Forms.MouseEventHandler(bt_MouseMove);
            }
        }

        /// <summary>
        /// 生成初始棋盘
        /// </summary>
        private void MapInit()
        {
            //初始化逻辑棋盘
            Map = new int[12, 5];
        }

        /// <summary>
        /// 显示棋盘横坐标与逻辑棋盘映射
        /// </summary>
        /// <param name="a">待转换位置</param>
        /// <returns></returns>
        private int MapConvertXandLeft(int a)
        {
            int b = 0;
            switch (a)
            {
                case 0: b = 61; break;
                case 1: b = 208; break;
                case 2: b = 356; break;
                case 3: b = 505; break;
                case 4: b = 651; break;
                case 61: b = 0; break;
                case 208: b = 1; break;
                case 356: b = 2; break;
                case 505: b = 3; break;
                case 651: b = 4; break;
            }
            return b;
        }
        /// <summary>
        /// 显示棋盘纵坐标与逻辑棋盘映射
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private int MapConvertYandTop(int a)
        {
            int b = 0;
            switch (a)
            {
                case 0: b = 63; break;
                case 1: b = 131; break;
                case 2: b = 193; break;
                case 3: b = 261; break;
                case 4: b = 329; break;
                case 5: b = 397; break;
                case 6: b = 598; break;
                case 7: b = 665; break;
                case 8: b = 733; break;
                case 9: b = 801; break;
                case 10: b = 863; break;
                case 11: b = 931; break;
                case 63: b = 0; break;
                case 131: b = 1; break;
                case 193: b = 2; break;
                case 261: b = 3; break;
                case 329: b = 4; break;
                case 397: b = 5; break;
                case 598: b = 6; break;
                case 665: b = 7; break;
                case 733: b = 8; break;
                case 801: b = 9; break;
                case 863: b = 10; break;
                case 931: b = 11; break;
            }
            return b;
        }
 
        /// <summary>
        /// 生成一个棋盘
        /// </summary>
        private void HaveNewMap()
        {
            int i;
            for (i = 0; i < 12; i++)
                for (int j = 0; j < 5; j++)
                    Map[i, j] = 999;  //999表示(i,j)处没放置棋子
            
            //ShowNewMap();
        }
      
        /// <summary>
        /// 随机安置棋子
        /// </summary>
        private void HaveChessOnNewMap()
        {
            //一种初始化:随机
            //随机安置军棋和地雷（0、1、2、3、25、26、27、28）
            {
                int flagRed = rd.Next(2);
                if (flagRed == 0)
                {
                    //军棋放置
                    Map[11, 1] = 0;
                    //地雷放置
                    Map[11, 0] = 1;
                    Map[11, 2] = 2;
                    Map[10, 1] = 3;
                }
                else
                {
                    Map[11, 3] = 0;
                    Map[11, 2] = 1;
                    Map[11, 4] = 2;
                    Map[10, 3] = 3;
                }
                int flagBlue = rd.Next(2);
                if (flagBlue == 0)
                {
                    Map[0, 1] = 25;
                    Map[0, 0] = 26;
                    Map[0, 2] = 27;
                    Map[1, 1] = 28;
                }
                else
                {
                    Map[0, 3] = 25;
                    Map[0, 2] = 26;
                    Map[0, 4] = 27;
                    Map[1, 3] = 28;
                }
            }
            //随机安置其他棋子（4-24，29-49）
            {
                int t;
                //随机安置蓝棋            
                for (int m = 0; m < 6; m++)
                    for (int n = 0; n < 5; n++)
                    {
                        if ((m == 2 && n == 1)
                            || (m == 2 && n == 3)
                            || (m == 3 && n == 2)
                            || (m == 4 && n == 1)
                            || (m == 4 && n == 3))
                        {
                            continue;
                        }
                        else if (Map[m, n] == 999)
                        {
                            t = GetFreeChessIdx(rd.Next(29, 49));
                            Map[m, n] = t;
                        }

                    }
                //随机安置红棋
                for (int m = 6; m < 12; m++)
                    for (int n = 0; n < 5; n++)
                    {
                        if ((m == 7 && n == 1)
                            || (m == 7 && n == 3)
                            || (m == 8 && n == 2)
                            || (m == 9 && n == 1)
                            || (m == 9 && n == 3))
                        {
                            continue;
                        }
                        else if (Map[m, n] == 999)
                        {
                            t = GetFreeChessIdx(rd.Next(4, 24));
                            Map[m, n] = t;
                        }
                    }
            }
        }

        /// <summary>
        /// 返回一个还未布局的棋子idx（用于随机安置棋子）
        /// </summary>
        /// <param name="a">返回的数据</param>
        /// <returns></returns>
        private int GetFreeChessIdx(int a)
        {
            bool flag=true;
            for (int m = 0; m < 12; m++)
                for (int n = 0; n < 5; n++)
                {
                    if (Map[m, n] == a)
                    {
                        flag = false;
                    }
                }
            if (flag)
            {
                return a;
            }
            else
            {
                if (a == 24)
                { return GetFreeChessIdx(4); }
                if (a == 49)
                { return GetFreeChessIdx(29); }
                else
                { return GetFreeChessIdx(a + 1); }
            }
        }
        
        /// <summary>
        /// 根据逻辑棋盘信息显示棋盘
        /// </summary>
        private void ShowNewMap()
        {
            for (int m = 0; m < 12; m++)
                for (int n = 0; n < 5; n++)
                {
                    if(Map[m, n]!=999)
                    {
                        ChessPicBox[Map[m, n]].Left = MapConvertXandLeft(n);
                        ChessPicBox[Map[m, n]].Top = MapConvertYandTop(m);
                        ChessPicBox[Map[m, n]].Visible = true;
                    }
                }
        }
        
        /// <summary>
        /// 网络对战棋盘point信息转换
        /// </summary>
        /// <param name="OldPoint">旧的棋子序号</param>
        /// <returns></returns>
        private int ConvertPoint(int OldPoint)
        {
            return 59 - OldPoint;
        }
       
        /// <summary>
        /// 移动棋子 idx棋子索引号，（x,y)目标位置
        /// </summary>
        /// <param name="idx">待移动棋子索引</param>
        /// <param name="x">移动横坐标</param>
        /// <param name="y">移动纵坐标</param>
        private void MoveChess(int idx, int x, int y)
        {
            if (Music)
            {
                PlaySound.Play(path + "\\wav\\" + "MOVE.wav");
            }
            Map[MapConvertYandTop(ChessPicBox[idx].Top), MapConvertXandLeft(ChessPicBox[idx].Left)] = 999;//原来位置的棋子置为空
            if (Map[y, x] == 999)
            {
                ChessPicBox[idx].Left = MapConvertXandLeft(x);
                ChessPicBox[idx].Top = MapConvertYandTop(y);
                Map[y, x] = idx;
            }
            else
            {
                ChessPicBox[Map[y, x]].Visible = false;
                ChessPicBox[idx].Left = MapConvertXandLeft(x);
                ChessPicBox[idx].Top = MapConvertYandTop(y);
                Map[y, x] = idx;
            }
        }
        
        /// <summary>
        /// 棋子是否是红棋
        /// </summary>
        /// <param name="tmp">选中棋子</param>
        /// <returns></returns>
        private bool IsRedChess(PictureBox tmp)
        {
            int i = Convert.ToInt16(tmp.Tag);
            if (i >= 0 && i <= 24) return true;
            return false;
        }
        
        /// <summary>
        /// 第一个【点击】棋子能否移动
        /// </summary>
        /// <param name="tmp">选中棋子</param>
        /// <returns></returns>
        private bool FirstChessCanMove(PictureBox tmp)
        {
            if (Wechat&&(state==3||state==4))
            {
                OpenWechat();
            }
            else
            {
                CloseWechat();
            }
            int i = (Convert.ToInt16(tmp.Tag) % 25);
            if(state==2&&GamerColor!=TurnColor)
            { MessageBox.Show("等待机器下棋。"); return false; }
            
            else if (TurnColor == PlayerColor.Red && IsRedChess(tmp))
            {
                if (Chess[i] == 1 || Chess[i] == 2 || IsBigHome(MapConvertXandLeft(tmp.Left), MapConvertYandTop(tmp.Top)))
                { MessageBox.Show("军旗区棋子和地雷不可移动"); return false; }
                return true;
            }
            else if (TurnColor == PlayerColor.Blue && !IsRedChess(tmp))
            {
                if (Chess[i] == 1 || Chess[i] == 2 || IsBigHome(MapConvertXandLeft(tmp.Left), MapConvertYandTop(tmp.Top)))
                { MessageBox.Show("军旗区棋子和地雷不可移动"); return false; }
                return true;
            }
            else { MessageBox.Show("当前应该对方颜色下棋。"); return false; }
        }
        
        /// <summary>
        /// 是否是大本营
        /// </summary>
        /// <param name="old_x">选中横坐标</param>
        /// <param name="old_y">选中纵坐标</param>
        /// <returns></returns>
        private bool IsBigHome(int old_x, int old_y)
        {
            if (old_x == 1 && old_y == 0) return true;
            if (old_x == 3 && old_y == 0) return true;
            if (old_x == 1 && old_y == 11) return true;
            if (old_x == 3 && old_y == 11) return true;
            return false;
        }
        
        /// <summary>
        /// 是否是行营
        /// </summary>
        /// <param name="x">选中横坐标</param>
        /// <param name="y">选中纵坐标</param>
        /// <returns></returns>
        private bool IsCamp(int x, int y)
        {
            if (
                (x == 1 && y == 2) || (x == 1 && y == 4) || (x == 1 && y == 7) || (x == 1 && y == 9) ||
                (x == 3 && y == 2) || (x == 3 && y == 4) || (x == 3 && y == 7) || (x == 3 && y == 9) ||
                (x == 2 && y == 3) || (x == 2 && y == 8)
                ) return true;
            else return false;
        }

        /// <summary>
        /// 【点击】第二个棋子吃子，网络对战中使用CI.sendMsg(2, 0, Str);移动，[idx,point]
        /// </summary>
        /// <param name="tmp">选中棋子</param>
        private void SecondChessAction(PictureBox tmp)
        {
            int i = Convert.ToInt16(tmp.Tag);
            int x = MapConvertXandLeft(tmp.Left);
            int y = MapConvertYandTop(tmp.Top);
            IsFirstChess = true;//无论能否移动，都需要重新选第一个棋子

            if (state == 2 && IsRedChess(tmp))
            { MessageBox.Show("不要尝试吃自己的棋子。"); return ; }
            if (state == 3 && IsRedChess(tmp))
            { MessageBox.Show("不要尝试吃自己的棋子。"); return ; }
            if (state == 4 && !IsRedChess(tmp))
            { MessageBox.Show("不要尝试吃自己的棋子。"); return; }
            if ((TurnColor == PlayerColor.Red && !IsRedChess(tmp)) || (TurnColor == PlayerColor.Blue && IsRedChess(tmp))) { }
            else { MessageBox.Show("不要尝试吃自己的棋子。"); return; }

            //子在行营，无法吃
            if (IsCamp(x, y)) { MessageBox.Show("对方棋子在行营，不能吃。"); }
            //子在相邻位置，判断能否吃（判断自己是否在行营）
            else if (IsCamp(old_x, old_y))
            {
                if (Math.Abs(x - old_x) <= 1 && Math.Abs(y - old_y) <= 1)
                {
                    EatAction(idx, i); 
                }
                else { MessageBox.Show("距离太远，吃不到。"); }
            }
            else if (x == old_x && Math.Abs(y - old_y) == 1 || y == old_y && Math.Abs(x - old_x) == 1)
            {
                if (!(y + old_y == 11 && (x == 1 || x == 3)))
                { EatAction(idx, i);  }
            }
            //子在不相邻位置，但都铁道上且能吃到（判断步兵行走）
            else if (IfTOnRail(x, y) && IfTOnRail(old_x, old_y))
            {
                if (RailCanGO(idx, x, y)) { EatAction(idx, i);  }
            }
            else { MessageBox.Show("操作不规范，吃不到。"); }
            if(state==1||state==2) ReverseTurnColor();
            if (state == 2) AIAction();
        }

        /// <summary>
        /// 【点击】第二个空地移动，网络对战中使用CI.sendMsg(2, 0, Str);移动，[idx,point]
        /// </summary>
        /// <param name="x">选中横坐标</param>
        /// <param name="y">选中纵坐标</param>
        private void SecondFreeAction(int x, int y)
        {
            IsFirstChess = true;//无论能否移动，都需要重新选第一个棋子
            byte IClass=2;
            bool flag=false;
            //进空行营
            if (IsCamp(x, y) && Math.Abs(x - old_x) <= 1 && Math.Abs(y - old_y) <= 1)
            {
                MoveChess(idx, x, y); 
                flag = true;
            }
            else if (IsCamp(old_x, old_y) && Math.Abs(x - old_x) <= 1 && Math.Abs(y - old_y) <= 1)//出行营去空地
            {
                MoveChess(idx, x, y); 
                flag = true;
            }
            else if (x == old_x && Math.Abs(y - old_y) == 1 || y == old_y && Math.Abs(x - old_x) == 1)//移动至相邻位置
            {
                if (!(y + old_y == 11 && (x == 1 || x == 3)))
                { MoveChess(idx, x, y);  flag = true; }
            }
            else if (IfTOnRail(x, y) && IfTOnRail(old_x, old_y))//铁道移动
            {
                if (RailCanGO(idx, x, y)) { MoveChess(idx, x, y);  flag = true; }
                else MessageBox.Show("铁路无法直达。");
            }
            else { MessageBox.Show("操作不规范，无法到达。"); }

            if(flag&&(state == 3 || state == 4))
            {
                CI.sendMsg(IClass, 0, idx+","+(y*5+x));
            }
            if(flag&& (state == 1 || state == 2) ) ReverseTurnColor();
            if (state == 2) AIAction();
        }
        
        /// <summary>
        /// AI模式中机器下棋，使用遍历打分机制，选取得分最高的走法
        /// </summary>
        private void AIAction()
        {
            int bestscore = 0;//评分
            int score = 0;
            int idx=25;//欲移动棋子索引
            int tmpidx;//待考量棋子索引：异色，普通棋子吃子得（2+吃-被吃），炸弹吃子（被吃-吃-5），吃地雷炸弹（被吃-吃+5），吃军旗20分 
            int oldx, oldy;//原棋子横纵坐标
            int x=0;//欲移动位置横坐标
            int y=0;//欲移动位置纵坐标
            for(int i=29;i<50;i++)
            {
                if(ChessPicBox[i].Visible)
                {
                    oldx = MapConvertXandLeft(ChessPicBox[i].Left);
                    oldy = MapConvertYandTop(ChessPicBox[i].Top);
                    if (IfTOnRail(oldx, oldy))//棋子在铁道上
                    {
                        //能否向左，能向左到哪一位置，能否吃
                        if (oldx >= 1)
                        {
                            int length = 0;
                            bool flag = true;
                            for (; flag;)
                            {

                                if (oldx - (length + 1) >= 0 && Map[oldy, oldx - (length + 1)] == 999 && IfTOnRail(oldx - (length + 1), oldy))//左侧为空且是铁路
                                {
                                    length++;
                                    if (oldx - length == 0)
                                    {
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = 0;
                                            y = oldy;
                                            idx = i;
                                        }
                                        break;
                                    }
                                }
                                else if (oldx - (length + 1) >= 0 && Map[oldy, oldx - (length + 1)] == 999)//左侧为空但不是铁路
                                {
                                    if (length == 0)//左边直接是空地非铁路
                                    {
                                        length++;

                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx - (length + 1);
                                            y = oldy;
                                            idx = i;
                                        }
                                        break;
                                    }
                                    //或者，再往前是非铁路，铁路走到尽头
                                    else
                                    {

                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx - length;
                                            y = oldy;
                                            idx = i;
                                        }
                                        break;
                                    }
                                }
                                else if (oldx - (length + 1) >= 0 && Map[oldy, oldx - (length + 1)] < 25 && !IsCamp(oldx - (length + 1), oldy))//左侧是其他棋子，且可以吃：棋子在不在铁路上
                                {
                                    if (IfTOnRail(oldx - (length + 1), oldy))//两个棋都在铁路上
                                    {
                                        //打分
                                        if (Chess[i - 25] == 3)//蓝色炸弹吃
                                        {
                                            score = Chess[Map[oldy, oldx - (length + 1)]] - Chess[i - 25] - 5;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx - (length + 1);
                                                y = oldy;
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy, oldx - (length + 1)]] == 2 || Chess[Map[oldy, oldx - (length + 1)]] == 3)//吃炸弹地雷
                                        {
                                            score = Chess[Map[oldy, oldx - (length + 1)]] - Chess[i - 25] + 5;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx - (length + 1);
                                                y = oldy;
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy, oldx - (length + 1)]] == 1) //吃军旗
                                        {
                                            score = 20;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx - (length + 1);
                                                y = oldy;
                                                idx = i;
                                            }
                                        }
                                        else //其他情况
                                        {
                                            score = 2 + Chess[i - 25] - Chess[Map[oldy, oldx - (length + 1)]];
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx - (length + 1);
                                                y = oldy;
                                                idx = i;
                                            }
                                        }

                                        break;
                                    }
                                    else//被吃的棋不在铁路上
                                    {

                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx - length;
                                            y = oldy;
                                            idx = i;
                                        }
                                        break;
                                    }
                                }
                                else break;
                               
                            }
                        }
                        //能否向右，能向右到哪一位置，能否吃
                        if (oldx <= 3)
                        {
                            int length = 0;
                            bool flag = true;
                            for (; flag;)
                            {
                                if (oldx + (length + 1) <= 4 && Map[oldy, oldx + (length + 1)] == 999 && IfTOnRail(oldx + (length + 1), oldy))//左侧为空且是铁路
                                {
                                    length++;
                                    if (oldx + length == 4)
                                    {
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = 4;
                                            y = oldy;
                                            idx = i;
                                        }
                                        flag = false;
                                    }
                                }
                                else if (oldx + (length + 1) <= 4 && Map[oldy, oldx + (length + 1)] == 999)//左侧为空但不是铁路
                                {
                                    if (length == 0)//左边直接是空地非铁路
                                    {
                                        length++;
                                        flag = false;
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx + (length + 1);
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                    //或者，再往前是非铁路，铁路走到尽头
                                    else
                                    {
                                        flag = false;
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx + length;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                }
                                else if (oldx + (length + 1) <= 4 && Map[oldy, oldx + (length + 1)] < 25 && !IsCamp(oldx + (length + 1), oldy))//左侧是其他棋子，且可以吃：棋子在不在铁路上
                                {
                                    if (IfTOnRail(oldx + (length + 1), oldy))//两个棋都在铁路上
                                    {
                                        //打分
                                        if (Chess[i - 25] == 3)//蓝色炸弹吃
                                        {
                                            score = Chess[Map[oldy, oldx + (length + 1)]] - Chess[i - 25] - 5;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx + (length + 1);
                                                y = oldy;
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy, oldx + (length + 1)]] == 2 || Chess[Map[oldy, oldx + (length + 1)]] == 3)//吃炸弹地雷
                                        {
                                            score = Chess[Map[oldy, oldx + (length + 1)]] - Chess[i - 25] + 5;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx + (length + 1);
                                                y = oldy;
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy, oldx + (length + 1)]] == 1) //吃军旗
                                        {
                                            score = 20;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx + (length + 1);
                                                y = oldy;
                                                idx = i;
                                            }
                                        }
                                        else //其他情况
                                        {
                                            score = 2 + Chess[i - 25] - Chess[Map[oldy, oldx + (length + 1)]];
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx + (length + 1);
                                                y = oldy;
                                                idx = i;
                                            }
                                        }
                                        length++;
                                        flag = false;

                                    }
                                    else//被吃的棋不在铁路上
                                    {
                                        flag = false;
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx + length;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                }
                                else break;
                            }
                        }
                        //能否向上，能向上到哪一位置，能否吃
                        if (oldy >= 1 && !(oldx == 1 && oldy == 6) && !(oldx == 3 && oldy == 6))
                        {
                            int length = 0;
                            bool flag = true;
                            for (; flag;)
                            {
                                if (oldy - (length + 1) >= 0 && Map[oldy - (length + 1), oldx] == 999 && IfTOnRail(oldx, oldy - (length + 1)))//上侧为空且是铁路
                                {
                                    length++;
                                    if (oldy - length == 0) flag = false;
                                }
                                else if (oldy - (length + 1) >= 0 && Map[oldy - (length + 1), oldx] == 999)//上侧为空但不是铁路
                                {
                                    if (length == 0)//上边直接是空地非铁路
                                    {
                                        length++;
                                        flag = false;
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy - length;
                                            idx = i;
                                        }
                                    }
                                    //或者，再往前是非铁路，铁路走到尽头
                                    else
                                    {
                                        flag = false;
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy - length;
                                            idx = i;
                                        }
                                    }
                                }
                                else if (oldy - (length + 1) >= 0 && Map[oldy - (length + 1), oldx] < 25 && !IsCamp(oldx + (length + 1), oldy))//左侧是其他棋子，且可以吃：棋子在不在铁路上
                                {
                                    if (IfTOnRail(oldx, oldy - (length + 1)))//两个棋都在铁路上
                                    {
                                        //打分
                                        if (Chess[i - 25] == 3)//蓝色炸弹吃
                                        {
                                            score = Chess[Map[oldy - (length + 1), oldx]] - Chess[i - 25] - 5;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx;
                                                y = oldy - (length + 1);
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy - (length + 1), oldx]] == 2 || Chess[Map[oldy - (length + 1), oldx]] == 3)//吃炸弹地雷
                                        {
                                            score = Chess[Map[oldy - (length + 1), oldx]] - Chess[i - 25] + 5;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx;
                                                y = oldy - (length + 1);
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy - (length + 1), oldx]] == 1) //吃军旗
                                        {
                                            score = 20;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx;
                                                y = oldy - (length + 1);
                                                idx = i;
                                            }
                                        }
                                        else //其他情况
                                        {
                                            score = 2 + Chess[i - 25] - Chess[Map[oldy - (length + 1), oldx]];
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx;
                                                y = oldy - (length + 1);
                                                idx = i;
                                            }
                                        }
                                        length++;
                                        flag = false;

                                    }
                                    else//被吃的棋不在铁路上
                                    {
                                        flag = false;
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy - length;
                                            idx = i;
                                        }
                                    }
                                }
                                else break;
                            }
                        }
                        //能否向下，能向下到哪一位置，能否吃
                        if (oldy <= 10 && !(oldx == 1 && oldy == 5) && !(oldx == 3 && oldy == 5))
                        {
                            int length = 0;
                            bool flag = true;
                            for (; flag;)
                            {
                                if (oldy + (length + 1) <= 11 && Map[oldy + (length + 1), oldx] == 999 && IfTOnRail(oldx, oldy + (length + 1)))//上侧为空且是铁路
                                {
                                    length++;
                                    if (oldy + length == 4) flag = false;
                                }
                                else if (oldy + (length + 1) <= 11 && Map[oldy + (length + 1), oldx] == 999)//上侧为空但不是铁路
                                {
                                    if (length == 0)//上边直接是空地非铁路
                                    {
                                        length++;
                                        flag = false;
                                        score = 2;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy + length;
                                            idx = i;
                                        }
                                    }
                                    //或者，再往前是非铁路，铁路走到尽头
                                    else
                                    {
                                        flag = false;
                                        score = 2;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy + length;
                                            idx = i;
                                        }
                                    }
                                }
                                else if (oldy + (length + 1) <= 11 && Map[oldy + (length + 1), oldx] < 25 && !IsCamp(oldx + (length + 1), oldy))//左侧是其他棋子，且可以吃：棋子在不在铁路上
                                {
                                    if (IfTOnRail(oldx, oldy + (length + 1)))//两个棋都在铁路上
                                    {
                                        //打分
                                        if (Chess[i - 25] == 3)//蓝色炸弹吃
                                        {
                                            score = Chess[Map[oldy + (length + 1), oldx]] - Chess[i - 25] - 5 + 2;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx;
                                                y = oldy + (length + 1);
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy + (length + 1), oldx]] == 2 || Chess[Map[oldy + (length + 1), oldx]] == 3)//吃炸弹地雷
                                        {
                                            score = Chess[Map[oldy + (length + 1), oldx]] - Chess[i - 25] + 5 + 2;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx;
                                                y = oldy + (length + 1);
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy + (length + 1), oldx]] == 1) //吃军旗
                                        {
                                            score = 20;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx;
                                                y = oldy + (length + 1);
                                                idx = i;
                                            }
                                        }
                                        else //其他情况
                                        {
                                            score = 2 + Chess[i - 25] - Chess[Map[oldy + (length + 1), oldx]];
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx;
                                                y = oldy + (length + 1);
                                                idx = i;
                                            }
                                        }
                                        length++;
                                        flag = false;

                                    }
                                    else//被吃的棋不在铁路上
                                    {
                                        flag = false;
                                        score = 1;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy + length;
                                            idx = i;
                                        }
                                    }
                                }
                                else break;
                            }
                        }
                        //能否斜着进行营

                    }
                    else if (IsCamp(oldx, oldy))//棋子在行营
                    {
                        for(int m = -1 ; m <= 1 ; m++ )
                            for (int n = -1; n <= 1; n++)
                            {
                                if (m == 0 && n == 0) continue;
                                //是否为空，不空时是否为行营，不为行营能否吃，打分
                                if (Map[oldy + m, oldx + n ] == 999)
                                {
                                    score = 1;
                                    if (bestscore < score)
                                    {
                                        bestscore = score;
                                        x = oldx+n;
                                        y = oldy+m;
                                        idx = i;
                                    }
                                }
                                else
                                {
                                    if (Map[oldy + m, oldx +n ] < 25 && !IsCamp(oldx +n, oldy+m))
                                    {
                                        if (Chess[i - 25] == 3)//蓝色炸弹吃
                                        {
                                            score = Chess[Map[oldy + m, oldx + n]] - Chess[i - 25] - 5;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx + n;
                                                y = oldy + m;
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy + m, oldx + n]] == 2 || Chess[Map[oldy + m, oldx + n]] == 3)//吃炸弹地雷
                                        {
                                            score = Chess[Map[oldy + m, oldx + n]] - Chess[i - 25] + 5;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx + n;
                                                y = oldy + m;
                                                idx = i;
                                            }
                                        }
                                        else if (Chess[Map[oldy + m, oldx + n]] == 1) //吃军旗
                                        {
                                            score = 20;
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx + n;
                                                y = oldy + m;
                                                idx = i;
                                            }
                                        }
                                        else //其他情况
                                        {
                                            score = 2 + Chess[i - 25] - Chess[Map[oldy + m, oldx + n]];
                                            if (bestscore < score)
                                            {
                                                bestscore = score;
                                                x = oldx + n;
                                                y = oldy + m;
                                                idx = i;
                                            }
                                        }
                                    }
                                }

                            }
                    }
                    else if (!IsBigHome(oldx, oldy))//普通兵站
                    {
                        //能否向左，能否吃。空1分，吃子(2+落差)分
                        if (oldx >= 1)
                        {
                            if (Map[oldy, oldx - 1] == 999)
                            {
                                score = 1;
                                if (bestscore < score)
                                {
                                    bestscore = score;
                                    x = oldx - 1;
                                    y = oldy;
                                    idx = i;
                                }
                            }
                            else
                            {
                                if (Map[oldy, oldx - 1] < 25 && !IsCamp(oldx - 1, oldy))
                                {
                                    if (Chess[i - 25] == 3)//蓝色炸弹吃
                                    {
                                        score = Chess[Map[oldy, oldx - 1]] - Chess[i - 25] - 5;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx - 1;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                    else if (Chess[Map[oldy, oldx - 1]] == 2 || Chess[Map[oldy, oldx - 1]] == 3)//吃炸弹地雷
                                    {
                                        score = Chess[Map[oldy, oldx - 1]] - Chess[i - 25] + 5 ;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx - 1;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                    else if (Chess[Map[oldy, oldx - 1]] == 1) //吃军旗
                                    {
                                        score = 20;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx - 1;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                    else //其他情况
                                    {
                                        score = 2 +  Chess[i - 25] - Chess[Map[oldy, oldx - 1]];
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx - 1;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                }
                            }
                        }
                        //能否向右，能否吃。空1分，吃子(2+落差)分
                        if (oldx <= 3)
                        {
                            if (Map[oldy, oldx + 1] == 999)
                            {
                                score = 1;
                                if (bestscore < score)
                                {
                                    bestscore = score;
                                    x = oldx + 1;
                                    y = oldy;
                                    idx = i;
                                }
                            }
                            else
                            {
                                if (Map[oldy, oldx + 1] < 25 && !IsCamp(oldx + 1, oldy))
                                {
                                    if (Chess[i - 25] == 3)//蓝色炸弹吃
                                    {
                                        score = Chess[Map[oldy, oldx + 1]] - Chess[i - 25] - 5;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx + 1;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                    else if (Chess[Map[oldy, oldx + 1]] == 2 || Chess[Map[oldy, oldx + 1]] == 3)//吃炸弹地雷
                                    {
                                        score = Chess[Map[oldy, oldx + 1]] - Chess[i - 25] + 5;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx + 1;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                    else if (Chess[Map[oldy, oldx + 1]] == 1) //吃军旗
                                    {
                                        score = 20;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx + 1;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                    else //其他情况
                                    {
                                        score = 2 + Chess[i - 25] - Chess[Map[oldy, oldx + 1]];
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx + 1;
                                            y = oldy;
                                            idx = i;
                                        }
                                    }
                                }
                            }
                        }
                        //能否向上，能否吃。空1分，吃子(2+落差)分
                        if (oldy >= 1 && !( oldx ==1 && oldy == 6 ) && !(oldx == 3 && oldy == 6))//山界不可越过
                        {
                            if (Map[oldy-1, oldx] == 999)
                            {
                                score = 1;
                                if (bestscore < score)
                                {
                                    bestscore = score;
                                    x = oldx;
                                    y = oldy-1;
                                    idx = i;
                                }
                            }
                            else
                            {
                                if (Map[oldy - 1, oldx] < 25 && !IsCamp(oldx , oldy-1))
                                {
                                    if (Chess[i - 25] == 3)//蓝色炸弹吃
                                    {
                                        score = Chess[Map[oldy - 1, oldx]] - Chess[i - 25] - 5;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy - 1;
                                            idx = i;
                                        }
                                    }
                                    else if (Chess[Map[oldy - 1, oldx]] == 2 || Chess[Map[oldy - 1, oldx]] == 3)//吃炸弹地雷
                                    {
                                        score = Chess[Map[oldy - 1, oldx]] - Chess[i - 25] + 5;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy - 1;
                                            idx = i;
                                        }
                                    }
                                    else if (Chess[Map[oldy - 1, oldx]] == 1) //吃军旗
                                    {
                                        score = 20;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy - 1;
                                            idx = i;
                                        }
                                    }
                                    else //其他情况
                                    {
                                        score = 2 + Chess[i - 25] - Chess[Map[oldy - 1, oldx]];
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy - 1;
                                            idx = i;
                                        }
                                    }
                                }
                            }
                        }
                        //能否向下，能否吃。空2分，吃子(3+落差)分
                        if (oldy <= 10 && !(oldx == 1 && oldy == 5) && !(oldx == 3 && oldy == 5))//山界不可越过
                        {
                            if (Map[oldy + 1, oldx] == 999)
                            {
                                score = 2;
                                if (bestscore < score)
                                {
                                    bestscore = score;
                                    x = oldx;
                                    y = oldy + 1;
                                    idx = i;
                                }
                            }
                            else
                            {
                                if (Map[oldy + 1, oldx] < 25 && !IsCamp(oldx, oldy + 1))
                                {
                                    if (Chess[i - 25] == 3)//蓝色炸弹吃
                                    {
                                        score = Chess[Map[oldy + 1, oldx]] - Chess[i - 25] - 5;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy + 1;
                                            idx = i;
                                        }
                                    }
                                    else if (Chess[Map[oldy + 1, oldx]] == 2 || Chess[Map[oldy + 1, oldx]] == 3)//吃炸弹地雷
                                    {
                                        score = Chess[Map[oldy + 1, oldx]] - Chess[i - 25] + 5;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy + 1;
                                            idx = i;
                                        }
                                    }
                                    else if (Chess[Map[oldy + 1, oldx]] == 1) //吃军旗
                                    {
                                        score = 20;
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy + 1;
                                            idx = i;
                                        }
                                    }
                                    else //其他情况
                                    {
                                        score = 2 + Chess[i - 25] - Chess[Map[oldy + 1, oldx]];
                                        if (bestscore < score)
                                        {
                                            bestscore = score;
                                            x = oldx;
                                            y = oldy + 1;
                                            idx = i;
                                        }
                                    }
                                }
                            }
                        }
                    }
                 
                }
            }
            //遍历结束，按照分数移动棋子
            if(idx == 25 && x==0 && y==0 )
            {
                
                MessageBox.Show("机器人无路可走，认输。");
                MessageBox.Show("游戏结束，你赢了。");
                return;
            }
            if(Map[y,x]==999)
            {
                Map[MapConvertYandTop(ChessPicBox[idx].Top), MapConvertXandLeft(ChessPicBox[idx].Left)]=999;
                ChessPicBox[idx].Top = MapConvertYandTop(y);
                ChessPicBox[idx].Left = MapConvertXandLeft(x);
                MessageBox.Show(x+"999"+y);
                Map[y, x] = idx;
            }
            else
            {

                if (Chess[Map[y, x]] == 2 || Chess[Map[y, x]] == 3 || Chess[idx-25] == 3)
                {
                    ChessPicBox[idx].Visible = false;
                    ChessPicBox[Map[y, x]].Visible = false;
                    Map[MapConvertYandTop(ChessPicBox[idx].Top), MapConvertXandLeft(ChessPicBox[idx].Left)] = 999;
                    Map[y, x] = 999;
                }
                else
                {
                    ChessPicBox[Map[y, x]].Visible = false;
                    Map[MapConvertYandTop(ChessPicBox[idx].Top), MapConvertXandLeft(ChessPicBox[idx].Left)] = 999;
                    ChessPicBox[idx].Top = MapConvertYandTop(y);
                    ChessPicBox[idx].Left = MapConvertXandLeft(x);
                    MessageBox.Show(x + "idx" + y);
                    Map[y, x] = idx;
                }

            }
            ReverseTurnColor();
        }

        
        /// <summary>
        /// 棋子是否在铁路上
        /// </summary>
        /// <param name="old_x">选中横坐标</param>
        /// <param name="old_y">选中纵坐标</param>
        /// <returns></returns>
        private bool IfTOnRail(int old_x, int old_y)
        {
            if (old_y == 1 || old_y == 5 || old_y == 6 || old_y == 10 ||
                (old_x == 0 || old_x == 4) && (old_y >= 1 && old_y <= 10))
                return true;
            return false;
        }
        
        /// <summary>
        /// 铁路上能否走到指定位置
        /// </summary>
        /// <param name="idx">选中棋子ID</param>
        /// <param name="x">选中横坐标</param>
        /// <param name="y">选中纵坐标</param>
        /// <returns></returns>
        private bool RailCanGO(int idx, int x, int y)
        {
            if (Chess[idx % 25] == 4)//是工兵
            {
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 12; j++)
                    {
                        if (j == 1 || j == 5 || j == 6 || j == 10 ||
                            (i == 0 || i == 4) && (j >= 1 && j <= 10))
                        {
                            if (Map[j, i] != 999) MIGONG[i, j] = 0;
                            else MIGONG[i, j] = 1;
                        }
                        else MIGONG[i, j] = 0;
                    }
                MIGONG[old_x, old_y] = 1;
                // MessageBox.Show(MIGONG[0,5]+"+"+ MIGONG[1, 5]+"+"+ MIGONG[2, 5] + "+"+ MIGONG[3, 5] + "+"+ MIGONG[4, 5]+"\n"+
                //    MIGONG[0, 6] + "+" + MIGONG[1, 6] + "+" + MIGONG[2, 6] + "+" + MIGONG[3,6] + "+" + MIGONG[4, 6]);
                return JudgeWay(old_x, old_y, x, y);
            }
            else if (x == old_x && VLine_Juge(y, old_y, x)) return true;
            else if (y == old_y && HLine_Juge(x, old_x, y)) return true;
            else return false;

        }
     
        /// <summary>
        /// 工兵寻径算法
        /// </summary>
        /// <param name="x1">初始横坐标</param>
        /// <param name="y1">初始纵坐标</param>
        /// <param name="x2">目标横坐标</param>
        /// <param name="y2">目标纵坐标</param>
        /// <returns></returns>
        private bool JudgeWay(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && y1 == y2) return true;
            if (x1 < 0 || x1 > 4 || y1 < 0 || y1 > 11) return false;
            if (MIGONG[x1, y1] == 0) return false;
            MIGONG[x1, y1] = 0;
            return (JudgeWay(x1 + 1, y1, x2, y2) || JudgeWay(x1 - 1, y1, x2, y2) || JudgeWay(x1, y1 + 1, x2, y2) || JudgeWay(x1, y1 - 1, x2, y2));

        }

        /// <summary>
        /// 垂直方向判断是否有别的棋子挡路
        /// </summary>
        /// <param name="m">初始纵坐标</param>
        /// <param name="n">目标纵坐标</param>
        /// <param name="x">横坐标</param>
        /// <returns></returns>
        private bool VLine_Juge(int m, int n, int x)
        {
            int t = Math.Max(m, n);
            m = Math.Min(m, n);
            n = t;
            for (int i = m + 1; i < n; i++)
                if (Map[i, x] != 999) //有别的棋子
                    return false;
            return true;
        }
    
        /// <summary>
        /// 水平方向判断是否有别的棋子挡路
        /// </summary>
        /// <param name="m">初始横坐标</param>
        /// <param name="n">目标横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns></returns>
        private bool HLine_Juge(int m, int n, int y)
        {
            int t = Math.Max(m, n);
            m = Math.Min(m, n);
            n = t;
            for (int i = m + 1; i < n; i++)
                if (Map[y, i] != 999) //有别的棋子
                    return false;
            return true;
        }

        /// <summary>
        /// 走棋吃子：CI.sendMsg(3, 0, Str);调EatAction[idx1,idx2]，CI.sendMsg(4, 0, Str);扛军棋，游戏结束[idx1,idx2]
        /// </summary>
        /// <param name="idx">吃子棋子</param>
        /// <param name="i">被吃棋子</param>
        private void EatAction(int idx, int i)
        {
            byte IClass;
            if (Chess[i % 25] == 2)//吃地雷，如果是工兵则在，其他子死亡
            {
                if (Music)
                {
                    PlaySound.Play(path + "\\wav\\" + "DILEI.wav");
                }
                if (Chess[idx % 25] == 4)
                {
                    ChessPicBox[i].Visible = false;
                    MoveChess(idx, MapConvertXandLeft(ChessPicBox[i].Left), MapConvertYandTop(ChessPicBox[i].Top));
                    IClass = 3;
                 
                }
                else
                {
                    ChessPicBox[i].Visible = false;
                    ChessPicBox[idx].Visible = false;
                    Map[MapConvertYandTop(ChessPicBox[i].Top), MapConvertXandLeft(ChessPicBox[i].Left)] = 999;//原来位置的棋子置为空
                    Map[MapConvertYandTop(ChessPicBox[idx].Top), MapConvertXandLeft(ChessPicBox[idx].Left)] = 999;//原来位置的棋子置为空
                    IClass = 3;
                    
                }
            }
            else if (Chess[i % 25] == 1)//吃军旗，游戏结束
            {
                if (Music)
                {
                    PlaySound.Play(path + "\\wav\\" + "WIN.wav");
                }
                ChessPicBox[i].Visible = false;
                MoveChess(idx, MapConvertXandLeft(ChessPicBox[i].Left), MapConvertXandLeft(ChessPicBox[i].Top));
                MessageBox.Show("游戏结束。");
                IClass = 4;
            }
            else if (Chess[idx % 25] == 3 || Chess[i % 25] == 3)//有一方是炸弹，全消失
            {
                if (Music)
                {
                    PlaySound.Play(path + "\\wav\\" + "DILEI.wav");
                }
                ChessPicBox[i].Visible = false;
                ChessPicBox[idx].Visible = false;
                Map[MapConvertYandTop(ChessPicBox[i].Top), MapConvertXandLeft(ChessPicBox[i].Left)] = 999;//原来位置的棋子置为空
                Map[MapConvertYandTop(ChessPicBox[idx].Top), MapConvertXandLeft(ChessPicBox[idx].Left)] = 999;//原来位置的棋子置为空
                IClass = 3;
            }
            else//普通军队
            {
                if (Chess[idx % 25] > Chess[i % 25])//比对方大，能吃
                {
                    if (Music)
                    {
                        PlaySound.Play(path + "\\wav\\" + "EAT.wav");
                    }
                    ChessPicBox[i].Visible = false;
                    MoveChess(idx, MapConvertXandLeft(ChessPicBox[i].Left), MapConvertYandTop(ChessPicBox[i].Top));
                    //MessageBox.Show("比对方大，能吃。");
                    IClass = 3;
                }
                else if (Chess[idx % 25] == Chess[i % 25])//一样大，全消失
                {
                    if (Music)
                    {
                        PlaySound.Play(path + "\\wav\\" + "DILEI.wav");
                    }
                    ChessPicBox[i].Visible = false;
                    ChessPicBox[idx].Visible = false;
                    Map[MapConvertYandTop(ChessPicBox[i].Top), MapConvertXandLeft(ChessPicBox[i].Left)] = 999;//原来位置的棋子置为空
                    Map[MapConvertYandTop(ChessPicBox[idx].Top), MapConvertXandLeft(ChessPicBox[idx].Left)] = 999;//原来位置的棋子置为空
                    //MessageBox.Show("一样大，全消失。");
                    IClass = 3;
                }
                else//比对方小，消失
                {
                    if (Music)
                    {
                        PlaySound.Play(path + "\\wav\\" + "ERR.wav");
                    }
                    ChessPicBox[idx].Visible = false;
                    Map[MapConvertYandTop(ChessPicBox[idx].Top), MapConvertXandLeft(ChessPicBox[idx].Left)] = 999;//原来位置的棋子置为空
                    //MessageBox.Show("比对方小，消失。");
                    IClass =3;
                }
            }
            if(state==3||state==4) CI.sendMsg(IClass, 0, idx+","+i);
            
           
        }
        
        /// <summary>
        /// 用于变换turncolor
        /// </summary>
        private void ReverseTurnColor()
        {
            if (TurnColor == PlayerColor.Red)
                TurnColor = PlayerColor.Blue;
            else TurnColor = PlayerColor.Red;
        }
        /// <summary>
        /// 用于设置部分聊天功能
        /// </summary>
        public void OpenWechat()
        {
            TalkTextBox.Enabled = true;
            SendTextBox.Enabled = true;
            SendMessageBtn.Enabled = true;
        }
        /// <summary>
        /// 用于重新开始聊天功能
        /// </summary>
        public void CloseWechat()
        {
            TalkTextBox.Enabled = false;
            SendTextBox.Enabled = false;
            SendMessageBtn.Enabled = false; 
        }
        
        /// <summary>
        /// 专门用于将空地图上点击转化为坐标t[y=(t/5),x=(t%5)]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int MapConvertLogic(int x, int y)
        {   //0
            if ((x >= 61 && x <= 121) && (y >= 63 && y <= 93)) { return 0; }
            if ((x >= 208 && x <= 268) && (y >= 63 && y <= 93)) { return 1; }//蓝大本营
            if ((x >= 356 && x <= 416) && (y >= 63 && y <= 93)) { return 2; }
            if ((x >= 505 && x <= 565) && (y >= 63 && y <= 93)) { return 3; }//蓝大本营
            if ((x >= 651 && x <= 711) && (y >= 63 && y <= 93)) { return 4; }
            //1
            if ((x >= 61 && x <= 121) && (y >= 131 && y <= 161)) { return 5; }
            if ((x >= 208 && x <= 268) && (y >= 131 && y <= 161)) { return 6; }
            if ((x >= 356 && x <= 416) && (y >= 131 && y <= 161)) { return 7; }
            if ((x >= 505 && x <= 565) && (y >= 131 && y <= 161)) { return 8; }
            if ((x >= 651 && x <= 711) && (y >= 131 && y <= 161)) { return 9; }
            //2
            if ((x >= 61 && x <= 121) && (y >= 193 && y <= 223)) { return 10; }
            if ((x >= 208 && x <= 268) && (y >= 183 && y <= 233)) { return 11; }//蓝行营
            if ((x >= 356 && x <= 416) && (y >= 193 && y <= 223)) { return 12; }
            if ((x >= 505 && x <= 565) && (y >= 183 && y <= 233)) { return 13; }//蓝行营
            if ((x >= 651 && x <= 711) && (y >= 193 && y <= 223)) { return 14; }
            //3
            if ((x >= 61 && x <= 121) && (y >= 261 && y <= 291)) { return 15; }
            if ((x >= 208 && x <= 268) && (y >= 261 && y <= 291)) { return 16; }
            if ((x >= 356 && x <= 416) && (y >= 251 && y <= 301)) { return 17; }//蓝行营
            if ((x >= 505 && x <= 565) && (y >= 261 && y <= 291)) { return 18; }
            if ((x >= 651 && x <= 711) && (y >= 261 && y <= 291)) { return 19; }
            //4
            if ((x >= 61 && x <= 121) && (y >= 329 && y <= 359)) { return 20; }
            if ((x >= 208 && x <= 268) && (y >= 319 && y <= 369)) { return 21; }//蓝行营
            if ((x >= 356 && x <= 416) && (y >= 329 && y <= 359)) { return 22; }
            if ((x >= 505 && x <= 565) && (y >= 319 && y <= 369)) { return 23; }//蓝行营
            if ((x >= 651 && x <= 711) && (y >= 329 && y <= 359)) { return 24; }
            //5
            if ((x >= 61 && x <= 121) && (y >= 397 && y <= 427)) { return 25; }
            if ((x >= 208 && x <= 268) && (y >= 397 && y <= 427)) { return 26; }
            if ((x >= 356 && x <= 416) && (y >= 397 && y <= 427)) { return 27; }
            if ((x >= 505 && x <= 565) && (y >= 397 && y <= 427)) { return 28; }
            if ((x >= 651 && x <= 711) && (y >= 397 && y <= 427)) { return 29; }
            //6
            if ((x >= 61 && x <= 121) && (y >= 598 && y <= 628)) { return 30; }
            if ((x >= 208 && x <= 268) && (y >= 598 && y <= 628)) { return 31; }
            if ((x >= 356 && x <= 416) && (y >= 598 && y <= 628)) { return 32; }
            if ((x >= 505 && x <= 565) && (y >= 598 && y <= 628)) { return 33; }
            if ((x >= 651 && x <= 711) && (y >= 598 && y <= 628)) { return 34; }
            //7
            if ((x >= 61 && x <= 121) && (y >= 665 && y <= 695)) { return 35; }
            if ((x >= 208 && x <= 268) && (y >= 655 && y <= 705)) { return 36; }//红行营
            if ((x >= 356 && x <= 416) && (y >= 665 && y <= 695)) { return 37; }
            if ((x >= 505 && x <= 565) && (y >= 655 && y <= 705)) { return 38; }//红行营
            if ((x >= 651 && x <= 711) && (y >= 665 && y <= 695)) { return 39; }
            //8
            if ((x >= 61 && x <= 121) && (y >= 733 && y <= 763)) { return 40; }
            if ((x >= 208 && x <= 268) && (y >= 733 && y <= 763)) { return 41; }
            if ((x >= 356 && x <= 416) && (y >= 723 && y <= 773)) { return 42; }//红行营
            if ((x >= 505 && x <= 565) && (y >= 733 && y <= 763)) { return 43; }
            if ((x >= 651 && x <= 711) && (y >= 733 && y <= 763)) { return 44; }
            //9
            if ((x >= 61 && x <= 121) && (y >= 801 && y <= 831)) { return 45; }
            if ((x >= 208 && x <= 268) && (y >= 791 && y <= 841)) { return 46; }//红行营
            if ((x >= 356 && x <= 416) && (y >= 801 && y <= 831)) { return 47; }
            if ((x >= 505 && x <= 565) && (y >= 791 && y <= 841)) { return 48; }//红行营
            if ((x >= 651 && x <= 711) && (y >= 801 && y <= 831)) { return 49; }
            //10
            if ((x >= 61 && x <= 121) && (y >= 863 && y <= 893)) { return 50; }
            if ((x >= 208 && x <= 268) && (y >= 863 && y <= 893)) { return 51; }
            if ((x >= 356 && x <= 416) && (y >= 863 && y <= 893)) { return 52; }
            if ((x >= 505 && x <= 565) && (y >= 863 && y <= 893)) { return 53; }
            if ((x >= 651 && x <= 711) && (y >= 863 && y <= 893)) { return 54; }
            //11
            if ((x >= 61 && x <= 121) && (y >= 931 && y <= 961)) { return 55; }
            if ((x >= 208 && x <= 268) && (y >= 931 && y <= 961)) { return 56; }//红大本营
            if ((x >= 356 && x <= 416) && (y >= 931 && y <= 961)) { return 57; }
            if ((x >= 505 && x <= 565) && (y >= 931 && y <= 961)) { return 58; }//红大本营
            if ((x >= 651 && x <= 711) && (y >= 931 && y <= 961)) { return 59; }

            return 999;
        }
        
        /// <summary>
        /// 专门用于捕捉鼠标在空地图上的点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_MouseDownMap(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            PictureBox picBoxtmp = (PictureBox)sender;//获取整张地图
            int tmp = MapConvertLogic(e.X, e.Y);
            if (tmp != 999)
            {
                int x = tmp % 5;
                int y = tmp / 5;
                if (!IsFirstChess)
                {
                    SecondFreeAction(x, y);
                }
            }
        }
        /// <summary>
        /// 专门用于捕捉鼠标在空地图上的移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_MouseMoveMap(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (MapConvertLogic(e.X, e.Y) != 999) { Cursor.Current = Cursors.Hand; }
        }

        /// <summary>
        /// 专门用于捕捉鼠标在棋子上的点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            PictureBox TmpPicBox = (PictureBox)sender;
            int i = Convert.ToInt16(TmpPicBox.Tag);
            int x = MapConvertXandLeft(TmpPicBox.Left);
            int y = MapConvertYandTop(TmpPicBox.Top);
            //MessageBox.Show(y+","+x ); 
            if (IsFirstChess)
            {
                if (FirstChessCanMove(TmpPicBox))
                {
                    idx = i;
                    old_x = x;
                    old_y = y;
                    IsFirstChess = false;
                }
            }
            else if (!IsFirstChess)
            {
                SecondChessAction(TmpPicBox);
            }

        }
        /// <summary>
        /// 专门用于捕捉鼠标在棋子上的移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }
        
        /// <summary>
        /// 开启本地对战
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalPlayBtn_Click(object sender, EventArgs e)
        {
            state = 1;
            RePlayBtn.Enabled = true;
            SaveMapBtn.Enabled = true;
            ReadMapBtn.Enabled = true;
            HaveNewMap();
            HaveChessOnNewMap();
            ShowNewMap();
            
            if (Music)
            {
                PlaySound.Play(path + "\\wav\\" + "START.wav");
            }
            WebPlayBtn.Enabled = false;
        }

        /// <summary>
        /// 开启人机对战
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AIPlayBtn_Click(object sender, EventArgs e)
        {
            state = 2;
            RePlayBtn.Enabled = true;
            SaveMapBtn.Enabled = true;
            ReadMapBtn.Enabled = true;
            GamerColor = PlayerColor.Red;
            TurnColor = PlayerColor.Red;
            HaveNewMap();
            HaveChessOnNewMap();
            ShowNewMap();
            if (Music)
            {
 
                PlaySound.Play(path + "\\wav\\" + "START.wav");
            }
            WebPlayBtn.Enabled = false;
        }

 
        /// <summary>
        /// 开启网络对战
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebPlayBtn_Click(object sender, EventArgs e)
        {
            if (Music)
            {             
                PlaySound.Play(path + "\\wav\\"+ "START.wav");
            }
            IPOfMineBox.Enabled = true;
            IPAddressTextBox.Enabled = true;
            MyServerBtn.Enabled = true;
            MyClientBtn.Enabled = true;
            IPAddress[] ip_list = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            IPAddress myIp = ip_list[ip_list.Length - 1];//获取IP地址
            IPOfMineBox.Text = myIp.ToString();
            LocalPlayBtn.Enabled = false;
            AIPlayBtn.Enabled = false;
            RePlayBtn.Enabled = false;
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RePlayBtn_Click(object sender, EventArgs e)
        {
            if (Music)
            {    
                PlaySound.Play(path + "\\wav\\" + "START.wav");
            }
            HaveNewMap();
            HaveChessOnNewMap();
            ShowNewMap();

        }

        /// <summary>
        /// 保存残局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveMapBtn_Click(object sender, EventArgs e)
        {
            string txtstate = state.ToString() ;
            string txtplayer = GamerColor.ToString()+"@"+ TurnColor.ToString();
            string MAP="";
            for (int m = 0; m < 12; m++)
                for (int n = 0; n < 5; n++)
                {
                    MAP += Map[m, n] + "@";
                }
            if (state == 1 || state == 2)
            {
                
                    FileStream tmp = new FileStream("CANJU.txt", FileMode.Create, FileAccess.ReadWrite);
                    tmp.Close();
                    File.WriteAllText("CANJU.txt", txtstate+"\n"+ txtplayer + "\n"+MAP, Encoding.UTF8);
                

            }
            else if (state == 3 || state == 4)
            {
                MessageBox.Show("网络实时对战进行中，无法存储进度！");
            }
            else
            {
                MessageBox.Show("游戏尚未开始，无法存储进度！");
            }
        }

        /// <summary>
        /// 读取残局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadMapBtn_Click(object sender, EventArgs e)
        {          
            HaveNewMap();
            string[] CANJU = File.ReadAllLines("CANJU.txt");
            state = Convert.ToInt32(CANJU[0]);
            string[] words = CANJU[1].Split('@');
            if(words[0]=="Red")
            {
                GamerColor = PlayerColor.Red;
            }
            else GamerColor = PlayerColor.Blue;
            if (words[1] == "Red")
            {
                GamerColor = PlayerColor.Red;
            }
            else GamerColor = PlayerColor.Blue;
            
            string[] map = CANJU[2].Split('@');
            for (int m = 0; m < 12; m++)
                for (int n = 0; n < 5; n++)
                {
                    Map[m, n] = Convert.ToInt32(map[m * 5 + n]);
                }
            ShowNewMap();
        }

        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingBtn_Click(object sender, EventArgs e)
        {
            if (Music)
            {
                
                PlaySound.Play(path + "\\wav\\" + "BTNSHEZHI.wav");
            }
            FormSet set = new FormSet();
            set.Show();
        }

        /// <summary>
        /// 帮助按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpBtn_Click(object sender, EventArgs e)
        {
            if (Music)
            {
                
                PlaySound.Play(path + "\\wav\\" + "BTNBANGZHU.wav");
            }
            FormHelp help = new FormHelp();
            help.Show();
            
        }

        /// <summary>
        /// 关于按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutBtn_Click(object sender, EventArgs e)
        {
            if (Music)
            {
               
                PlaySound.Play(path + "\\wav\\" + "BTNGUANYU.wav");
            }
            FormAbout about = new FormAbout();
            about.Show();
        }

        /// <summary>
        /// 监听按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListen_Click(object sender, EventArgs e)
        {
            if (Music)
            {
              
                PlaySound.Play(path + "\\wav\\" + "BTNBANGZHU.wav");
            }
            IPAddressTextBox.Enabled = false;
            MyClientBtn.Enabled = false;
            Thread listenThread = new Thread(new ThreadStart(listen));
            listenThread.Start();
        }

        /// <summary>
        /// 监听函数
        /// </summary>
        private void listen()
        {
            try
            {
                Thread.CurrentThread.IsBackground = true;
                Control.CheckForIllegalCrossThreadCalls = false;  
                CI.listen();
                CI.OnReceiveMsg += new ChessEventHander(manageChessEvent);
            }
            catch (Exception ex)
            {
                TalkTextBox.Text += "listen:\r\n" + ex.Message + "\r\n";
            }
        }

        /// <summary>
        /// 连接按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (Music)
                {
                    
                    PlaySound.Play(path + "\\wav\\" + "BTNGUANYU.wav");
                }
                CI.connect(IPAddressTextBox.Text);     
                CI.OnReceiveMsg += new ChessEventHander(manageChessEvent);
                CI.sendMsg(0, 0,"READY");
                IPAddressTextBox.ReadOnly = true;
                MyClientBtn.Enabled = false;          
                TalkTextBox.Enabled = true;
                SendTextBox.Enabled = true;
                SendMessageBtn.Enabled = true;
                state = 4;//发送消息，网络对战后手蓝方
                GamerColor = PlayerColor.Blue;
                ReverseTurnColor();
                MyServerBtn.Enabled = false;
            }
            catch (Exception ex)
            {
                IPAddressTextBox.Text += "btn_connect_Click:\r\n" + ex.Message + "\r\n";
            }
        }

        /// <summary>
        /// 发送聊天消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (Music)
                { 
                    PlaySound.Play(path + "\\wav\\" + "BTN.wav");
                }
                CI.sendMsg(1, 0, SendTextBox.Text);//【Iclass\flag\content】
                if(state==3) 
                TalkTextBox.Text += "红方："+SendTextBox.Text + "\r\n";
                else TalkTextBox.Text += "蓝方：" + SendTextBox.Text + "\r\n";
                SendTextBox.Clear();
            }
            catch (Exception ex)
            {
                SendTextBox.Text += "btnSendMs_Click:\r\n" + ex.Message + "\r\n";
            }
        }

        /// <summary>
        /// 处理接收信息事件
        /// CI.sendMsg(2, 0, Str);移动，[idx,point]
        /// CI.sendMsg(3, 0, Str);大于，吃子,[idx1,null]
        /// CI.sendMsg(4, 0, Str);小于，消失,[idx1, idx2]
        /// CI.sendMsg(5, 0, Str);等于，共同消失[idx1, idx2]
        /// CI.sendMsg(6, 0, Str); 扛军棋，游戏结束[idx1, idx2]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void manageChessEvent(object sender, ChessEvent e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            switch (e.Iclass)
            {
                case "0":   //初始化棋局信息
                    {
                        if (e.content == "READY")
                        {
                            HaveNewMap();
                            HaveChessOnNewMap();
                            ShowNewMap();
                            MyServerBtn.Enabled = false;
                            MyClientBtn.Enabled = false;
                            TalkTextBox.Enabled = true;
                            SendTextBox.Enabled = true;
                            SendMessageBtn.Enabled = true;
                            state = 3;//接收消息，网络对战先手红方
                            GamerColor = PlayerColor.Red;
                            for (int m = 0; m < 12; m++)
                                for (int n = 0; n < 5; n++)
                                {
                                    string tmp = Map[m, n] + "," + (m * 5 + n);
                                    Thread.Sleep(10);
                                    CI.sendMsg(0, 0 ,tmp );
                                }
                        }
                        else
                        {
             
                            string[] PTemp = e.content.Split(',');
                            string Sidx = PTemp[0];
                            string Spoint = PTemp[1];
                            int idx = Convert.ToInt32(Sidx);
                            int point = ConvertPoint(Convert.ToInt32(Spoint));
                            int x = point % 5;
                            int y = point / 5;
                            Map[y, x] = idx;
                            if(point==0)
                            {
                                ShowNewMap();
                            }
                        }
                        break;
                    }
                case "1":   //聊天
                    {
                        if(state==3) 
                        TalkTextBox.Text += "蓝方："+e.content + "\r\n";
                        else TalkTextBox.Text += "红方：" + e.content + "\r\n";
                    }
                    break;
                case "2":   //下棋:移动至空处
                    {
                        string[] PTemp = e.content.Split(',');
                        string Sidx = PTemp[0];
                        string Spoint = PTemp[1];
                        int idx = Convert.ToInt32(Sidx);
                        int point = ConvertPoint(Convert.ToInt32(Spoint));
                        int x = point % 5;
                        int y = point / 5;
                        MoveChess(idx, x, y);                
                        break;
                    }
                case "3":   //下棋:移动至棋子处
                    {
                        string[] Sidx = e.content.Split(',');
                        string Sidx1 = Sidx[0];
                        string Sidx2 = Sidx[1];
                        int idx1 = Convert.ToInt32(Sidx1);
                        int idx2 = Convert.ToInt32(Sidx2);
                        EatAction(idx1, idx2);                       
                        break;
                    }
                case "4": //胜利
                    {
                        string[] Sidx = e.content.Split(',');
                        string Sidx1 = Sidx[0];
                        string Sidx2 = Sidx[1];
                        int idx1 = Convert.ToInt32(Sidx1);
                        int idx2 = Convert.ToInt32(Sidx2);
                        EatAction(idx1, idx2);
                        MessageBox.Show("游戏结束。");
                        break;
                    }
            }
        }

    }
   
  
}
