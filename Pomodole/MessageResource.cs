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

        public static string GetMessageFor(Message message, params string[] elements)
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
                            case Message.DisplayPomodoroSetMessage:
                                return string.Format("残り {0} ポモドーロ", elements[0]);
                            case Message.AlmostLongBreakMessage:
                                return "この後小休止";
                            case Message.LongBreakMessage:
                                return "小休止中";
                            case Message.StartTaskMessage:
                                return "タスクを開始";
                            case Message.StartBreakMessage:
                                return "休憩を開始";
                            case Message.StartLongBreakMessage:
                                return "小休止を開始";
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
                            case Message.DisplayPomodoroSetMessage:
                                return string.Format("{0} Pomodoros Left", elements[0]);
                            case Message.AlmostLongBreakMessage:
                                return "Long Break After This";
                            case Message.LongBreakMessage:
                                return "Taking Long Break";
                            case Message.StartTaskMessage:
                                return "Start Task";
                            case Message.StartBreakMessage:
                                return "Take a break";
                            case Message.StartLongBreakMessage:
                                return "Take a long break";
                        }
                    }
                    return "";
                default: return "";
            }
        }
    }
}
