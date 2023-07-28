using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SwerveVisualizer
{
    internal class SwerveModule
    {
        private float driveVelocity;
        private float angleDegrees;
        private float angleOffset;
        public int moduleNumber;
        private float x;
        private float y;

        float tireMarkSpawnTimer, tireMarkSpawnRate;

        private Texture2D moduleTexture, tireMarkTexture;
        private Rectangle screenRect, boundsRect;
        private AngleMarker trajectoryLine;
        private List<TireMark> tireMarks;

        public SwerveModule(float x, float y, float angleOffset, int moduleNumber, Texture2D moduleTexture, Texture2D tireMarkTexture, GraphicsDevice graphicsDevice)
        {
            this.angleOffset = angleOffset;
            this.moduleNumber = moduleNumber;
            this.moduleTexture = moduleTexture;
            this.tireMarkTexture = tireMarkTexture;

            this.x = x;
            this.y = y;

            tireMarkSpawnTimer = 0f;
            tireMarkSpawnRate = 30f;
            tireMarks = new List<TireMark>();

            screenRect = new Rectangle(0, 0, Globals.Window.WIDTH, Globals.Window.HEIGHT);
            boundsRect = new Rectangle(0, 0, 50, 50);

             trajectoryLine = new AngleMarker(
                0, 0,
                0,
                0,
                3,
                Color.Blue,
                Color.Yellow,
                AngleMarker.Spin.CLOCKWISE,
                270,
                0,
                graphicsDevice);
        }

        public void updateTireMarks(GameTime gameTime, SwerveModuleState swerveModuleState, float rotationalVelocity)
        {
            tireMarkSpawnTimer += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (tireMarkSpawnTimer > tireMarkSpawnRate)
            {
                if (driveVelocity > 0) tireMarks.Add(new TireMark(x, y, angleDegrees, tireMarkTexture));
                tireMarkSpawnTimer = 0f;
            }

            for (int i = tireMarks.Count() - 1; i >= 0; i--)
            {
                if (rotationalVelocity == 0)
                    tireMarks[i].setPosition(tireMarks[i].getPosition().X - (swerveModuleState.getVectorDirection().X * 2.5f), tireMarks[i].getPosition().Y + (swerveModuleState.getVectorDirection().Y * 2.5f));

                Rectangle tempRect = new Rectangle((int) (screenRect.X - screenRect.Width), (int) (screenRect.Y - screenRect.Height), screenRect.Width*3, screenRect.Height*3);
                if (!tempRect.Contains(tireMarks[i].getBounds())) tireMarks.Remove(tireMarks[i]);
            }
        }

        public void setTrajectoryLineVisibility(bool status)
        {
            trajectoryLine.setShowAngleLine(status);
            trajectoryLine.setShowArrowHead(status);
        }

        public float getAngle()
        {
            return angleDegrees;
        }

        public void setAngle(float angle)
        {
            angleDegrees = angle;
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

        public void setPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 getPosition()
        {
            return new Vector2(x, y);
        }

        public void DrawTireMarks(SpriteBatch spriteBatch, float rotationalVelocity, float robotFrameSize, Vector2 robotFramePosition)
        {
            foreach (TireMark mark in tireMarks)
            {
                mark.Draw(spriteBatch, rotationalVelocity, moduleNumber, robotFramePosition, robotFrameSize);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                moduleTexture,
                new Rectangle((int) x, (int) y, boundsRect.Width, boundsRect.Height),
                null,
                Color.White,
                ConvertToRadians(angleDegrees),
                new Vector2(250, 250),
                SpriteEffects.None,
                0);

            trajectoryLine.setPosition(x, y);
            trajectoryLine.setAngleLineLength(driveVelocity * 20);
            trajectoryLine.setAngle(angleDegrees);
            trajectoryLine.Draw(spriteBatch);
        }
    }
}
