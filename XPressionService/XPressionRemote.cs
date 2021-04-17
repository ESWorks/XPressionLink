using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPression;
using XPressionService.Properties;

namespace XPressionService
{
    [Serializable]
    public delegate void MessageHandler(long message);

    public interface IXPressionRemote
    {

        bool CreateEngine();

        bool GetScene(string name, bool as_copy);

        bool SetTakeMode(int index, bool mode);

        bool SetTLWidgetValue(string name, string value);

        bool SetTLWidgetList(string name, string[] value);

        bool SetTLWidgetIndex(string name, int value);

        bool SetClkWidgetValue(string name, int value);

        bool SetClkWidgetFormat(string name, string value);

        bool RunAnimationController(string scene, string name, bool forward = true);

        bool RunSceneDirector(string scene, string name, int position);

        bool SetCntWidgetValue(string name, int value);

        bool ChangeMaterial(string name, string filename, int index = 0);

        bool RunTimer(bool mode);

        string GetSceneImage(string scene, int start, int height, int width);

        bool IsSceneOnline(string scene);

        bool SetSceneOnline(string scene, bool mode, int buffer);

        bool SceneExists(string scene);

        void CreateTimer(string clockwidget);

        bool TimerStatus();

        void PostServer(string message);

        void SetReverbAction(MessageHandler drawClock);
    }

    public class XPressionRemote : MarshalByRefObject, IXPressionRemote
    {
        public bool ChangeMaterial(string name, string filename, int index = 0)
        {
            try
            {
                Immutable.Engine.ChangeMaterial(name, filename, index);
                return true;
            }
            catch
            {
                Console.WriteLine("Can't find material '" + name + "' shader object '" + index + "' in graphics xpression engine.");
                return false;
            }
        }

        public bool CreateEngine()
        {
            try
            {
                Immutable.Engine = new Graphic();
                return true;
            }
            catch
            {
                Console.WriteLine("XPression Not Found.");
                return false;
            }
        }

        public bool GetScene(string name, bool as_copy = true)
        {
            try
            {
                if (Immutable.Engine != null)
                {
                    if (Immutable.Scenes == null) Immutable.Scenes = new Dictionary<string, XPression.xpScene>();
                    xpScene scene = Immutable.Engine.GetSceneObject(name, as_copy);
                    Immutable.Scenes[name] = scene;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public string ImageToString(Bitmap img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();
                byteArray = stream.ToArray();
            }
            return Convert.ToBase64String(byteArray);
        }
        public string GetSceneImage(string scene, int start, int height, int width)
        {
            try
            {
                if (Immutable.Scenes.ContainsKey(scene))
                {
                    return ImageToString(Immutable.Engine.GetSceneImage(start, height, width, scene));
                }
                else
                {
                    if(GetScene(scene))
                    {
                        return ImageToString(Immutable.Engine.GetSceneImage(start, height, width, scene));
                    }
                    else
                    {
                        return ImageToString(Resources.Error);
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ImageToString(Resources.Error);
        }

        public bool IsSceneOnline(string scene)
        {
            if(Immutable.Scenes.ContainsKey(scene))
            {
                return Immutable.Scenes[scene].IsOnline;
            }
            else
            {
                if(GetScene(scene))
                {
                    return Immutable.Scenes[scene].IsOnline;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool RunAnimationController(string scene, string name, bool forward = true)
        {
            try
            {
                if (!Immutable.Scenes.ContainsKey(scene)) GetScene(scene);
                xpAnimController animator = Immutable.Engine.GetAnimator(Immutable.Scenes[scene], name);
                Immutable.Engine.PlayAnimationDirector(animator, forward?PlayDirection.pd_Forward:PlayDirection.pd_Backward);
                return true;
            }
            catch
            {
                Console.WriteLine("Can't find scene '" + scene + "' animation controller '" + name + "' in graphics xpression engine.");
                return false;
            }
        }

        public bool RunSceneDirector(string scene, string name, int direction = 0)
        {
            try
            {
                if (!Immutable.Scenes.ContainsKey(name)) GetScene(name);
                xpSceneDirector animator = Immutable.Engine.GetSceneDirector(name);
                animator.Position = direction;
                animator.Play();
                return true;
            }
            catch
            {
                Console.WriteLine("Can't find scene '" + name + "' scene director in graphics xpression engine.");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns>If it is running.</returns>
        public bool RunTimer(bool mode)
        {
            if (Immutable.Timer == null) Immutable.Timer = new MultimediaTimer() { Interval = 100 };

            if (mode)
            {
                Immutable.Timer.Start();
            }
            else
            {
                Immutable.Timer.Stop();
            }
            return Immutable.Timer.IsRunning;
        }

        public bool SceneExists(string scene)
        {
            return Immutable.Engine.GetSceneObject(scene) != null;
        }

        public bool SetSceneOnline(string scene, bool mode, int buffer)
        {
            if (Immutable.Scenes.ContainsKey(scene))
            {

                return mode ? Immutable.Scenes[scene].SetOnline(buffer) : Immutable.Scenes[scene].SetOffline();
            }
            else
            {
                if (GetScene(scene))
                {
                    return mode ? Immutable.Scenes[scene].SetOnline(buffer) : Immutable.Scenes[scene].SetOffline();
                }
                else
                {
                    return false;
                }
            }
        }

        public bool SetClkWidgetFormat(string name, string value)
        {
            try
            {
                xpBaseWidget baseWidget;
                if(Immutable.Engine._engine.GetWidgetByName(name, out baseWidget))
                {
                    xpClockTimerWidget clock = (xpClockTimerWidget)baseWidget;
                    clock.Format = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("Can't find widget '" + name + "' in graphics xpression engine.");
                return false;
            }
            
        }

        public bool SetClkWidgetValue(string name, int value)
        {
            try
            {
                Immutable.Engine.SetClockWidgetValue(name, value);
                return true;
            }
            catch
            {
                Console.WriteLine("Can't find widget '" + name + "' in graphics xpression engine.");
                return false;
            }
        }

        public bool SetCntWidgetValue(string name, int value)
        {
            try
            {
                Immutable.Engine.SetCounterWidgetValue(name, value);
                return true;
            }
            catch
            {
                Console.WriteLine("Can't find widget '" + name + "' in graphics xpression engine.");
                return false;
            }
        }

        public bool SetTakeMode(int index, bool mode)
        {
            try
            {
                Immutable.Engine.SetTakeMode(index, mode);
                return true;
            }
            catch
            {
                Console.WriteLine("Can't find Take Item '" + index + "' in graphics xpression engine.");
                return false;
            }
        }

        public bool SetTLWidgetIndex(string name, int value)
        {
            try
            {
                xpBaseWidget baseWidget;
                if (Immutable.Engine._engine.GetWidgetByName(name, out baseWidget))
                {
                    xpTextListWidget clock = (xpTextListWidget)baseWidget;
                    clock.ItemIndex = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("Can't find widget '" + name + "' in graphics xpression engine.");
                return false;
            }
        }

        public bool SetTLWidgetList(string name, string[] value)
        {
            try
            {
                xpBaseWidget baseWidget;
                if (Immutable.Engine._engine.GetWidgetByName(name, out baseWidget))
                {
                    xpTextListWidget clock = (xpTextListWidget)baseWidget;
                    clock.ClearStrings();
                    value.Select(T => clock.AddString(T));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("Can't find widget '" + name + "' in graphics xpression engine.");
                return false;
            }
        }

        public bool SetTLWidgetValue(string name, string value)
        {
            try
            {
                return Immutable.Engine.SetTextListWidgetValue(name, value) == null;
            }
            catch
            {
                Console.WriteLine("Can't find widget '" + name + "' in graphics xpression engine.");
                return false;
            }
        }

        public void CreateTimer(string clockwidget)
        {
            Immutable.SetupTimer(clockwidget);
        }

        public bool TimerStatus()
        {
            return Immutable.Timer.IsRunning;
        }

        public void PostServer(string message)
        {
            Console.WriteLine(message);
        }
        
        public void SetReverbAction(MessageHandler drawClock)
        {
            Immutable.Timer.Elapsed += (s,e) => { drawClock(Immutable.millisecondclock); };
        }
    }
}
