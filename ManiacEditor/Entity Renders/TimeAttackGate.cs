﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ManiacEditor;
using Microsoft.Xna.Framework;
using RSDKv5;

namespace ManiacEditor.Entity_Renders
{
    public class TimeAttackGate : EntityRenderer
    {

        public override void Draw(DevicePanel d, SceneEntity entity, EditorEntity e, int x, int y, int Transparency)
        {
            bool finish = entity.attributesMap["finishLine"].ValueBool;
            var editorAnimBase = e.LoadAnimation2("SpeedGate", d, 0, 0, false, false, false);
            var editorAnimTop = e.LoadAnimation2("SpeedGate", d, 1, 0, false, false, false);
            var editorAnimFins = e.LoadAnimation2("SpeedGate", d, finish ? 4 : 3, -1, false, false, false);
            if (editorAnimBase != null && editorAnimTop != null && editorAnimFins != null)
            {
                var frameBase = editorAnimBase.Frames[0];
                var frameTop = editorAnimTop.Frames[0];
                d.DrawBitmap(frameBase.Texture, x + frameBase.Frame.CenterX, y + frameBase.Frame.CenterY,
                    frameBase.Frame.Width, frameBase.Frame.Height, false, Transparency);
                d.DrawBitmap(frameTop.Texture, x + frameTop.Frame.CenterX, y + frameTop.Frame.CenterY,
                    frameTop.Frame.Width, frameTop.Frame.Height, false, Transparency);
                for (int i = 0; i < editorAnimFins.Frames.Count; ++i)
                {
                    var frame = editorAnimFins.Frames[i];
                    d.DrawBitmap(frame.Texture, x + frame.Frame.CenterX, y + frame.Frame.CenterY,
                        frame.Frame.Width, frame.Frame.Height, false, Transparency);
                }
            }
        }

        public override string GetObjectName()
        {
            return "TimeAttackGate";
        }
    }
}
