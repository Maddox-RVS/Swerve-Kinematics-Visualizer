using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwerveVisualizer
{
    internal class SwerveModule
    {
        private float driveVelocity;
        private float angleVelocity;
        private float angleOffset;
        public int moduleNumber;

        private Texture2D texture;
        private Rectangle screenRect, boundsRect;
        private AngleMarker trajectoryLine;

        public SwerveModule(float angleOffset, int moduleNumber, Texture2D texure)
        {
            this.angleOffset = angleOffset;
            this.moduleNumber = moduleNumber;
            this.texture = texure;

            screenRect = new Rectangle(0, 0, Globals.Window.WIDTH, Globals.Window.HEIGHT);
            boundsRect = new Rectangle(0, 0, 50, 50);
        }

        public float getAngle()
        {
            return angleVelocity;
        }

        public void setAngle(float angle)
        {
            angleVelocity = angle;
        }

        public float getDrive()
        {
            return driveVelocity;
        }

        public void setDrive(float velocity)
        {
            this.driveVelocity = velocity;
        }

        public float ConvertToRadians(float degrees)
        {
            return 0.01745329f * degrees;
        }

        public void Draw(int x, int y, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle(x, y, boundsRect.Width, boundsRect.Height),
                null,
                Color.White,
                ConvertToRadians(angleVelocity),
                new Vector2(250, 250),
                SpriteEffects.None,
                0);
        }
    }
}
