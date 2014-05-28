using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS3ApplicationControllerProject
{
    /*
     * Info on pad button and analog states
     */

    class padData
    {

        public bool isCross = false;
        public bool isTriangle = false;
        public bool isCircle = false;
        public bool isSquare = false;

        public bool isLeft = false;
        public bool isUp = false;
        public bool isRight = false;
        public bool isDown = false;

        public bool isR1 = false;
        public bool isR2 = false;
        public bool isL1 = false;
        public bool isL2 = false;
        public bool isR3 = false;
        public bool isL3 = false;

        public bool isStart = false;
        public bool isSelect = false;

        public int isANA_L_LR = 0;
        public int isANA_L_UD = 0;

        public int isANA_R_LR = 0;
        public int isANA_R_UD = 0;

        public string PadDataToString()
        {
            string ret = "";

            ret += "isCross: " + isCross.ToString() + "\r\n";
            ret += "isTriangle: " + isTriangle.ToString() + "\r\n";
            ret += "isCircle: " + isCircle.ToString() + "\r\n";
            ret += "isSquare: " + isSquare.ToString() + "\r\n";

            ret += "isLeft: " + isLeft.ToString() + "\r\n";
            ret += "isUp: " + isUp.ToString() + "\r\n";
            ret += "isRight: " + isRight.ToString() + "\r\n";
            ret += "isDown: " + isDown.ToString() + "\r\n";

            ret += "isR1: " + isR1.ToString() + "\r\n";
            ret += "isR2: " + isR2.ToString() + "\r\n";
            ret += "isL1: " + isL1.ToString() + "\r\n";
            ret += "isL2: " + isL2.ToString() + "\r\n";

            ret += "isR3: " + isR3.ToString() + "\r\n";
            ret += "isL3: " + isL3.ToString() + "\r\n";
            ret += "isStart: " + isStart.ToString() + "\r\n";
            ret += "isSelect: " + isSelect.ToString() + "\r\n";

            ret += "Left_Analog_Hor: " + isANA_L_LR.ToString() + "\r\n";
            ret += "Left_Analog_Ver: " + isANA_L_UD.ToString() + "\r\n";
            ret += "Right_Analog_Hor: " + isANA_R_LR.ToString() + "\r\n";
            ret += "Right_Analog_Ver: " + isANA_R_UD.ToString() + "\r\n";

            return ret;
        }

        /*
         * Converts a byte[] of length 20 to an instance of padData
         */
        public static padData ByteAToPadData(byte[] chars)
        {
            padData ret = new padData();

            if (chars.Length != 20)
                return ret;

            ret.isCross = chars[0] != 0;
            ret.isTriangle = chars[1] != 0;
            ret.isCircle = chars[2] != 0;
            ret.isSquare = chars[3] != 0;
            ret.isLeft = chars[4] != 0;
            ret.isUp = chars[5] != 0;
            ret.isRight = chars[6] != 0;
            ret.isDown = chars[7] != 0;
            ret.isR1 = chars[8] != 0;
            ret.isR2 = chars[9] != 0;
            ret.isL1 = chars[10] != 0;
            ret.isL2 = chars[11] != 0;
            ret.isR3 = chars[12] != 0;
            ret.isL3 = chars[13] != 0;
            ret.isStart = chars[14] != 0;
            ret.isSelect = chars[15] != 0;
            ret.isANA_L_LR = chars[16];
            ret.isANA_L_UD = chars[17];
            ret.isANA_R_LR = chars[18];
            ret.isANA_R_UD = chars[19];
            

            return ret;
        }

    }
}
