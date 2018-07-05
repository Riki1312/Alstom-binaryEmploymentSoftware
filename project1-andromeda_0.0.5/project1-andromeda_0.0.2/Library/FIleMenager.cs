using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

/*
 * Funzioni:                                               Return:          Descrizione:
 * 
 *                                  
 * ReadFile (string path)                                  string           Tutto il contenuto
 * ReadFileLines (string path)                             string[]         Tutte le linee
 * 
 * WriteFile (string path, string content, bool append)    void             True se è andata a buon fine
 * WriteFile (string path, string[] content, bool append)  void             True se è andata a buon fine
 * 
 * SelectFile (string[] extensions)                        string           Percorso del file
 * SaveFile ()                                             string           Percorso del file
 * 
 */

namespace CustomLibrary
{
    static class FileMenager
    {
        static public string ReadFile(string path)
        {
            StreamReader file = new StreamReader(path);
            return file.ReadToEnd();
        }

        static public List<string> ReadFileLines(string path)
        {
            StreamReader file = new StreamReader(path);
            List<string> lines = new List<string>();

            while (!file.EndOfStream)
                lines.Add(file.ReadLine());
            file.Close();

            return lines;
        }

        static public string ReadFileAlternative(string path)
        {
            return new StreamReader(path).ReadToEnd();
        }

        static public List<string> ReadFileLinesAlternative(string path)
        {
            return File.ReadAllLines(path).ToList();
        }

        static public void WriteFile(string path, string content, bool append = false)
        {
            StreamWriter file = new StreamWriter(path, append);
            file.Write(content);
            file.Close();
        }

        static public void WriteFile(string path, string[] content, bool append = false)
        {
            StreamWriter file = new StreamWriter(path, append);

            for (int i = 0; i < content.Length; i++)
                file.WriteLine(content[i]);

            file.Close();
        }

        static public void WriteFileAlternative(string path, string content, bool append)
        {
            if(append)
                File.WriteAllText(path, content);
            else
                File.AppendAllText(path, content);
        }

        static public string SelectFile(string[] extensionsName, string[] extensionsType, string initialDirectory = @"C:\")
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = initialDirectory;
            ofd.Filter = CalculateExtensions(extensionsName, extensionsType);
            var result = ofd.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(ofd.FileName))
                return ofd.FileName;
            return null;
        }

        static public string SaveFile(string name, string content, string[] extensionsName, string[] extensionsType, string initialDirectory = @"C:\")
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = name;
            sfd.InitialDirectory = initialDirectory;
            sfd.Filter = CalculateExtensions(extensionsName, extensionsType);
            var result = sfd.ShowDialog();

            if (result == true)
            {
                StreamWriter writer = new StreamWriter(sfd.OpenFile());
                writer.Write(content);
                writer.Close();

                return sfd.FileName;
            }
            else
                return null;
        }

        static private string CalculateExtensions(string[] extensionsName, string[] extensionsType)
        {
            string extensions = null;
            for (int i = 0; i < extensionsName.Length; i++)
                extensions += extensionsName[i] + "|*." + extensionsType[i] + ((i < extensionsName.Length - 1) ? "|" : "");
            return extensions;
        }
    }
}
