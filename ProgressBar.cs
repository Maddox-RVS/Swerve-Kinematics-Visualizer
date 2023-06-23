using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwerveVisualizer
{
    internal class ProgressBar
    {
        private float min, max, value, x, y;
        private Rectangle barBounds, fillBounds;
        private Texture2D barTexture, fillTexture;

        public ProgressBar(float min, float max, float x, float y, Texture2D barTexture, Texture2D fillTexture)
        {
            this.min = min;
            this.max = max;
            this.x = x;
            this.y = y;
            barBounds = new Rectangle((int) x, (int) y, 177, 46);
            fillBounds = new Rectangle(barBounds.X + 4, barBounds.Y + 4, 169, 38);
            this.barTexture = barTexture;
            this.fillTexture = fillTexture;
        }

        public void Update()
        {
            fillBounds.Width = Math.Clamp(fillBounds.X, 0, barBounds.Width - 4);
            fillBounds.Width = (int) convertValueToWidth(value);
        }

        public float convertValueToWidth(float value)
        {
            float totalValueRange = Math.Abs(min-max);
            float numerator = value-min;
            float denominator = max-min;
            float percentInValueRange = numerator/denominator;
            return percentInValueRange*169;
        }

        public void setValue(float value)
        {
            this.value = value;
        }

        public float getValue()
        {
            return value;
        }

        public Vector2 getPosition()
        {
            return new Vector2(x, y);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Color color)
        {
            spriteBatch.Draw(barTexture, barBounds, color);
            spriteBatch.Draw(fillTexture, fillBounds, Color.White);

            if (value >= 0)
                spriteBatch.DrawString(font, value.ToString("0.000"), new Vector2(barBounds.X - 30 + barBounds.Width/2, barBounds.Y - 15 + barBounds.Height/2), Color.White);
            else
                spriteBatch.DrawString(font, value.ToString("0.000"), new Vector2(barBounds.X - 39 + barBounds.Width/2, barBounds.Y - 15 + barBounds.Height/2), Color.White);
        }
    }
}
