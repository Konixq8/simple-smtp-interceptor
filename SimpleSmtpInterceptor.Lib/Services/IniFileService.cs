using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleSmtpInterceptor.Lib.Services
{
    //Copied from      : https://stackoverflow.com/questions/217902/reading-writing-an-ini-file
    //This exact answer: https://stackoverflow.com/a/14906422/603807
    public class IniFileService   // revision 11
    {
        private readonly string _path;
        private readonly string _executableName = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        //The INI path is expected to be the full file path including the file name and extension
        public IniFileService(string iniPath = null)
        {
            //If the provided path is null then provide a default file name that will be located in the bin folder
            _path = iniPath ?? new FileInfo(_executableName + ".ini").FullName;
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? _executableName, Key, "", RetVal, 255, _path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? _executableName, Key, Value, _path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? _executableName);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? _executableName);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}