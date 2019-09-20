using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Misc = ComunicationProtocol.Misc;

namespace FQC
{
    public class IniReader
    {
        private string _fileName;

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(
          string lpAppName,// 指向包含 Section 名称的字符串地址
          string lpKeyName,// 指向包含 Key 名称的字符串地址
          int nDefault,// 如果 Key 值没有找到，则返回缺省的值是多少
          string lpFileName
          );

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
          string lpAppName,// 指向包含 Section 名称的字符串地址
          string lpKeyName,// 指向包含 Key 名称的字符串地址
          string lpDefault,// 如果 Key 值没有找到，则返回缺省的字符串的地址
          StringBuilder lpReturnedString,// 返回字符串的缓冲区地址
          int nSize,// 缓冲区的长度
          string lpFileName
          );

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(
          string lpAppName,// 指向包含 Section 名称的字符串地址
          string lpKeyName,// 指向包含 Key 名称的字符串地址
          string lpString,// 要写的字符串地址
          string lpFileName
          );

        public IniReader(string filename)
        {
            _fileName = filename;
        }

        public int GetInt(string section, string key, int def)
        {
            return GetPrivateProfileInt(section, key, def, _fileName);
        }

        public string GetString(string section, string key, string def = "")
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, temp, 1024, _fileName);
            return temp.ToString();
        }

        public void ReadSettings()
        {
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC8, Misc.OcclusionLevel.N);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC8, Misc.OcclusionLevel.L);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC8, Misc.OcclusionLevel.C);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC8, Misc.OcclusionLevel.H);

            ReadGrasebyPumpPressureSettings(ProductID.GrasebyF8, Misc.OcclusionLevel.N);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyF8, Misc.OcclusionLevel.L);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyF8, Misc.OcclusionLevel.C);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyF8, Misc.OcclusionLevel.H);

            ReadGrasebyPumpPressureSettings(ProductID.Graseby2000, Misc.OcclusionLevel.L);
            ReadGrasebyPumpPressureSettings(ProductID.Graseby2000, Misc.OcclusionLevel.C);
            ReadGrasebyPumpPressureSettings(ProductID.Graseby2000, Misc.OcclusionLevel.H);

            ReadGrasebyPumpPressureSettings(ProductID.Graseby2100, Misc.OcclusionLevel.L);
            ReadGrasebyPumpPressureSettings(ProductID.Graseby2100, Misc.OcclusionLevel.C);
            ReadGrasebyPumpPressureSettings(ProductID.Graseby2100, Misc.OcclusionLevel.H);

            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC6T, Misc.OcclusionLevel.L);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC6T, Misc.OcclusionLevel.C);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC6T, Misc.OcclusionLevel.H);

            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC6, Misc.OcclusionLevel.L);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC6, Misc.OcclusionLevel.C);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyC6, Misc.OcclusionLevel.H);

            //ReadGrasebyPumpPressureSettings(ProductID.WZ50C6T, Misc.OcclusionLevel.L);
            //ReadGrasebyPumpPressureSettings(ProductID.WZ50C6T, Misc.OcclusionLevel.C);
            //ReadGrasebyPumpPressureSettings(ProductID.WZ50C6T, Misc.OcclusionLevel.H);

            ReadGrasebyPumpPressureSettings(ProductID.GrasebyF6, Misc.OcclusionLevel.L);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyF6, Misc.OcclusionLevel.C);
            ReadGrasebyPumpPressureSettings(ProductID.GrasebyF6, Misc.OcclusionLevel.H);

            //ReadGrasebyPumpPressureSettings(ProductID.GrasebyF6_2, Misc.OcclusionLevel.L);
            //ReadGrasebyPumpPressureSettings(ProductID.GrasebyF6_2, Misc.OcclusionLevel.C);
            //ReadGrasebyPumpPressureSettings(ProductID.GrasebyF6_2, Misc.OcclusionLevel.H);

            //ReadGrasebyPumpPressureSettings(ProductID.WZS50F6, Misc.OcclusionLevel.L);
            //ReadGrasebyPumpPressureSettings(ProductID.WZS50F6, Misc.OcclusionLevel.C);
            //ReadGrasebyPumpPressureSettings(ProductID.WZS50F6, Misc.OcclusionLevel.H);

            //ReadGrasebyPumpPressureSettings(ProductID.WZS50F6_2, Misc.OcclusionLevel.L);
            //ReadGrasebyPumpPressureSettings(ProductID.WZS50F6_2, Misc.OcclusionLevel.C);
            //ReadGrasebyPumpPressureSettings(ProductID.WZS50F6_2, Misc.OcclusionLevel.H);
        }

        public void ReadGrasebyPumpPressureSettings(ProductID pid, Misc.OcclusionLevel level)
        {
            string section = string.Empty;

            #region section
            switch (pid)
            {
                case ProductID.GrasebyC8:
                    section = "GrasebyC8";
                    break;
                case ProductID.GrasebyF8:
                    section = "GrasebyF8";
                    break;
                case ProductID.GrasebyC6:
                    section = "GrasebyC6";
                    break;
                case ProductID.Graseby2000:
                    section = "Graseby2000";
                    break;
                case ProductID.Graseby2100:
                    section = "Graseby2100";
                    break;
                case ProductID.GrasebyC6T:
                    section = "GrasebyC6T";
                    break;
                case ProductID.GrasebyF6:
                    section = "GrasebyF6";
                    break;
                default:
                    section = "GrasebyC6";
                    break;
            }
            #endregion

            string val = GetString(section, level.ToString()).Trim();
            string[] pressures = val.Split('~');
            if (pressures.Length != 2)
                return;
            float pMin = 0, pMax = 0;
            if (float.TryParse(pressures[0].Trim(), out pMin) && float.TryParse(pressures[1].Trim(), out pMax))
            {
                PressureManager.Instance().Add(pid, level, pMin, pMax);
            }
        }



    }
}