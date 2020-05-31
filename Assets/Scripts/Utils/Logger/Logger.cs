using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    class Logger { 
    

        public Logger() { 

        }

        public void setFileName(string file)
        {
            fileName = file + DateTime.Now.Ticks + ".txt";
            using (StreamWriter newFile = File.CreateText(fileName))
            {
            }                
        }


        public void printToFileAndConsole(string content)
        {
            printToFile(content);
            printToConsole(content);
        }

        public void printToConsole(string content)
        {
            Debug.Log(content);
        }

        public void printToFile(string content)
        {
            if (fileName != null)
            {
                using (StreamWriter file = File.AppendText(fileName))
                {
                    file.WriteLine(content);
                }
            }
        }

        private string fileName;
    }
}
