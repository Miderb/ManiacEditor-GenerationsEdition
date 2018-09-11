using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SystemColor = System.Drawing.Color;
using System.IO;

namespace RSDKv5
{
    public class GIF : IDisposable
    {
        Bitmap _bitmap;
        string _bitmapFilename;

        Dictionary<Tuple<Rectangle, bool, bool>, Bitmap> _bitmapCache = new Dictionary<Tuple<Rectangle, bool, bool>, Bitmap>();
        Dictionary<Tuple<Rectangle, bool, bool>, Texture2D> _texturesCache = new Dictionary<Tuple<Rectangle, bool, bool>, Texture2D>();

        public GIF(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("The GIF file was not found.", filename);
            }
            _bitmap = new Bitmap(filename);

            if (_bitmap.Palette != null && _bitmap.Palette.Entries.Length > 0)
            {
                _bitmap.MakeTransparent(_bitmap.Palette.Entries[0]);
            }
            else
            {
                _bitmap.MakeTransparent(SystemColor.FromArgb(0xff00ff));
            }
            // stash the filename too, so we can reload later
            _bitmapFilename = filename;
            // TODO: Proper transparent (palette index 0)
            _bitmap.MakeTransparent(SystemColor.FromArgb(0xff00ff));
        }

        private GIF(Bitmap bitmap)
        {
            this._bitmap = new Bitmap(bitmap);
        }

        private Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Draw the given area (section) of the source image
                // at location 0,0 on the empty bitmap (bmp)
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            }

            return bmp;
        }

        public Bitmap GetBitmap(Rectangle section, bool flipX = false, bool flipY = false)
        {
            Bitmap bmp;
            if (_bitmapCache.TryGetValue(new Tuple<Rectangle, bool, bool>(section, flipX, flipY), out bmp)) return bmp;

            bmp = CropImage(_bitmap, section);
            if (flipX)
            {
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            if (flipY)
            {
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            _bitmapCache[new Tuple<Rectangle, bool, bool>(section, flipX, flipY)] = bmp;
            return bmp;
        }

        // TOREMOVE
        public Texture2D GetTexture(Device device, Rectangle section, bool flipX = false, bool flipY = false)
        {
            Texture2D texture;
            if (_texturesCache.TryGetValue(new Tuple<Rectangle, bool, bool>(section, flipX, flipY), out texture)) return texture;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                GetBitmap(section, flipX, flipY).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);
                texture = Texture2D.FromStream(device, memoryStream);
            }

            _texturesCache[new Tuple<Rectangle, bool, bool>(section, flipX, flipY)] = texture;
            return texture;
        }

        public void Dispose()
        {
            ReleaseResources();
        }

        public void DisposeTextures()
        {
            if (null == _texturesCache) return;
            foreach (Texture2D texture in _texturesCache.Values)
                texture?.Dispose();
            _texturesCache.Clear();
        }

        public void Reload()
        {
            if (!File.Exists(_bitmapFilename))
            {
                throw new FileNotFoundException(string.Format("Could not find the file {0}", _bitmapFilename),
                                                _bitmapFilename);
            }
            ReleaseResources();
            _bitmap = new Bitmap(_bitmapFilename);
            _bitmap.MakeTransparent(SystemColor.FromArgb(0xff00ff));
        }

        private void ReleaseResources()
        {
            _bitmap.Dispose();
            DisposeTextures();
            foreach (Bitmap b in _bitmapCache.Values)
                b?.Dispose();
            _bitmapCache.Clear();
        }

        public GIF Clone()
        {
            return new GIF(_bitmapFilename);
        }
    }
}