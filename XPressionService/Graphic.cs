using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XPression;

namespace XPressionService
{
    public class Graphic
    {

        public xpEngine _engine { get; private set; }

        public Graphic()
        {
            _engine = new xpEngine();
        }

        public static void AdjustComboBox(ComboBox box, object[] list)
        {
            box.Items.Clear();
            box.Items.AddRange(list);
        }

        public void SetTakeMode(int id, bool mode)
        {
            xpBaseTakeItem takeItem;
            xpTakeItem item = new xpTakeItem();
            
            if (_engine.Sequencer.GetTakeItemByID(id, out takeItem))
            {
                if (mode) {
                    takeItem.Execute();
                }
                else
                {
                    takeItem.SetOffline();
                }
            }
        }
        public void SetTextListValue(string text_name, int index)
        {
            xpTextListWidget textObj;
            xpBaseWidget baseObj;

            if (_engine.GetWidgetByName(text_name, out baseObj))
            {
                textObj = (xpTextListWidget)baseObj;
                textObj.ItemIndex = index;
            }
        }
        public xpPublishedObject[] GetPublishedObjects(int id)
        {
            
            xpBaseTakeItem baseItem;
            xpTakeItem takeItem;

            if (_engine.Sequencer.GetTakeItemByID(id, out baseItem))
            {
                takeItem = (xpTakeItem)baseItem;

                xpPublishedObject[] publishedObjs = new xpPublishedObject[takeItem.PublishedObjectCount];

                for (int i = 0; i < takeItem.PublishedObjectCount; i++)
                {
                    xpPublishedObject published;
                    if(takeItem.GetPublishedObject(i, out published))
                    {
                        publishedObjs[i] = published;
                    }
                }
                return publishedObjs;
            }
            return new xpPublishedObject[0];
        }

        public string[] GetProperties(xpPublishedObject pobject)
        {
            string[] properties = new string[pobject.PropertyCount];
            for (int i = 0; i < pobject.PropertyCount; i++)
            {
                string property_name;
                PropertyType type;
                if (pobject.GetPropertyInfo(i, out property_name, out type))
                {
                    properties[i] = i + ", " + property_name + ", " + type;
                }
            }
            return properties;
        }

        public string[] GetPublishedProperties(int id, string object_name, int index)
        {
            xpPublishedObject published;
            
            xpBaseTakeItem baseItem;
            xpTakeItem takeItem;
            
            if (_engine.Sequencer.GetTakeItemByID(id, out baseItem))
            {
                takeItem = (xpTakeItem)baseItem;
                if (takeItem.GetPublishedObjectByName(object_name, out published))
                {
                    string[] properties = new string[published.PropertyCount];
                    for (int i = 0; i < published.PropertyCount; i++)
                    {
                        string property_name;
                        PropertyType type;
                        if (published.GetPropertyInfo(i, out property_name, out type))
                        {
                            properties[i] = i + ", " + property_name + ", " + type;
                        }
                    }
                    return properties;
                }
            }
            return new string[0];
        }

        public xpScene SetTextSceneValue(string scene_name, string text_name, string text_value, bool as_copy = true)
        {
            xpTextObject textObj;
            xpBaseObject baseObj;
            xpScene _scene;
            if (_engine.GetSceneByName(scene_name, out _scene, as_copy))
            {
               
                if (_scene.GetObjectByName(text_name, out baseObj))
                {
                    textObj = (xpTextObject)baseObj;
                    textObj.Text = text_value;
                   // textObj.BeginUpdate();
                    return _scene;
                }
            }
            return _scene;
        }
        public xpTextObject SetTextSceneValue(xpScene scene_name, string text_name, string text_value)
        {
            xpTextObject textObj;
            xpBaseObject baseObj;
            if (scene_name.GetObjectByName(text_name, out baseObj))
            {
                textObj = (xpTextObject)baseObj;
                textObj.Text = text_value;
                // textObj.BeginUpdate();
                return textObj;
            }
            return null;
        }

        private void SetSceneState(xpScene scene, bool mode, int FrameBuffer = 1)
        {
            if(mode)
            {
                scene.SetOnline(FrameBuffer);
            }
            else
            {
                scene.SetOffline();
            }
        }

        public void SetClockWidgetValue(string text_name, int text_value)
        {
            xpClockTimerWidget clockObj;
            xpBaseWidget baseObj;

            if (_engine.GetWidgetByName(text_name, out baseObj))
            {
                clockObj = (xpClockTimerWidget)baseObj;
                clockObj.TimerValue = text_value;
            }
        }
        public void SetCounterWidgetValue(string text_name, int text_value)
        {
            xpBaseWidget baseObj;
            xpCounterWidget counterWidget;
            if (_engine.GetWidgetByName(text_name, out baseObj))
            {
                counterWidget = (xpCounterWidget)baseObj;
                counterWidget.Value = text_value;
            }
        }
        public xpClockTimerWidget GetClockWidget(string text_name)
        {
            xpClockTimerWidget clockObj;
            xpBaseWidget baseObj;

            if (_engine.GetWidgetByName(text_name, out baseObj))
            {
                clockObj = (xpClockTimerWidget)baseObj;
                return clockObj;
            }
            return null;
        }
        public xpSceneDirector PlayCurrentSceneDirector(string scene_name)
        {
            xpSceneDirector directorObj; xpScene _scene;
            if (_engine.GetSceneByName(scene_name, out _scene))
            {
                directorObj = _scene.SceneDirector;
                directorObj.Play();
                return directorObj;
            }
            return null;
        }
        public xpSceneDirector PlayPositionSceneDirector(xpScene scene, string scene_director, int position = 0)
        {
            xpSceneDirector directorObj;
            
            if (scene.GetSceneDirectorByName(scene_director,out directorObj))
            {
                directorObj.Position = position;
                
                directorObj.Play();
                return directorObj;
            }
            return null;
        }
        public xpSceneDirector GetSceneDirector(string scene_name)
        {
            xpSceneDirector directorObj; xpScene _scene;
            if (_engine.GetSceneByName(scene_name, out _scene))
            {
                directorObj = _scene.SceneDirector;
                return directorObj;
            }
            return null;
        }

        public void SetSceneState(string scene_name, bool mode)
        {
            xpScene _scene;
            if (_engine.GetSceneByName(scene_name, out _scene))
            {
                if (mode)
                    _scene.SetOnline(0);
                else
                    _scene.SetOffline();
            }
        }

        public Bitmap GetSceneImage(int position, int height, int width, string scene_name)
        {
            xpImage image_preview;
            xpScene preview_scene;
            if (_engine.GetSceneByName(scene_name, out preview_scene, true))
            {
                if (preview_scene.GetRenderedFrame(position, width, height, out image_preview))
                {
                    return xpTools.xpImageToBitmap(image_preview);
                }
            }
            return null;
        }

        public Bitmap GetSceneImage(int position, int height, int width, xpScene scene_name)
        {
            xpImage image_preview;
            if (scene_name.GetRenderedFrame(position, width, height, out image_preview))
            {
                return xpTools.xpImageToBitmap(image_preview);
            }
            return null;
        }
        public xpAnimController PlayAnimationDirector(string scene_name, string animation_name, PlayDirection pd_dir = PlayDirection.pd_Forward)
        {
            xpAnimController animObj; xpScene _scene;
            if (_engine.GetSceneByName(scene_name, out _scene))
            {
                if(_scene.GetAnimControllerByName(animation_name, out animObj))
                {
                    animObj.PlayDirection = pd_dir;
                    animObj.Play();
                }
                
                return animObj;
            }
            return null;
        }
        public xpAnimController PlayAnimationDirector(xpScene scene_name, string animation_name, PlayDirection pd_dir = PlayDirection.pd_Forward)
        {
            xpAnimController animObj;

            if (scene_name.GetAnimControllerByName(animation_name, out animObj))
            {
                animObj.PlayDirection = pd_dir;
                animObj.Play();
            }

            return animObj;
        }
        public void PlayAnimationDirector(xpAnimController animator_object, PlayDirection pd_dir = PlayDirection.pd_Forward)
        {
            if (animator_object != null)
            {
                animator_object.PlayDirection = pd_dir;
                animator_object.Play();
            }
        }

        public bool GetSceneState(string scene_name)
        {
            xpScene _scene;
            if (_engine.GetSceneByName(scene_name, out _scene))
            {
                if (_scene.IsOnline)
                {
                    return true;
                }
            }
            return false;
        }

        public xpAnimController GetAnimator(string scene_name, string animation_name)
        {
            xpScene _scene;
            xpAnimController animObj;
            if (_engine.GetSceneByName(scene_name, out _scene))
            {
                if (_scene.GetAnimControllerByName(animation_name, out animObj))
                {
                    return animObj;
                }
            }
            return null;
        }
        public xpAnimController GetAnimator(xpScene scene_name, string animation_name)
        {
            xpAnimController animObj;

            if (scene_name.GetAnimControllerByName(animation_name, out animObj))
            {
                return animObj;
            }
            return null;
        }
        public xpScene GetSceneObject(string scene_name)
        {
            xpScene _scene;
            if (_engine.GetSceneByName(scene_name, out _scene))
            {
                return _scene;
            }
            return null;
        }
        public xpScene GetSceneObject(string scene_name, bool as_copy = true)
        {
            xpScene _scene;
            if (_engine.GetSceneByName(scene_name, out _scene, as_copy))
            {
                return _scene;
            }
            return null;
        }
        public xpScene SetPreviewScene(string scene_name)
        {
            xpScene _scene = GetSceneObject(scene_name);
            _scene.SetPreview();
            return _scene;
        }

        public xpMaterial ChangeMaterial(string material_name, string file, int shader_name = 0)
        {
            xpMaterial material;
            if (_engine.GetMaterialByName(material_name, out material))
            {
                xpBaseShader baseShader;
                if(material.GetShader(shader_name, out baseShader))
                {
                    baseShader.FileName = file;
                }
            }
            return null;
        }

        public xpPublishedObject SetPublishedTextValue(int index, string name, string text)
        {
            xpPublishedObject published;
            xpBaseTakeItem baseItem;
            xpTakeItem takeItem;

            if (_engine.Sequencer.GetTakeItemByID(index, out baseItem))
            {
                takeItem = (xpTakeItem)baseItem;
                if(takeItem.GetPublishedObjectByName(name, out published))
                {
                    published.SetPropertyString(0, text);
                    return published;
                }
              
            }
            return null;
        }

        public xpTextListWidget SetTextListWidgetValue(string widget, string value)
        {
            xpTextListWidget textObj;
            xpBaseWidget baseObj;

            if (_engine.GetWidgetByName(widget, out baseObj))
            {
                textObj = (xpTextListWidget)baseObj;
                textObj.ClearStrings();
                textObj.AddString(value);
                textObj.ItemIndex = 0;
                return textObj;
            }

            return null;
        }
    }
}


