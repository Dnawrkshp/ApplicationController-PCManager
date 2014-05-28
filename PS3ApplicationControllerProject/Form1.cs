using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

using ZLibNet;
using WindowsInput;
using ScreenShotDemo;
using DoctaJonez.Drawing.Imaging;

namespace PS3ApplicationControllerProject
{
    /*
     * Application Controller PS3
     * Developers:
     *      Dnawrkshp, 
     *      
     * Hopefully that short list will be added to in the future.
     * 
     * The purpose of this application is to
     *  - Allow the PS3 to control the PC via PS3 controller(s)
     *  - Display the current screen or application window at high quality on the TV with little lag
     * 
     */


    public partial class Form1 : Form
    {
        /*
         * These are the header definitions
         * Set as the first byte in the header
         */
        public const byte requestPad = 1;               // Requests the paddata of a controller. Index/Controller port is specified in header[1]
        public const byte updateImage = 2;              // Informs that an uncompressed jpg image will be sent. Specify size in header
        public const byte updateImageZLIB = 3;          // Informs that a compressed jpg image will be sent. Both the size of the raw image and the compressed are set in the header
        public const byte updateShowFPS = 4;            // Informs the PS3 of whether to display the FPS, bool set in header[1]
        public const byte exitToXMB = 5;                // Informs the PS3 to exit to XMB, closes connection
        public const byte resetToListen = 6;            // Informs the PS3 to disconnect and wait for a new connection from the PC

        public const int PAD_ANALOG_RightDown = 2;      // Signifies whether the analog stick is directed Right or Down
        public const int PAD_ANALOG_LeftUp = 1;         // Signifies whether the analog stick is directed Left or Up

        public static string IPAddress = "";            // Global IP Address of PS3

        public static string settFile = ".acps3.ini";   // Path of the settings file (PC)

        // Array of 24 keyInputIC user controls. Used for the 24 different inputs
        public static keyInputUC[] keyInputs = new keyInputUC[24];
        // Array of the 4 checkboxes stating whether each of the right analog stick's direction should affect the mouse
        public static CheckBox[] mouseCB = new CheckBox[4];

        // Holds the input info for the controller
        static padData Pad = new padData();

        static bool readPad = true;                     // A boolean stating whether the pad will be requested and inputted
        static bool sendScreen = true;                  // A boolean stating whether to send the PC's screen to the PS3
        static bool showFPS = false;                    // A boolean stating whether the FPS will be printed on the screen or sent via TTP
        static bool threadShowFPS = false;              // A boolean stating the current thread showFPS state. Since I can't use CheckedChanged event

        static bool closeThreads = false;               // A boolean set to true when the app is closing. Tells the threads to exit

        /*
         * Client used to connect and communicate with the PS3
         * 
         * Go to properties, Build, and Conditional compilation symbols.
         * Remove SOCKET_UDP if it you want TCP.
         * 
         * At this point UDP on the PS3 side isn't added. But when it is be sure to make use of this so TCP can still be supported.
         */

#if SOCKET_UDP
        static UdpClient padUDPClient = new UdpClient();
#else
        static TcpClient padTCPClient = new TcpClient();
#endif

        public Form1()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(this_FormClosing);
        }

        #region Pad Parsing

        /*
         * Takes the key index and returns the corresponding button/analog state
         */
        bool parsePadKey(int index)
        {
            switch (index)
            {
                case 0: //DPad Up
                    return Pad.isUp;
                case 1: //DPad Right
                    return Pad.isRight;
                case 2: //DPad Down
                    return Pad.isDown;
                case 3: //DPad Left
                    return Pad.isLeft;

                case 4: //R1
                    return Pad.isR1;
                case 5: //R2
                    return Pad.isR2;
                case 6: //R3
                    return Pad.isR3;
                case 7: //L1
                    return Pad.isL1;
                case 8: //L2
                    return Pad.isL2;
                case 9: //L3
                    return Pad.isL3;

                case 10: //Triangle
                    return Pad.isTriangle;
                case 11: //Circle
                    return Pad.isCircle;
                case 12: //Cross
                    return Pad.isCross;
                case 13: //Square
                    return Pad.isSquare;

                case 14: //Start
                    return Pad.isStart;
                case 15: //Select
                    return Pad.isSelect;

                case 16: //Left Analog Left
                    return Pad.isANA_L_LR == PAD_ANALOG_LeftUp;
                case 17: //Left Analog Right
                    return Pad.isANA_L_LR == PAD_ANALOG_RightDown;
                case 18: //Left Analog Up
                    return Pad.isANA_L_UD == PAD_ANALOG_LeftUp;
                case 19: //Left Analog Down
                    return Pad.isANA_L_UD == PAD_ANALOG_RightDown;

                case 20: //Right Analog Left
                    return Pad.isANA_R_LR == PAD_ANALOG_LeftUp;
                case 21: //Right Analog Right
                    return Pad.isANA_R_LR == PAD_ANALOG_RightDown;
                case 22: //Right Analog Up
                    return Pad.isANA_R_UD == PAD_ANALOG_LeftUp;
                case 23: //Right Analog Down
                    return Pad.isANA_R_UD == PAD_ANALOG_RightDown;
            }
            return false;
        }

        /*
         * Takes the key index and returns the new mouse location (right analog stick only)
         */
        Point parseNewMouse(int index)
        {
            switch (index)
            {
                case 20:
                    return new Point(Cursor.Position.X - 10, Cursor.Position.Y);
                case 21:
                    return new Point(Cursor.Position.X + 10, Cursor.Position.Y);
                case 22:
                    return new Point(Cursor.Position.X, Cursor.Position.Y - 10);
                case 23:
                    return new Point(Cursor.Position.X, Cursor.Position.Y + 10);
            }

            return Cursor.Position;
        }

        /*
         * Takes the Pad variable and the user config and sends the corresponding keys to the focused application with the SendKeys API
         * The inputted variable is used to determine if the key is already down
         */
        static List<int> inputted = new List<int>();
        InputSend IS = new InputSend();
        void ParsePad()
        {
            int x = 0;

            //inputted.Clear();

            for (x = 0; x < 24; x++)
            {
                bool state = parsePadKey(x), contains = inputted.Contains(keyInputs[x].keyInt);
                if (keyInputs[x].keyInt != 0 && !keyInputs[x].isMouse)
                {
                    if (state && !contains)
                    {
                        //InputSimulator.SimulateKeyPress(keyInputs[x].vKeyCode);
                        //InputSimulator.SimulateKeyDown(keyInputs[x].vKeyCode);
                        //IS.SendKeyDown(keyInputs[x].sKeyCode);
                        IS.SendKeyAsInput((Keys)keyInputs[x].keyInt, true);
                        //System.Threading.Thread.Sleep(500);
                        //InputSimulator.SimulateKeyUp(keyInputs[x].vKeyCode);
                        inputted.Add(keyInputs[x].keyInt);
                    }
                    else if (!state && contains)
                    {
                        //InputSimulator.SimulateKeyUp(keyInputs[x].vKeyCode);
                        IS.SendKeyAsInput((Keys)keyInputs[x].keyInt, false);
                        //IS.SendKeyUp(keyInputs[x].sKeyCode);
                        inputted.Remove(keyInputs[x].keyInt);
                    }
                }
                else if (keyInputs[x].isMouse && state)
                {
                    Cursor.Position = parseNewMouse(x); //new Point(Cursor.Position.X - 10, Cursor.Position.Y);
                }
            }
        }

        #endregion

        #region Pad and PS3 Communication Threads

        /*
         * Thread used to take the pad info and convert it into input specified by the user
         */
        Thread ParsePadThread;
        void ParsePadThreadStart()
        {
            while (ParsePadThread.ThreadState == System.Threading.ThreadState.Background)
            {
                if (closeThreads)
                    return;

                if (readPad)
                {
                    ParsePad();
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        /*
         * Thread used to communicate with the PS3
         * This includes recieving pad states, send screen, ect
         */
        Thread PadThread;
        void PadThreadStart()
        {
            int padreadcnt = 0;

#if SOCKET_UDP
            if (!padUDPClient.Client.Connected)
            {
                try
                {
                    padUDPClient.Connect(IPAddress, 3000);
                    if (!padUDPClient.Client.Connected)
                    {
                        MessageBox.Show("Failed to Connect to the PS3!");
                        return;
                    }
                    File.WriteAllText(settFile, IPAddress);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                    padUDPClient = new UdpClient();
                    threadShowFPS = false;

                    return;
                }
            }
            
#else
            if (!padTCPClient.Connected)
            {
                try
                {
                    padTCPClient.Connect(IPAddress, 3000);
                    if (!padTCPClient.Connected)
                    {
                        MessageBox.Show("Failed to Connect to the PS3!");
                        return;
                    }
                    File.WriteAllText(settFile, IPAddress);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                    padTCPClient = new TcpClient();
                    threadShowFPS = false;

                    return;
                }
            }

            NetworkStream serverStream = padTCPClient.GetStream();
#endif

            ScreenCapture sc = new ScreenCapture();

            try
            {
                bool sendImage = false, sendImageZLIB = true;
                while (PadThread.ThreadState == System.Threading.ThreadState.Background)
                {
                    if (closeThreads)
                        return;

                    float percentImage = 1;
                    Invoke((MethodInvoker)delegate
                    {
                        if (comboBox1.SelectedIndex == 0)
                            percentImage = .01f;
                        else
                        {
                            percentImage = comboBox1.SelectedIndex * 0.05f;
                        }
                    });

                    byte[] header = new byte[8];

                    // Checks if the showFPS bool has changed. If so tell the PS3 the new state
                    if (showFPS != threadShowFPS)
                    {
                        threadShowFPS = showFPS;
                        
                        //Enable FPS on TV

                        header[0] = updateShowFPS;
                        header[1] = (byte)(threadShowFPS ? 1 : 0);
                        SendPacket(header);
                    }

                    // Requests the pad states from the PS3. Updated Pad variable to new states
                    if (readPad)
                    {
                        padreadcnt++;

                        if (padreadcnt > 0)
                        {
                            header[0] = requestPad;
                            header[1] = 0; //controller number
                            SendPacket(header);


                            byte[] c = ReadPacket(20);
                            Pad = padData.ByteAToPadData(c);

                            //Invoke((MethodInvoker)delegate
                            //{
                            //    textBox3.Text = Pad.PadDataToString();
                            //});
                            
                            //ParsePad();
                            padreadcnt = 0;
                        }
                        
                    }

                    // Sends an uncompressed jpeg to the PS3
                    if (sendImage && sendScreen)
                    {
                        Image img = Screenshot(); //sc.CaptureScreen();
                        //img = (Image)ImageUtilities.ResizeImage(img, 848, 512);
                        //byte[] imageB = ImageToByte(img, ImageFormat.Jpeg);
                        img = (Image)ImageUtilities.ResizeImage(img, (int)(848 * percentImage), (int)(512 * percentImage));
                        //img = (Image)ImageUtilities.ResizeImage(img, 848, 512);
                        byte[] imageB = ImageToByte(img, ImageFormat.Jpeg);

                        header[0] = updateImage;
                        byte[] size = BitConverter.GetBytes((int)imageB.Length);
                        Array.Reverse(size);
                        Array.Copy(size, 0, header, 4, 4);
                        SendPacket(header);

                        SendPacket(imageB);

                        waitForReply();
                    }

                    // Sends a compressed jpeg to the PS3
                    if (sendImageZLIB && sendScreen)
                    {
                        Image img = sc.CaptureScreen(); //Screenshot();
                        
                        //img = (Image)ImageUtilities.ResizeImage(img, 848, 512);
                        //byte[] imageB = ImageToByte(img, ImageFormat.Jpeg);
                        img = (Image)ImageUtilities.ResizeImage(img, (int)(848 * percentImage), (int)(512 * percentImage));
                        //img = (Image)ImageUtilities.ResizeImage(img, 848, 512);
                        byte[] imageB = ImageToByte(img, ImageFormat.Jpeg);
                        byte[] zlibCompress = ZLibCompressor.Compress(imageB);

                        header[0] = updateImageZLIB;
                        byte[] siz = BitConverter.GetBytes((int)imageB.Length);
                        Array.Reverse(siz);
                        Array.Copy(siz, 0, header, 4, 4);
                        siz = BitConverter.GetBytes((int)zlibCompress.Length);
                        Array.Reverse(siz);
                        Array.Copy(siz, 1, header, 1, 3);

                        SendPacket(header);

                        SendPacket(zlibCompress);

                        //We use this to verify that the PS3 is done updating the screen and can return to net communications
                        waitForReply();
                    }

                    System.Threading.Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                /*
                 * If an error occurs then it will be displayed to the user
                 * The connection will be ended
                 */

                MessageBox.Show(e.Message);

#if SOCKET_UDP
                padUDPClient.Close();
                padUDPClient = new UdpClient();
#else
                padTCPClient.Close();
                padTCPClient = new TcpClient();
#endif

                //showFPS = false;
                //readPad = true;
                //sendScreen = true;
                threadShowFPS = false;
            }
        }

        #endregion

        #region Networking Functions

        /*
         * Reads a byte[] of length size from the stream
         */
        public byte[] ReadPacket(int size)
        {
            byte[] ret = new byte[size];

#if SOCKET_UDP
            if (padUDPClient.Client.Connected)
            {
                padUDPClient.Client.Receive(ret);
            }
#else
            if (padTCPClient.Connected)
            {
                padTCPClient.GetStream().Read(ret, 0, size);
            }
#endif

            return ret;
        }

        /*
         * Sends data to the PS3
         */
        public void SendPacket(byte[] data)
        {
#if SOCKET_UDP
            if (padUDPClient.Client.Connected)
            {
                padUDPClient.Send(data, data.Length);
            }
#else
            if (padTCPClient.Connected)
            {
                padTCPClient.GetStream().Write(data, 0, data.Length);
            }
#endif
        }

        /*
         * Waits for the PS3 to send it { 'K', '\0' }
         */
        public void waitForReply()
        {
            byte[] wait = new byte[2];
            while (wait[0] != 'K')
            {
#if SOCKET_UDP
                padUDPClient.Client.Receive(wait);
#else
                padTCPClient.GetStream().Read(wait, 0, 2);
#endif
            }
        }

        #endregion

        #region Image Misc

        /*
         * Takes a screenshot of the whole screen
         * Not as fast as ScreenCapture's method (not tested, I just get higher FPS on PS3)
         */
        public Image Screenshot()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            }

            return (Image)bitmap;
        }

        /*
         * Encodes an image to format and converts it into a byte[]
         */
        public static byte[] ImageToByte(Image img, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, format);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        #endregion

        #region Control Events

        /*
         * If connected, it tells the PS3 to disconnect and go back to listening for a new connection
         * It then waits for all threads to close before closing itself
         */
        void this_FormClosing(object sender, FormClosingEventArgs e)
        {
#if SOCKET_UDP
            if (padUDPClient.Client.Connected)
            {
#else
            if (padTCPClient.Connected)
            {
#endif
                byte[] header = new byte[8];
                header[0] = resetToListen;
                SendPacket(header);

                closeThreads = true;

                while (ParsePadThread.ThreadState == System.Threading.ThreadState.Background &&
                    PadThread.ThreadState == System.Threading.ThreadState.Background)
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        /*
         * Takes the IP Address inputted by the user and initializes two threads
         * One interacts with the PS3 and the other takes the pad info and uses it for input
         */
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            IPAddress = ipBox.Text;
            if (IPAddress == "")
            {
                MessageBox.Show("Invalid IP Address!");
                return;
            }

            Pad = new padData();

            PadThread = new Thread(new ThreadStart(PadThreadStart));
            PadThread.IsBackground = true;
            PadThread.Start();

            ParsePadThread = new Thread(new ThreadStart(ParsePadThreadStart));
            ParsePadThread.IsBackground = true;
            ParsePadThread.Start();
        }

        /*
         * Initialization
         * 
         * Sets setting file path and adds the key input controls to form
         */
        private void Form1_Load(object sender, EventArgs e)
        {
            settFile = Application.StartupPath + "\\" + settFile;
            string[] sett = new string[0];
            if (File.Exists(settFile))
                sett = (string[])File.ReadLines(settFile).ToArray();

            if (sett.Length > 0)
                ipBox.Text = sett[0];

            comboBox1.SelectedIndex = 10;

            string[][] kiNames =
            {
                new string[] { "DP_U", "DPad Up" },
                new string[] { "DP_R", "DPad Right" },
                new string[] { "DP_D", "DPad Down" },
                new string[] { "DP_L", "DPad Left" },
                new string[] { "P_R1", "R1" },
                new string[] { "P_R2", "R2" },
                new string[] { "P_R3", "R3" },
                new string[] { "P_L1", "L1" },
                new string[] { "P_L2", "L2" },
                new string[] { "P_L3", "L3" },
                new string[] { "P_TR", "Triangle" },
                new string[] { "P_CI", "Circle" },
                new string[] { "P_CR", "Cross" },
                new string[] { "P_SQ", "Square" },
                new string[] { "P_ST", "Start" },
                new string[] { "P_SE", "Select" },

                new string[] { "A_L_H_L", "Left Analog Left" },
                new string[] { "A_L_H_R", "Left Analog Right" },
                new string[] { "A_L_V_U", "Left Analog Up" },
                new string[] { "A_L_V_D", "Left Analog Down" },
                new string[] { "A_R_H_L", "Right Analog Left" },
                new string[] { "A_R_H_R", "Right Analog Right" },
                new string[] { "A_R_V_U", "Right Analog Up" },
                new string[] { "A_R_V_D", "Right Analog Down" },
            };

            for (int x = 0; x < keyInputs.Length; x++)
            {
                keyInputs[x] = new keyInputUC();
                keyInputs[x].Name = kiNames[x][0];
                keyInputs[x].keyName.Text = kiNames[x][1];
                keyInputs[x].index = x;
                //if (sett.Length > (x + 1))
                //{
                //    try
                //    {
                //        int keyValue = int.Parse(sett[x + 1]);
                //        Keys key = (Keys)keyValue;
                //        keyInputs[x].vKeyCode = keyInputUC.ParseKeyCode(keyValue);
                //        keyInputs[x].keyValue.Text = key.ToString();
                //    }
                //    catch { }
                //}

                if (x >= 20)
                {
                    keyInputs[x].Location = new Point(5, (25 * (x - 8) + 15));
                    mouseCB[x - 20] = new CheckBox();
                    mouseCB[x - 20].Location = new Point(265, (25 * (x - 8) + 22));
                    mouseCB[x - 20].Text = "Mouse";
                    mouseCB[x - 20].Name = "MouseCB_" + x.ToString();
                    mouseCB[x - 20].AutoSize = true;
                    mouseCB[x - 20].Tag = x.ToString();
                    mouseCB[x - 20].CheckedChanged += new EventHandler(mouseCB_CheckedChanged);
                    groupBox1.Controls.Add(mouseCB[x - 20]);
                }
                else if (x >= 10)
                {
                    keyInputs[x].Location = new Point(265, (25 * (x - 10) + 15));
                }
                else
                {
                    keyInputs[x].Location = new Point(5, (25 * x) + 15);
                }

                groupBox1.Controls.Add(keyInputs[x]);
            }
        }

        private void mouseCB_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            int index = int.Parse(cb.Tag.ToString());
            keyInputs[index].isMouse = cb.Checked;
        }

        private void cbConInput_CheckedChanged(object sender, EventArgs e)
        {
            readPad = cbConInput.Checked;
        }

        private void cbScrShare_CheckedChanged(object sender, EventArgs e)
        {
            sendScreen = cbScrShare.Checked;
        }

        private void cbFPSCount_CheckedChanged(object sender, EventArgs e)
        {
            showFPS = cbFPSCount.Checked;
        }

        private void configSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "App Control Config Files (*.qacc)|*.qacc|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(fd.FileName);
                foreach (keyInputUC keyI in keyInputs)
                {
                    sw.WriteLine(keyI.keyInt.ToString() + " " + keyI.isMouse.ToString());
                }
                sw.Close();
            }
        }

        private void configLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "App Control Config Files (*.qacc)|*.qacc|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                int kval = 0;
                bool isMouse = false;

                string[] config = File.ReadAllLines(fd.FileName);

                for (int x = 0; x < keyInputs.Length; x++)
                {
                    string[] vals = config[x].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (vals.Length > 0)
                        kval = int.Parse(vals[0]);
                    if (vals.Length > 1)
                        isMouse = bool.Parse(vals[1]);
                    keyInputs[x].keyInt = kval;
                    keyInputs[x].isMouse = isMouse;
                    if (x >= 20)
                        mouseCB[x - 20].Checked = isMouse;
                    keyInputs[x].vKeyCode = keyInputUC.ParseKeyCode(kval);
                    //keyInputs[x].sKeyCode = keyInputUC.ParseScanKeyCode(kval);
                    keyInputs[x].keyValue.Text = ((Keys)kval).ToString();
                }

            }
        }

        
        private void toXMBButt_Click(object sender, EventArgs e)
        {
#if SOCKET_UDP
            if (padUDPClient.Client.Connected)
            {
#else
            if (padTCPClient.Connected)
            {
#endif
                byte[] header = new byte[8];
                header[0] = exitToXMB;
                SendPacket(header);
            }
        }

        #endregion

    }
}
