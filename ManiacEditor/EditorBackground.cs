using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using RSDKv5Color = RSDKv5.Color;


namespace ManiacEditor
{
    class EditorBackground : IDrawable
    {
        const int BOX_SIZE = 8;
        const int TILE_BOX_SIZE = 1;

        Vertices vb1;
        Vertices vb2;

        static int DivideRoundUp(int number, int by)
        {
            return (number + by - 1) / by;
        }

        int width;
        int height;

        public EditorBackground(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Draw(Graphics g)
        {
            
        }

        public void Draw(GLViewControl d)
        {
            RSDKv5Color rcolor1 = Editor.Instance.EditorScene.EditorMetadata.BackgroundColor1;
            RSDKv5Color rcolor2 = Editor.Instance.EditorScene.EditorMetadata.BackgroundColor2;
            

            Color color1 = Color.FromArgb(rcolor1.A, rcolor1.R, rcolor1.G, rcolor1.B);
            Color color2 = Color.FromArgb(rcolor2.A, rcolor2.R, rcolor2.G, rcolor2.B);

            // Draw with first color everything
            if (vb1 == null)
            {
                using (var c = new VBCreator())
                {
                    c.AddRectangle(new Rectangle(0, 0, width, height));
                    vb1 = c.GetVertices();
                }
            }
            vb1.Draw(PrimitiveType.Quads, color1);

            if (color2.A != 0)
            {
                if (vb2 == null)
                {
                    using (var c = new VBCreator())
                    {
                        for (int y = 0; y < DivideRoundUp(height, BOX_SIZE * EditorLayer.TILE_SIZE); ++y)
                        {
                            for (int x = 0; x < DivideRoundUp(width, BOX_SIZE * EditorLayer.TILE_SIZE); ++x)
                            {
                                if ((x + y) % 2 == 1) c.AddRectangle(new Rectangle(x * BOX_SIZE * EditorLayer.TILE_SIZE, y * BOX_SIZE * EditorLayer.TILE_SIZE, BOX_SIZE * EditorLayer.TILE_SIZE, BOX_SIZE * EditorLayer.TILE_SIZE));
                            }
                        }
                        vb2 = c.GetVertices();
                    }
                }
                GL.PushMatrix();
                GL.Translate(0, 0, Editor.LAYER_DEPTH / 2);
                vb2.Draw(PrimitiveType.Quads, color2);
                GL.PopMatrix();
            }
        }

        public void DrawEdit(GLViewControl d)
        {
            RSDKv5Color rcolor1 = Editor.Instance.EditorScene.EditorMetadata.BackgroundColor1;
            RSDKv5Color rcolor2 = Editor.Instance.EditorScene.EditorMetadata.BackgroundColor2;


            Color color1 = Color.FromArgb(30, rcolor1.R, rcolor1.G, rcolor1.B);
            Color color2 = Color.FromArgb(30, rcolor2.R, rcolor2.G, rcolor2.B);

            // Draw with first color everything
            if (vb1 == null)
            {
                using (var c = new VBCreator())
                {
                    c.AddRectangle(new Rectangle(0, 0, width, height));
                    vb1 = c.GetVertices();
                }
            }
            vb1.Draw(PrimitiveType.Quads, color1);

            if (color2.A != 0)
            {
                if (vb2 == null)
                {
                    using (var c = new VBCreator())
                    {
                        for (int y = 0; y < DivideRoundUp(height, BOX_SIZE * EditorLayer.TILE_SIZE); ++y)
                        {
                            for (int x = 0; x < DivideRoundUp(width, BOX_SIZE * EditorLayer.TILE_SIZE); ++x)
                            {
                                if ((x + y) % 2 == 1) c.AddRectangle(new Rectangle(x * BOX_SIZE * EditorLayer.TILE_SIZE, y * BOX_SIZE * EditorLayer.TILE_SIZE, BOX_SIZE * EditorLayer.TILE_SIZE, BOX_SIZE * EditorLayer.TILE_SIZE));
                            }
                        }
                        vb2 = c.GetVertices();
                    }
                }
                GL.PushMatrix();
                GL.Translate(0, 0, Editor.LAYER_DEPTH / 2);
                vb2.Draw(PrimitiveType.Quads, color2);
                GL.PopMatrix();
            }
        }

        public void DrawGrid(DevicePanel d)
        {
            Rectangle screen = d.GetScreen();

            RSDKv5Color rcolor1 = Editor.Instance.EditorScene.EditorMetadata.BackgroundColor1;
            RSDKv5Color rcolor2 = Editor.Instance.EditorScene.EditorMetadata.BackgroundColor2;

            Color color1 = Color.FromArgb(rcolor1.A, rcolor1.R, rcolor1.G, rcolor1.B);
            Color color2 = Color.FromArgb(rcolor2.A, rcolor2.R, rcolor2.G, rcolor2.B);

            int start_x = screen.X / (TILE_BOX_SIZE * EditorLayer.TILE_SIZE);
            int end_x = Math.Min(DivideRoundUp(screen.X + screen.Width, TILE_BOX_SIZE * EditorLayer.TILE_SIZE), Editor.Instance.SceneWidth);
            int start_y = screen.Y / (TILE_BOX_SIZE * EditorLayer.TILE_SIZE);
            int end_y = Math.Min(DivideRoundUp(screen.Y + screen.Height, TILE_BOX_SIZE * EditorLayer.TILE_SIZE), Editor.Instance.Height);

            if (color2.A != 0)
            {
                for (int y = start_y; y < end_y; ++y)
                {
                    for (int x = start_x; x < end_x; ++x)
                    {
                            d.DrawLine(x * EditorLayer.TILE_SIZE, y * EditorLayer.TILE_SIZE, x * EditorLayer.TILE_SIZE + EditorLayer.TILE_SIZE, y * EditorLayer.TILE_SIZE, System.Drawing.Color.Black);
                            d.DrawLine(x * EditorLayer.TILE_SIZE, y * EditorLayer.TILE_SIZE, x * EditorLayer.TILE_SIZE, y * EditorLayer.TILE_SIZE + EditorLayer.TILE_SIZE, System.Drawing.Color.Black);
                            //d.DrawLine(x * EditorLayer.TILE_SIZE + EditorLayer.TILE_SIZE, y * EditorLayer.TILE_SIZE + EditorLayer.TILE_SIZE, x * EditorLayer.TILE_SIZE + EditorLayer.TILE_SIZE, y * EditorLayer.TILE_SIZE, System.Drawing.Color.Black);
                            //d.DrawLine(x * EditorLayer.TILE_SIZE + EditorLayer.TILE_SIZE, y * EditorLayer.TILE_SIZE + EditorLayer.TILE_SIZE, x * EditorLayer.TILE_SIZE, y * EditorLayer.TILE_SIZE + EditorLayer.TILE_SIZE, System.Drawing.Color.Black);
                    }
                }
            }
        }
    }
}
