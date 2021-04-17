using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace XPressionService
{
    [ServiceContract(Namespace= "XPressionService", SessionMode = SessionMode.Allowed, Name = "XPressionWCF", ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, CallbackContract = typeof(ITimerDuplexCallback))]
    public interface IXPressionWCF
    {
        [OperationContract]
        bool CreateEngine();
        [OperationContract]
        bool GetScene(string name, bool as_copy);
        [OperationContract]
        bool SetTakeMode(int index, bool mode);
        [OperationContract]
        bool SetTLWidgetValue(string name, string value);
        [OperationContract]
        bool SetTLWidgetList(string name, string[] value);
        [OperationContract]
        bool SetTLWidgetIndex(string name, int value);
        [OperationContract]
        bool SetClkWidgetValue(string name, int value);
        [OperationContract]
        bool SetClkWidgetFormat(string name, string value);
        [OperationContract]
        bool RunAnimationController(string scene, string name, bool forward = true);
        [OperationContract]
        bool RunSceneDirector(string scene, string name, int position);
        [OperationContract]
        bool SetCntWidgetValue(string name, int value);
        [OperationContract]
        bool ChangeMaterial(string name, string filename, int index = 0);
        [OperationContract]
        bool RunTimer(bool mode);
        [OperationContract]
        string GetSceneImage(string scene, int start, int height, int width);
        [OperationContract]
        bool IsSceneOnline(string scene);
        [OperationContract]
        bool SetSceneOnline(string scene, bool mode, int buffer);
        [OperationContract]
        bool SceneExists(string scene);
        [OperationContract]
        void CreateTimer(string clockwidget);
        [OperationContract]
        bool TimerStatus();
        [OperationContract]
        void PostServer(string message);
        [OperationContract]
        void SetReverbAction(Action<long> drawClock);
        [OperationContract]
        void SetImmutableTime(long millisecondclock);
    }
    
    public interface ITimerDuplexCallback
    {
        [OperationContract(IsOneWay = true)]
        void MillisecondCounter(long result);
    }
}
