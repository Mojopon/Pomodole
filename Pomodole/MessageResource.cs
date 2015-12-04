using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public static class MessageResource
    {
        private static Language currentLanguage = Language.English;

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
                            case Message.PomodoroLeftSetMessage:    return "残り";
                            case Message.PomodoroRightSetMessage:   return "ポモドーロ";
                        }
                    }
                    return "";


                // return message for english users
                case Language.English:
                    {
                        switch(message)
                        {
                            case Message.StartButton: return "Start";
                            case Message.PomodoroLeftSetMessage: return "Long Break Until";
                            case Message.PomodoroRightSetMessage: return "Pomodoro";
                        }
                    }
                    return "";
                default: return "";
            }
        }
    }
}
