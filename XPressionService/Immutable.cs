using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPressionService
{
    public class Immutable
    {
        public static long millisecondclock;
        const int DEFAULT_TIME = 900000;
        const int MS_SECOND = 1000;
        const int MS_MINUTE = 60000;

        const string LONG_CLOCK = @"mm\:ss";
        const string SHORT_CLOCK = @"ss\.ff";
        public static string DrawClockFormat = LONG_CLOCK;

        private static Immutable _instance = new Immutable();

        public static Graphic Engine;

        public static Dictionary<string, XPression.xpScene> Scenes;

        public static MultimediaTimer Timer;

        public static Action<long> ReverbAction;

        public static void Create()
        {
  
            _instance = new Immutable();
            Timer = new MultimediaTimer() { Interval = 100 };
            
        }
        private static void DrawClock()
        {
            //if (Clock.Enabled) return;

            DrawClockFormat = millisecondclock >= MS_MINUTE || millisecondclock <= 0 ? LONG_CLOCK : SHORT_CLOCK;
            TimeSpan ts = TimeSpan.FromMilliseconds(millisecondclock);
            Console.WriteLine(ts.ToString());

            //Clock.Invoke(new Action(() => { Clock.Text = ts.ToString(DrawClockFormat); }));
        }
        public static void SetupTimer(string SCORECLOCK_WIDGET)
        {
            if (Timer == null)
            {
                Timer = new MultimediaTimer() { Interval = 100 };
            }

                millisecondclock = DEFAULT_TIME;

                //DrawClock();

                Timer.Elapsed += (sender, args) =>
                {
                    millisecondclock -= Timer.Interval;
                    DrawClock();
                    try
                    {
                        Engine.SetClockWidgetValue(SCORECLOCK_WIDGET, (int)millisecondclock);
                    }
                    catch
                    {
                        Console.WriteLine("Can't find clockwidget '" + SCORECLOCK_WIDGET + "' in graphics xpression engine.");
                    }
                    if (millisecondclock <= 0)
                    {
                        millisecondclock = 0;

                        DrawClock();

                        Timer.Stop();
                    //if (toggle_play.Checked)
                    //{
                    //    toggle_play.Checked = false;
                    //}
                }
                };
                Timer.Elapsed += (sender, args) =>
                {
                    try
                    {
                        if (millisecondclock < MS_MINUTE && Engine.GetClockWidget(SCORECLOCK_WIDGET).Format != "S.Z")
                        {

                            Engine.GetClockWidget(SCORECLOCK_WIDGET).Format = "S.Z";


                        }
                        else if (millisecondclock >= MS_MINUTE && Engine.GetClockWidget(SCORECLOCK_WIDGET).Format != "N:SS")
                        {

                            Engine.GetClockWidget(SCORECLOCK_WIDGET).Format = "N:SS";

                            Console.WriteLine("Can't find clockwidget '" + SCORECLOCK_WIDGET + "' in graphics xpression engine.");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Can't find clockwidget '" + SCORECLOCK_WIDGET + "' in graphics xpression engine.");
                    }
                };

                try
                {
                    Engine.GetClockWidget(SCORECLOCK_WIDGET).Format = "N:SS";
                }
                catch
                {
                    Console.WriteLine("Can't find clock widget in xpression engine.");

                }
            
        }
    }
}
