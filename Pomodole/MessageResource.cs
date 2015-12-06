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
                            case Message.MainButtonStartmessage:
                                return "スタート";
                            case Message.MainButtonStopMessage:
                                return "ストップ";
                            case Message.MainButtonResumeMessage:
                                return "再開";
                            case Message.LeftPomodoroSetMessage:
                                return "小休止まで";
                            case Message.RightPomodoroSetMessage:
                                return "ポモドーロ";
                            case Message.AlmostLongBreakMessage:
                                return "この後小休止";
                            case Message.LongBreakMessage:
                                return "小休止中";
                        }
                    }
                    return "";


                // return message for english users
                case Language.English:
                    {
                        switch(message)
                        {
                            case Message.MainButtonStartmessage:
                                return "Start";
                            case Message.MainButtonStopMessage:
                                return "Stop";
                            case Message.MainButtonResumeMessage:
                                return "Resume";
                            case Message.LeftPomodoroSetMessage:
                                return "Long Break Until";
                            case Message.RightPomodoroSetMessage:
                                return "Pomodoro";
                            case Message.AlmostLongBreakMessage:
                                return "Long Break After This";
                            case Message.LongBreakMessage:
                                return "Taking Long Break";
                        }
                    }
                    return "";
                default: return "";
            }
        }
    }
}
