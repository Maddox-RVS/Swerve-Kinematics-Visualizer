using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SwerveVisualizer
{
    internal class AngleMarker
    {
        private Texture2D baseLine, angleLine;
        private float x, y, angle, angleLineLength, angleLineThickness, baseLineLength, baseLineThickness, pointOfOrigin, baseLineAngle;
        Spin spinDirection;
        
        public enum Spin {
            CLOCKWISE,
            COUNTER_CLOCKWISE
        }

        public AngleMarker(float x, float y, float angle, float length, float thickness, Color baseLine, Color angleLine, Spin spinDirection, float pointOfOrigin, float baseLineAngle, GraphicsDevice graphicsDevice)
        {
            this.baseLine = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            this.baseLine.SetData(new[] { baseLine });

            this.angleLine = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            this.angleLine.SetData(new[] { angleLine });
            this.angle = angle;

            angleLineLength = length;
            baseLineLength = length*2;

            angleLineThickness = thickness;
            baseLineThickness = thickness;

            this.x = x;
            this.y = y;

            this.spinDirection = spinDirection;
            this.pointOfOrigin = pointOfOrigin;

            this.baseLineAngle = baseLineAngle;
        }

        public float convertDegreesToRadians(float degrees)
        {
            return (float) ((Math.PI / 180) * degrees);
        }

        public float getBaseLineLength()
        {
            return baseLineLength;
        }

        public void setBaseLineLength(float length)
        {
            baseLineLength = length;
        }

        public float getAngleLineLength()
        {
            return angleLineLength;
        }

        public void setAngleLineLength(float length)
        {
            angleLineLength = length;
        }

        public float getAngle()
        {
            return angle;
        }

        public void setAngle(float angle)
        {
            this.angle = angle;
        }

        public float getBaseLineAngle()
        {
            return baseLineAngle;
        }

        public void setBaseLineAngle(float baseLineAngle)
        {
            this.baseLineAngle = baseLineAngle;
        }

        public Vector2 getPosition()
        {
            return new Vector2(x, y);
        }

        public void setPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float applyAngleTanslation(float angle)
        {
            if (spinDirection == Spin.CLOCKWISE) return angle + pointOfOrigin;
            else if (spinDirection == Spin.COUNTER_CLOCKWISE) return 360 - (angle + pointOfOrigin);
            return angle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
             spriteBatch.Draw(
                baseLine, 
                new Rectangle((int) x, (int) y, (int) baseLineLength, (int) baseLineThickness), 
                null, 
                Color.White, 
                convertDegreesToRadians(applyAngleTanslation(baseLineAngle)), 
                new Vector2(0.5f, 0), 
                SpriteEffects.None, 0);

            spriteBatch.Draw(
                angleLine, 
                new Rectangle((int) x, (int) (y + 0.5f + angleLineThickness/2), (int) angleLineLength, (int) angleLineThickness), 
                null, 
                Color.White, 
                convertDegreesToRadians(applyAngleTanslation(angle)), 
                new Vector2(0, 0.5f), 
                SpriteEffects.None, 0);
        }
    }
}
