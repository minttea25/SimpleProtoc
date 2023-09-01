using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace SimpleProtoc
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonPath = "args.json";

            if (File.Exists(jsonPath) == false)
            {
                var defaultProto = new ProtocArgs();
                string text = JsonSerializer.Serialize(defaultProto, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                File.WriteAllText(jsonPath, text);
                Console.WriteLine("Can not find file: {0}, It is created as new. Check the file.", jsonPath);
                return;
            }

            string jsonText = File.ReadAllText(jsonPath);
            ProtocArgs option;
            try
            {
                option = JsonSerializer.Deserialize<ProtocArgs>(jsonText);
            }
            catch (Exception)
            {
                Console.WriteLine($"The option file has error(s). Check the file: {jsonPath}");
                return;
            }

            var tuples = ProtocArgs.GetCommandArgsAndInfo(option);
            if (tuples.Item1 == false)
            {
                Console.WriteLine("{0}", tuples.Item3);
                return;
            }
            else if (string.IsNullOrEmpty(tuples.Item2) == true)
            {
                Console.WriteLine("No selected files. Check the {0}", jsonPath);
                return;
            }

            if (File.Exists(tuples.Item4) == false)
            {
                Console.WriteLine($@"Can not find '{tuples.Item4}'. Check the path or you can download from here: https://protobuf.dev/downloads/");
                return;
            }

            Console.WriteLine("Command: {0} {1}", tuples.Item4, tuples.Item2);
            Console.WriteLine("{0}", tuples.Item3);

            try
            {
                ProcessStartInfo proInfo = new ProcessStartInfo();
                Process pro = new Process();

                proInfo.FileName = $"{tuples.Item4}";
                proInfo.Arguments = tuples.Item2;
                proInfo.CreateNoWindow = false;
                proInfo.UseShellExecute = false;
                proInfo.RedirectStandardOutput = true;
                proInfo.RedirectStandardInput = true;
                proInfo.RedirectStandardError = true;

                pro.StartInfo = proInfo;
                pro.Start();

                string error = pro.StandardError.ReadToEnd();
                pro.StandardOutput.Close();
                pro.Dispose(); // instead of pro.Close();

                if (string.IsNullOrEmpty(error) == false)
                    Console.WriteLine($"Failed to exec 'protoc.exe'.\n{error}");
                else
                    Console.WriteLine("Completed successfully!");
            }
            catch(Exception)
            {
                Console.WriteLine($"An error occured during processing Process.");
            }
        }
    }
}
