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
    public class Ring
    {

        public void Draw(DevicePanel d, SceneEntity entity, EditorEntity e, int x, int y, int Transparency)
        {
            int type = (int)entity.attributesMap["type"].ValueVar;
            int angle = 0;
            if (entity.Object.Attributes.Contains(new AttributeInfo("angle", AttributeTypes.INT32)))
            {
                angle = (int)entity.attributesMap["angle"].ValueInt32;
            }
            bool fliph = false;
            bool flipv = false;
            int amplitudeX = 0;
            int amplitudeY = 0;
            if (entity.Object.Attributes.Contains(new AttributeInfo("amplitude", AttributeTypes.POSITION))) {
                amplitudeX = (int)entity.attributesMap["amplitude"].ValuePosition.X.High;
                amplitudeY = (int)entity.attributesMap["amplitude"].ValuePosition.Y.High;
            }
            int angleStateX = 0;
            int angleStateY = 0;
            int animID;
            switch (type)
            {
                case 0:
                    animID = 0;
                    break;
                case 1:
                    animID = 1;
                    break;
                case 2:
                    animID = 2;
                    break;
                default:
                    animID = 0;
                    break;
            }



            var editorAnim = e.LoadAnimation2("Ring", d, animID, -1, fliph, flipv, false);
            if (editorAnim != null && editorAnim.Frames.Count != 0 && animID >= 0)
            {
                var frame = editorAnim.Frames[0];
                if (entity.Object.Attributes.Contains(new AttributeInfo("amplitude", AttributeTypes.POSITION)))
                {
                    if (amplitudeX != 0 || amplitudeY != 0)
                    {
                        angleStateX = (int)((frame.Frame.CenterX + amplitudeX - 8) * Math.Cos(Math.PI * angle / 128) + (frame.Frame.CenterY + amplitudeY - 8) * Math.Sin(Math.PI * angle / 128));
                        angleStateY = (int)((frame.Frame.CenterX + amplitudeX - 8) * Math.Sin(Math.PI * angle / 128) - (frame.Frame.CenterY + amplitudeY - 8) * Math.Cos(Math.PI * angle / 128));
                    }
                }

                if (type != 2)
                {
                    frame = editorAnim.Frames[e.index];
                    e.ProcessAnimation(frame.Entry.FrameSpeed, frame.Entry.Frames.Count, frame.Frame.Duration);
                }

                d.DrawBitmap(frame.Texture,
                    x + frame.Frame.CenterX + (angleStateX),
                    y + frame.Frame.CenterY - (angleStateY),
                    frame.Frame.Width, frame.Frame.Height, false, Transparency);
            }
        }
    }
}