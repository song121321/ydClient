using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace YDIOTSvr.YDIOTSvrUtil
{
    //public delegate void DataReceivedEventHandle(BaseReceive resultObj);
    public class CRCData
    {
        public byte CRCLow { get; set; }
        public byte CRCHigh { get; set; }
    }
    public class CRCDataCaculate
    {
        [DebuggerNonUserCode]
        public CRCDataCaculate()
        {
        }
        public static CRCData CRCCaculate(byte[] Data)
        {
            CRCData CRCV = new CRCData();
            long TempVal = 65535L;
            int arg_19_0 = 0;
            int num = Information.UBound(Data, 1);
            int i = arg_19_0;
            checked
            {
                while (true)
                {
                    int arg_86_0 = i;
                    int num2 = num;
                    if (arg_86_0 > num2)
                    {
                        break;
                    }
                    TempVal ^= (long)unchecked((ulong)Data[i]);
                    int j = 1;
                    int arg_75_0;
                    do
                    {
                        long YWval = (long)Math.Round(Conversion.Int((double)TempVal / 2.0));
                        int LSB = (int)(TempVal % 2L);
                        bool flag = LSB == 1;
                        if (flag)
                        {
                            YWval ^= 40961L;
                        }
                        TempVal = YWval;
                        j++;
                        arg_75_0 = j;
                        num2 = 8;
                    }
                    while (arg_75_0 <= num2);
                    i++;
                }
                double CRCValue = (double)(TempVal % 65536L);
                string CrcStr = Strings.Right("0000" + Conversion.Hex(CRCValue), 4);
                CRCV.CRCHigh = (byte)Math.Round(Conversion.Val("&H" + Strings.Right(CrcStr, 2)));
                CRCV.CRCLow = (byte)Math.Round(Conversion.Val("&H" + Strings.Left(CrcStr, 2)));
                return CRCV;
            }
        }
    }
}
