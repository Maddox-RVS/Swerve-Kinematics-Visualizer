using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        public Robot(Texture2D swerveModuleTexture, Texture2D robotFrameTexture, float x, float y, float chassisSize, GraphicsDevice graphicsDevice)
        {
            //Front right
            SwerveModule module0 = new SwerveModule(0, 0, swerveModuleTexture, graphicsDevice);

            //Front left
            SwerveModule module1 = new SwerveModule(0, 1, swerveModuleTexture, graphicsDevice);

            //Back left
            SwerveModule module2 = new SwerveModule(0, 2, swerveModuleTexture, graphicsDevice);

            //Back right
            SwerveModule module3 = new SwerveModule(0, 3, swerveModuleTexture, graphicsDevice);

            swerveModules = new SwerveModule[] { module0, module1, module2, module3 };
            swerveModuleStates = new SwerveModuleState[swerveModules.Length];

            this.x = x;
            this.y = y;
            this.chassisSize = chassisSize;
            this.robotFrameTexture = robotFrameTexture;
        }

        public void updateModuleStates(float forwardVelocity, float straithVelocity, float rotationVelocity)
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                robotFrameTexture,
                new Rectangle((int) x, (int) y, 300, 300),
                null,
                Color.LightSlateGray,
                0,
                new Vector2(250, 250),
                SpriteEffects.None,
                0);

            //Front right
            swerveModules[0].Draw((int) (x + chassisSize/2), (int) (y - chassisSize/2), spriteBatch);

            //Front left
            swerveModules[1].Draw((int) (x - chassisSize/2), (int) (y - chassisSize/2), spriteBatch);

            //Back left
            swerveModules[2].Draw((int) (x - chassisSize/2), (int) (y + chassisSize/2), spriteBatch);

            //Back right
            swerveModules[3].Draw((int) (x + chassisSize/2), (int) (y + chassisSize/2), spriteBatch);
        }
    }
}
