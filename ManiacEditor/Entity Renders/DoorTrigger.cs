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
    public class DoorTrigger : EntityRenderer
    {

        public override void Draw(DevicePanel d, SceneEntity entity, EditorEntity e, int x, int y, int Transparency)
        {
            int orientation = entity.attributesMap["orientation"].ValueUInt8;
            bool fliph = false;
            bool flipv = false;
            int AnimID_2 = 1;
            int frameID = 0;
            int frameID_2 = 0;
            int offsetX = 0;
            int offsetY = 0;
            switch (orientation)
            {
                case 0:
                    frameID = 0;
                    frameID_2 = 0;
                    AnimID_2 = 1;
                    offsetX = 0;
                    offsetY = 0;
                    break;
                case 1:
                    frameID = 0;
                    frameID_2 = 0;
                    AnimID_2 = 1;
                    fliph = true;
                    offsetX = -23;
                    offsetY = 0;
                    break;
                case 2:
                    frameID = 1;
                    AnimID_2 = 2;
                    offsetX = 0;
                    offsetY = 0;
                    break;
                case 3:
                    frameID = 1;
                    AnimID_2 = 2;
                    flipv = true;
                    offsetX = 0;
                    offsetY = 0;
                    break;
            }
            var editorAnim = e.LoadAnimation2("DoorTrigger", d, 0, frameID, fliph, flipv, false);
            var editorAnim2 = e.LoadAnimation2("DoorTrigger", d, AnimID_2, frameID_2, fliph, flipv, false);
            if (editorAnim != null && editorAnim.Frames.Count != 0 && frameID >= 0 && editorAnim2 != null && editorAnim2.Frames.Count != 0 && frameID >= 0)
            {
                var frame = editorAnim.Frames[e.index];
                var frame2 = editorAnim2.Frames[e.index];

                e.ProcessAnimation(frame2.Entry.FrameSpeed, frame2.Entry.Frames.Count, frame2.Frame.Duration);

                d.DrawBitmap(frame.Texture,
                    x + (fliph ? -frame.Frame.CenterX : frame.Frame.CenterX) - (fliph ? (frame.Frame.Width - editorAnim.Frames[0].Frame.Width) : 0) + offsetX,
                    y + (flipv ? -frame.Frame.CenterY : frame.Frame.CenterY) + (flipv ? (frame.Frame.Height - frame.Frame.Height - editorAnim.Frames[0].Frame.Height) : 0) + offsetY,
                    frame.Frame.Width, frame.Frame.Height, false, Transparency);

                d.DrawBitmap(frame2.Texture,
                    x + (fliph ? -frame2.Frame.CenterX : frame2.Frame.CenterX) - (fliph ? (frame2.Frame.Width - editorAnim2.Frames[0].Frame.Width) : 0) + offsetX,
                    y + (flipv ? -frame2.Frame.CenterY : frame2.Frame.CenterY) + (flipv ? (frame2.Frame.Height - frame2.Frame.Height - editorAnim2.Frames[0].Frame.Height) : 0) + offsetY,
                    frame2.Frame.Width, frame2.Frame.Height, false, Transparency);
            }
        }

        public override string GetObjectName()
        {
            return "DoorTrigger";
        }
    }
}
