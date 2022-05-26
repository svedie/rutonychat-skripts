using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace RutonyChat
{
    public class Script
    {
        // writes the text in a file
        public void RunScript(string site, string username, string text, string param)
        {      
            // simple script, that writes the text in a file       
            // change the path
            string file = @"d:\stream\simonsays.txt";

            // easy way to filter the code word, just use the substring without the code wort
            // for example for "!say " use substring beginning at char 5
            string say = text.Substring(5);

            string simonsays = string.Format("simon says {0}", simonsays);

            RutonyBot.BotSay(site, simonsays);

            // clear the file and write new text in it
            RutonyBotFunctions.FileClear(file);
            RutonyBotFunctions.FileAddString(file, simonsays);
        }
    }
}