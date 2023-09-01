using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleProtoc
{
    enum LangType : ushort
    {
        cpp, java, python, ruby, objc, csharp, kotlin, php, pyi
    }

    class ProtocArgs
    {
        const string Default_Protoc = "protoc.exe";
        const string Name_Cpp = "C++";
        const string Name_Java = "Java";
        const string Name_Python = "Python";
        const string Name_PythonPyi = "Python(pyi)";
        const string Name_Ruby = "CRuby";
        const string Name_Objc = "Objective-C";
        const string Name_CSharp = "C#";
        const string Name_Kotlin = "Kotlin";
        const string Name_PHP = "PHP";

        public string PROTOC_PATH { get; set; } = "";
        public List<string> IMPORT_PATH { get; set; } = new();
        public List<string> FILES { get; set; } = new();
        public string TARGET_DIR { get; set; } = "";
        public string COMMON_OUTPUT_PATH { get; set; } = "";
        public Dictionary<string, bool> LANGS { get; set; } = new()
        {
            { Name_Cpp, false },
            { Name_Java, false },
            { Name_Python, false },
            { Name_PythonPyi, false },
            { Name_Ruby, false },
            { Name_Objc, false },
            { Name_CSharp, false },
            { Name_Kotlin, false },
            { Name_PHP, false },
        };
        public Dictionary<string, string> LANGS_PATH { get; set; } = new()
        {
            { Name_Cpp, "" },
            { Name_Java, "" },
            { Name_Python, "" },
            { Name_PythonPyi, "" },
            { Name_Ruby, "" },
            { Name_Objc, "" },
            { Name_CSharp, "" },
            { Name_Kotlin, "" },
            { Name_PHP, "" },
        };

        readonly static Dictionary<string, LangType> _langs = new() 
        {
            { Name_Cpp, LangType.cpp },
            { Name_Java, LangType.java },
            { Name_Python, LangType.python },
            { Name_PythonPyi, LangType.pyi },
            { Name_Ruby, LangType.ruby },
            { Name_Objc, LangType.objc },
            { Name_CSharp, LangType.csharp },
            { Name_Kotlin, LangType.kotlin },
            { Name_PHP, LangType.php },
        };

        public static Tuple<bool, string, string, string> GetCommandArgsAndInfo(ProtocArgs args)
        {
            bool target_dir_flag = !string.IsNullOrWhiteSpace(args.TARGET_DIR);

            StringBuilder c = new("");
            StringBuilder info = new("");

            string protocPath = string.IsNullOrWhiteSpace(args.PROTOC_PATH) ? $@"{Default_Protoc}" : $@"{args.PROTOC_PATH}";

            info.Append('\n');
            info.Append($@"Protoc Path: {protocPath}");
            info.Append('\n');

            string common_path = string.IsNullOrWhiteSpace(args.COMMON_OUTPUT_PATH) == true ? $"." : args.COMMON_OUTPUT_PATH;
            info.Append($"\nDefault Output Path: {((common_path == ".") ? $"Current Path" : common_path)}\n");

            info.Append($"\nSelected Langs: ");
            int cnt = 0;
            foreach (string key in args.LANGS.Keys)
            {
                if (args.LANGS[key] == true)
                {
                    string name = Enum.GetName(_langs[key]);
                    if (string.IsNullOrWhiteSpace(args.LANGS_PATH[key]) == true)
                    {
                        c.Append($"--{name}_out={common_path} ");
                        info.Append($"\n{key}: path=[{common_path}] (default)");
                    }
                    else
                    {
                        if (Directory.Exists(args.LANGS_PATH[key]) == false)
                        {
                            return new Tuple<bool, string, string, string>(false, "", $"There is no directory: {args.LANGS_PATH[key]}", "");
                        }
                        c.Append($"--{name}_out={args.LANGS_PATH[key]} ");
                        info.Append($"\n{key}: path=[{args.LANGS_PATH[key]}]");
                    }
                    
                    cnt++;
                }
            }

            if (cnt == 0)
            {
                return new Tuple<bool, string, string, string>(false, "", "No selected langs.", "");
            }

            if (target_dir_flag == true)
            {
                if (Directory.Exists(args.TARGET_DIR) == false)
                {
                    return new Tuple<bool, string, string, string>(false, "", $"Can not find TARGET_DIR: {args.TARGET_DIR}", protocPath);
                }

                info.Append($"\n\nFound Proto File List in: [{args.TARGET_DIR}]\n");

                string[] list = Directory.GetFiles(args.TARGET_DIR);
                foreach (string file in list)
                {
                    if (file.EndsWith(".proto") == true)
                    {
                        c.Append($"{file} ");
                        info.Append($"{file}\n");
                    }
                }
            }
            else
            {
                if (args.IMPORT_PATH.Count != 0)
                {
                    info.Append($"Target Directories\n");
                    foreach (string path in args.IMPORT_PATH)
                    {
                        c.Append($"--proto_path={path} ");
                        info.Append($"{path}\n");
                    }
                }

                info.Append($"Target Files\n");
                foreach (string file in args.FILES)
                {
                    c.Append($"{file} ");
                    info.Append($"{file}\n");
                }
            }

            return new Tuple<bool, string, string, string>(true, c.ToString(), info.ToString(), protocPath);
        }

        public static ProtocArgs GetDefault()
        {
            return new ProtocArgs
            {
                PROTOC_PATH = "",
                IMPORT_PATH = new List<string>(),
                FILES = new List<string>(),
                TARGET_DIR = "",
                COMMON_OUTPUT_PATH = "",
                LANGS = new Dictionary<string, bool>()
                {
                    { "cpp", false },
                    { "java", false },
                    { "python", false },
                    { "ruby", false },
                    { "objc", false },
                    { "csharp", false },
                    { "kotlin", false },
                    { "php", false },
                    { "pyi", false },
                },
            };
        }
    }
}
