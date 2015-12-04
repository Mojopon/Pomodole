using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public static class MessageResource
    {
        private static Language currentLanguage = Language.Japanese;

        public static string GetMessageFor(Message message)
        {
            switch(currentLanguage)
            {
                // return message for japanese users
                case Language.Japanese:
                    {
                        switch(message)
                        {
                            case Message.StartButton:               return "スタート";
                            case Message.LeftPomodoroSetMessage:    return "残り";
                            case Message.RightPomodoroSetMessage:   return "ポモドーロ";
                        }
                    }
                    return "";


                // return message for english users
                case Language.English:
                    {
                        switch(message)
                        {
                            case Message.StartButton: return "Start";
                            case Message.LeftPomodoroSetMessage: return "Long Break Until";
                            case Message.RightPomodoroSetMessage: return "Pomodoro";
                        }
                    }
                    return "";
                default: return "";
            }
        }
    }
}
