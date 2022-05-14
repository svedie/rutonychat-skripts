using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace RutonyChat
{
    // Script based on the internal Rutony CreditsQty which are associated with tickets.
    // 1 ticket = 1 song, the quantity can be adjusted on line 87
    public class Script
    {        
        public void RunScript(string site, string username, string text, string param)
        {   
            string Text_NotNumber = "{0}, the input must be a number!";
            string Text_NumberNotInList = "Number {0}: request song is not in the songlist!";
            string Text_AlreadyInList = "{0}, song is already in the request list or was already on stream!";
            string Text_SuccessfullyAdded = "{0}, request <{1}> added.";
            string Text_NoTickets = "{0}, you don't have enough tickets!";
            string Text_ToManyArguments = "{0}, please request only a song from the songlist!";

            // edit here the path to the file
            string FileName_Source = @"C:\edit-here-path-to-file\songlist.txt";
            string FileName_Requests = @"C:\edit-here-path-to-file\request.txt";
            string FileName_Requests_Stream = @"C:\edit-here-path-to-file\request_stream.txt";

            string[] arrText = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if(arrText.Length > 2)
            {
                // if user writes more arguments then needed
                RutonyBot.BotSay(site, string.Format(Text_ToManyArguments, username));
                return;
            }
            
            int songnumber = 0;
            
            // check, if via command and is number
            if(arrText.Length == 1) 
            {
                if (int.TryParse(arrText[0], out songnumber) == false) 
                {
                    RutonyBot.BotSay(site, string.Format(Text_NotNumber, username));
                    return;
                }
            } 
            //check, if via sr and is number
            else if(arrText.Length == 2) 
            {
                if (int.TryParse(arrText[1], out songnumber) == false) 
                {
                    RutonyBot.BotSay(site, string.Format(Text_NotNumber, username));
                    return;
                }
            }
            
            // read the count of lines in songlist
            int songnumbers = RutonyBotFunctions.FileLength(FileName_Source);
            // if greater then number of songs, exit
            if(songnumbers < songnumber) 
            {
                RutonyBot.BotSay(site, string.Format(Text_NumberNotInList, songnumber));
                return;
            }            

            string songname = RutonyBotFunctions.FileStringAt(FileName_Source, songnumber-1);
            string request = username + " - " + songname;

            // if already exist in request list, exit
            if (FileHasString(FileName_Requests_Stream, songname))
            {
                RutonyBot.BotSay(site, string.Format(Text_AlreadyInList, username));
                return;
            }

            // get user
            RankControl.ChatterRank cr = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());            
            if (cr != null) 
            {                
                // add to request song list, if credits are greater 0
                if(cr.CreditsQty > 0)
                {
                    RutonyBotFunctions.FileAddString(FileName_Requests, request);
                    RutonyBotFunctions.FileAddString(FileName_Requests_Stream, request);
                    RutonyBot.BotSay(site, string.Format(Text_SuccessfullyAdded, username, request));
                    // reduce the number of tickets by 1
                    cr.CreditsQty -= 1;
                } else {
                    RutonyBot.BotSay(site, string.Format(Text_NoTickets, username));
                }          
            }
        }
        
        private bool FileHasString(string filename, string text) 
        {
            try 
            {
                string[] lines = File.ReadAllLines(filename);

                foreach (string strLine in lines) 
                {
                    if (strLine.contains(text)) 
                    {
                        return true;
                    }
                }
            } catch 
            {
                return false;
            }

            return false;
        }
    }
}