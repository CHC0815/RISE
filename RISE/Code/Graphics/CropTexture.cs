using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.Graphics
{
    static class CropTexture
    {
        /// <summary>
        /// Crops a texture and then returns the cropped texture.
        /// </summary>
        /// <param name="Graphics">MonoGame GraphicsDeviceManager.</param>
        /// <param name="Texture">Texture you want to crop.</param>
        /// <param name="Area">The area you want to be cropped.</param>
        /// <returns>The cropped texture.</returns>
        public static Texture2D Crop(GraphicsDeviceManager Graphics, Texture2D Texture, Rectangle Area)
        {
            try
            {
                Texture2D cropped = new Texture2D(Graphics.GraphicsDevice, Area.Width, Area.Height);
                Color[] data = new Color[Texture.Width * Texture.Height];
                Color[] cropData = new Color[cropped.Width * cropped.Height];

                Texture.GetData<Color>(data);

                int index = 0;

                for (int y = Area.Y; y < Area.Y + Area.Height; y++)
                {
                    for (int x = Area.X; x < Area.X + Area.Width; x++)
                    {
                        cropData[index] = data[x + (y * Texture.Width)];
                        index++;
                    }
                }

                cropped.SetData<Color>(cropData);

                return cropped;
            }
            catch (Exception ex)
            {
                if (Graphics == null)
                    throw new Exception("Graphics argument is null", ex);
                else if (Texture == null)
                    throw new Exception("Texture argument is null", ex);
                else
                    throw ex;
            }
        }
    }
}
