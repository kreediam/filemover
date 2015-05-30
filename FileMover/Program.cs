using System;
using System.Collections.Generic;
using System.IO;

namespace X
{
    class FileMover
    {
        const string source = @"F:\";
        const string dest = @"C:\Old Drives\Rogan\";
        const string include = "txt,jpg,doc,docx,xls,xlsx,bmp,mp3,wma,m4a";
        const string exclude = "bat,ini,sys,com,log,dat,exe";

        Dictionary<string, bool> extensions;

        static void Main(string[] args)
        {
            FileMover fileMover = new FileMover();
            fileMover.ProcessDir(new DirectoryInfo(source), false);
        }

        public FileMover()
        {
            this.extensions = new Dictionary<string, bool>();

            foreach (string str in include.Split(','))
            {
                this.extensions["." + str] = true;
            }

            foreach (string str in exclude.Split(','))
            {
                this.extensions["." + str] = false;
            }
        }

        private void ProcessDir(DirectoryInfo dir, bool prompt = true)
        {
            try
            {
                bool shouldProcess = true;

                if (prompt)
                {
                    Console.Write("Do you want to include dir {0} ([Y]/N): ", dir.FullName);
                    shouldProcess = this.ResponseIsAffirm();
                }

                if (shouldProcess)
                {
                    ProcessDirFiles(dir);

                    foreach (var subdir in dir.GetDirectories())
                    {
                        this.ProcessDir(subdir);
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
        }

        private void ProcessDirFiles(DirectoryInfo dir)
        {
            try
            {
                FileInfo[] files = dir.GetFiles();
                bool shouldProcess = false;
                string extension, destination, destDir;

                foreach (FileInfo file in files)
                {
                    extension = file.Extension.ToLower();

                    if (this.extensions.ContainsKey(extension))
                    {
                        shouldProcess = this.extensions[extension];
                    }
                    else
                    {
                        Console.Write("Do you want to include extension {0} ([Y]/N): ", file.Extension);
                        shouldProcess = this.ResponseIsAffirm();
                        this.extensions[extension] = shouldProcess;
                    }

                    if (shouldProcess)
                    {
                        destination = file.FullName.Replace(source, dest);
                        destDir = Path.GetDirectoryName(destination);

                        Directory.CreateDirectory(destDir);
                        file.MoveTo(destination);
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
        }

        private bool ResponseIsAffirm()
        {
            string response = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(response))
            {
                return true;
            }
            else
            {
                return string.Equals(response.Substring(0, 1), "y", StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
