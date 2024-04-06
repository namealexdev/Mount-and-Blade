namespace ConsoleApp3
{


    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    class IniFile   // revision 11
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            //detect is path exist folders -> (and exist) create it
            if (IniPath.Contains("/"))
            {
                string folders = IniPath.Substring(0, IniPath.LastIndexOf("/") + 1);
                if (!Directory.Exists(folders))
                {
                    Directory.CreateDirectory(folders);
                }
            }
            
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = new IniFile("./translet/ru/text/ru.ini");
            string lang = settings.Read("selected lang", "Lang");
            if (!string.IsNullOrEmpty(lang))
            {
                Console.WriteLine($"selected lang: {lang}");
            }
            else
            {
                settings.Write("selected lang", "ru", "Lang");
                Console.WriteLine("write lol");
            }
            
        }
    }
}
