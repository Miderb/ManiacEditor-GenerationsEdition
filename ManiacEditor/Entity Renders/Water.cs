﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ManiacEditor;
using Microsoft.Xna.Framework;
using RSDKv5;
using SystemColors = System.Drawing.Color;

namespace ManiacEditor.Entity_Renders
{
    public class Water : EntityRenderer
    {

        public override void Draw(DevicePanel d, SceneEntity entity, EditorEntity e, int x, int y, int Transparency)
        {
            int type = (int)entity.attributesMap["type"].ValueVar;
            var widthPixels = (int)(entity.attributesMap["size"].ValuePosition.X.High);
            var heightPixels = (int)(entity.attributesMap["size"].ValuePosition.Y.High);
            var width = (int)widthPixels / 16;
            var height = (int)heightPixels / 16;
            bool fliph = false;
            bool flipv = false;
            int animID;
            if (type == 2)
            {
                animID = 2;
            }
            else
            {
                animID = 0;
            }
            var editorAnim = e.LoadAnimation2("Water", d, animID, -1, fliph, flipv, false);
            if (editorAnim != null && editorAnim.Frames.Count != 0 && animID >= 0 && type != 1)
            {
                var frame = editorAnim.Frames[e.index];

                e.ProcessAnimation(frame.Entry.FrameSpeed, frame.Entry.Frames.Count, frame.Frame.Duration);

                d.DrawBitmap(frame.Texture,
x + frame.Frame.CenterX - (fliph ? (frame.Frame.Width - editorAnim.Frames[0].Frame.Width) : 0),
y + frame.Frame.CenterY + (flipv ? (frame.Frame.Height - editorAnim.Frames[0].Frame.Height) : 0),
frame.Frame.Width, frame.Frame.Height, false, Transparency);

            }
            else if (width != 0 && height != 0 && type != 2)
            {
                //Draw Icon
                editorAnim = e.LoadAnimation2("EditorIcons2", d, 0, 8, fliph, flipv, false);
                if (editorAnim != null && editorAnim.Frames.Count != 0)
                {
                    var frame = editorAnim.Frames[e.index];

                    d.DrawBitmap(frame.Texture,
                        x + frame.Frame.CenterX - (fliph ? (frame.Frame.Width - editorAnim.Frames[0].Frame.Width) : 0),
                        y + frame.Frame.CenterY + (flipv ? (frame.Frame.Height - editorAnim.Frames[0].Frame.Height) : 0),
                        frame.Frame.Width, frame.Frame.Height, false, Transparency);
                }
                    int x1 = x + widthPixels /  - 2;
                    int x2 = x + widthPixels /  2 - 1;
                    int y1 = y + heightPixels / - 2;
                    int y2 = y + heightPixels / 2 - 1;


                d.DrawLine(x1, y1, x1, y2, SystemColors.Aqua);
                d.DrawLine(x1, y1, x2, y1, SystemColors.Aqua);
                d.DrawLine(x2, y2, x1, y2, SystemColors.Aqua);
                d.DrawLine(x2, y2, x2, y1, SystemColors.Aqua);

                
                // draw corners
                for (int i = 0; i < 4; i++)
                {
                    bool right = (i & 1) > 0;
                    bool bottom = (i & 2) > 0;

                    editorAnim = e.LoadAnimation2("EditorAssets", d, 2, 0, right, bottom, false);
                    if (editorAnim != null && editorAnim.Frames.Count != 0)
                    {
                        var frame = editorAnim.Frames[e.index];
                        e.ProcessAnimation(frame.Entry.FrameSpeed, frame.Entry.Frames.Count, frame.Frame.Duration);
                        d.DrawBitmap(frame.Texture,
                            (x + widthPixels / (right ? 2 : -2)) - (right ? frame.Frame.Width : 0),
                            (y + heightPixels / (bottom ? 2 : -2) - (bottom ? frame.Frame.Height : 0)),
                            frame.Frame.Width, frame.Frame.Height, false, Transparency);

                    }
                }
            }
        }

        public override string GetObjectName()
        {
            return "Water";
        }
    }
}
