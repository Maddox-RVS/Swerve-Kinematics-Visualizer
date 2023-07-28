using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwerveVisualizer
{
    internal class TireMark
    {
        private float x;
        private float y;
        private float width;
        private float height;
        private float rotation;
        private Texture2D tireMarkTexture;

        public TireMark(float x, float y, float rotation, Texture2D tireMarkTexture)
        {
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            width = 201/10;
            height = 92/10;
            this.tireMarkTexture = tireMarkTexture;
        }

        public enum RotationType
        {
            MarkCenter,
            RobotCenter
        }

        public void setRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public float getRotation()
        {
            return this.rotation;
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int) x, (int) y, (int) width, (int) height);
        }

        public void setPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 getPosition()
        {
            return new Vector2(x, y);
        }

        public void Draw(SpriteBatch spriteBatch, float rotationalVelocity, int moduleNumber, Vector2 robotFramePosition, float robotFrameSize)
        {
            if (rotationalVelocity != 0)
            {
                // Calculate the distance and angle between obj1 and obj2
                float distance = Vector2.Distance(new Vector2(this.x, this.y), robotFramePosition);
                float angle = (float) Math.Atan2(this.y - robotFramePosition.Y, this.x - robotFramePosition.X);

                // Update the angle by adding or subtracting the rotation speed times the elapsed time
                angle -= rotationalVelocity * 0.1f;

                float xDistance = Math.Abs(this.x - robotFramePosition.X);
                float yDistance = Math.Abs(this.y - robotFramePosition.Y);

                float rise;
                float run;

                if (this.x < robotFramePosition.X) run = xDistance;
                else run = -xDistance;

                if (this.y < robotFramePosition.Y) rise = yDistance;
                else rise = -yDistance;

                rotation = Trigonometry.convertSlopeToDegrees(rise, run);

                // Calculate the new position of obj1 based on the updated angle and distance
                Vector2 newPosition;
                newPosition = Vector2.Transform(new Vector2(distance, 0), Matrix.CreateRotationZ(angle)) + robotFramePosition;
                this.x = newPosition.X;
                this.y = newPosition.Y;
            }

            spriteBatch.Draw(
            tireMarkTexture,
            new Rectangle((int) this.x, (int) this.y, (int) width, (int) height),
            null,
            Color.DarkSlateGray,
            Trigonometry.convertDegreesToRadians(rotation),
            new Vector2((int) (tireMarkTexture.Width/2), (int) (tireMarkTexture.Height/2)),
            SpriteEffects.None,
            0);
        }
    }
}
