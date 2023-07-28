using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace SwerveVisualizer
{
    internal class Robot
    {
        private SwerveModule[] swerveModules;
        private SwerveModuleState[] swerveModuleStates;
        private Texture2D robotFrameTexture;
        private float x, y, chassisSize;
        Vector2 totalTrajectory;
        AngleMarker totalTrajectoryLine;
        bool showModuleTrajectories, showTotalTrajectories, showRobotFrame, showTireMarks;

        public Robot(Texture2D swerveModuleTexture, Texture2D robotFrameTexture, Texture2D tireMark, float x, float y, float chassisSize, GraphicsDevice graphicsDevice)
        {
            //Front right
            SwerveModule module0 = new SwerveModule((int) (x + chassisSize/2), (int) (y - chassisSize/2), 0, 0, swerveModuleTexture, tireMark, graphicsDevice);

            //Front left
            SwerveModule module1 = new SwerveModule((int) (x - chassisSize/2), (int) (y - chassisSize/2), 0, 1, swerveModuleTexture, tireMark, graphicsDevice);

            //Back left
            SwerveModule module2 = new SwerveModule((int) (x - chassisSize/2), (int) (y + chassisSize/2), 0, 2, swerveModuleTexture, tireMark, graphicsDevice);

            //Back right
            SwerveModule module3 = new SwerveModule((int) (x + chassisSize/2), (int) (y + chassisSize/2), 0, 3, swerveModuleTexture, tireMark, graphicsDevice);

            swerveModules = new SwerveModule[] { module0, module1, module2, module3 };
            swerveModuleStates = new SwerveModuleState[swerveModules.Length];

            this.x = x;
            this.y = y;
            this.chassisSize = chassisSize;
            this.robotFrameTexture = robotFrameTexture;

            totalTrajectoryLine = new AngleMarker(
                0, 0,
                0,
                0,
                3,
                Color.Blue,
                Color.Yellow,
                AngleMarker.Spin.COUNTER_CLOCKWISE,
                0,
                0,
                graphicsDevice);
            totalTrajectoryLine.setShowArrowHead(true);
        }

        public void updateModuleStates(float forwardVelocity, float straithVelocity, float rotationVelocity, GameTime gameTime)
        {
            for (int i = 0; i < 4; i++) {
                swerveModuleStates[i] = Kinematics.updateSwerveModuleState(
                    straithVelocity, 
                    forwardVelocity, 
                    rotationVelocity, 
                    Globals.Swerve.MAX_VELOCITY, 
                    Globals.Swerve.MAX_ANGULAR_VELOCITY, 
                    i);
            }
            Kinematics.desaturateModuleStates(swerveModuleStates);

            foreach (SwerveModule module in swerveModules)
            {
                module.updateTireMarks(gameTime, swerveModuleStates[module.moduleNumber], rotationVelocity);

                if (swerveModuleStates[module.moduleNumber].getVelocity() == 0)
                    module.setTrajectoryLineVisibility(false);
                else
                    module.setTrajectoryLineVisibility(true);
                module.setDrive(swerveModuleStates[module.moduleNumber].getVelocity());

                if (float.IsNaN(swerveModuleStates[module.moduleNumber].getAngleDegrees()))
                    module.setAngle(module.getAngle());
                else
                    module.setAngle(swerveModuleStates[module.moduleNumber].getAngleDegrees());
            }

            totalTrajectory = Kinematics.addVectors2(
                new Vector2[] {
                    swerveModuleStates[0].getVectorDirection(),
                    swerveModuleStates[1].getVectorDirection(),
                    swerveModuleStates[2].getVectorDirection(),
                    swerveModuleStates[3].getVectorDirection()
                }    
            );
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

        public void Draw(SpriteBatch spriteBatch, float rotationalVelocity)
        {
            foreach (SwerveModule module in swerveModules)
            {
                module.DrawTireMarks(spriteBatch, rotationalVelocity, chassisSize, new Vector2(x, y));
            }

            spriteBatch.Draw(
                robotFrameTexture,
                new Rectangle((int) x, (int) y, 300, 300),
                null,
                Color.LightSlateGray,
                0,
                new Vector2(250, 250),
                SpriteEffects.None,
                0);

            foreach (SwerveModule module in swerveModules)
            {
                module.Draw(spriteBatch);
            }

            totalTrajectoryLine.Draw(spriteBatch);
            totalTrajectoryLine.setPosition(x, y);
            totalTrajectoryLine.setAngleLineLength(Trigonometry.convertSlopeToHypotenuse(totalTrajectory.Y, totalTrajectory.X) * 0.20f);
            totalTrajectoryLine.setAngle(Trigonometry.convertSlopeToDegrees(totalTrajectory.Y, totalTrajectory.X));
        }
    }
}
