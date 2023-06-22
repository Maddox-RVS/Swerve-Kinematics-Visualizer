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
    internal class Robot
    {
        private SwerveModule[] swerveModules;
        private AngleMarker[] trajectoryLines;
        private float gyro, x, y, chassisSize;

        public Robot(Texture2D swerveModuleTexture, float x, float y, float chassisSize, GraphicsDevice graphicsDevice)
        {
            gyro = 90;

            //Front right
            SwerveModule module0 = new SwerveModule(0, swerveModuleTexture);

            //Front left
            SwerveModule module1 = new SwerveModule(0, swerveModuleTexture);

            //Back left
            SwerveModule module2 = new SwerveModule(0, swerveModuleTexture);

            //Back right
            SwerveModule module3 = new SwerveModule(0, swerveModuleTexture);

            swerveModules = new SwerveModule[] { module0, module1, module2, module3 };

            AngleMarker trajectoryLineSetup = new AngleMarker(
                x, y,
                0,
                0,
                3,
                Color.Blue,
                Color.Yellow,
                graphicsDevice);

            AngleMarker marker0 = trajectoryLineSetup;
            AngleMarker marker1 = trajectoryLineSetup;
            AngleMarker marker2 = trajectoryLineSetup;
            AngleMarker marker3 = trajectoryLineSetup;

            trajectoryLines = new AngleMarker[] { marker0, marker1, marker2, marker3 };

            this.x = x;
            this.y = y;
            this.chassisSize = chassisSize;
        }

        public void updateModuleStates(float forwardVelocity, float straithVelocity, float rotationVelocity)
        {
            // Make robot field relative
            //temp = FWD·cos(θ) + STR·sin(θ);
            //STR = -FWD·sin(θ) + STR·cos(θ);
            //FWD = temp; 

            float temp = forwardVelocity * ((float) Math.Cos(gyro)) + straithVelocity * ((float) Math.Sin(gyro));
            straithVelocity = -forwardVelocity * ((float) Math.Sin(gyro)) + straithVelocity * ((float) Math.Cos(gyro));
            forwardVelocity = temp;

            //Convert the vehicle motion commands into wheel speed and angle commands (inverse kinematics)
            //L is the vehicle’s wheelbase
            //W is the vehicle’s trackwidth
            //R = sqrt(L2+W2);

            float L = 20.75f;
            float W = 20.75f;
            float R = (float) (Math.Sqrt(Math.Pow(L, 2) + Math.Pow(W, 2)));

            //A = STR - RCW·(L/R);
            //B = STR + RCW·(L/R);
            //C = FWD - RCW·(W/R);
            //D = FWD + RCW·(W/R);
            //ws1 = sqrt(B2+C2); wa1 = atan2(B,C)·180/pi;
            //ws2 = sqrt(B2+D2); wa2 = atan2(B,D)·180/pi;
            //ws3 = sqrt(A2+D2); wa3 = atan2(A,D)·180/pi;
            //ws4 = sqrt(A2+C2); wa4 = atan2(A,C)·180/pi; 

            float A = straithVelocity - rotationVelocity * (L/R);
            float B = straithVelocity + rotationVelocity * (L/R);
            float C = forwardVelocity - rotationVelocity * (W/R);
            float D = forwardVelocity + rotationVelocity * (W/R);

            float ws1 = (float) (Math.Sqrt(Math.Pow(B, 2) + Math.Pow(C, 2)));
            float ws2 = (float) (Math.Sqrt(Math.Pow(B, 2) + Math.Pow(D, 2)));
            float ws3 = (float) (Math.Sqrt(Math.Pow(A, 2) + Math.Pow(D, 2)));
            float ws4 = (float) (Math.Sqrt(Math.Pow(A, 2) + Math.Pow(C, 2)));

            float wa1 = (float) (Math.Atan2(B, C) * 1809/Math.PI);
            float wa2 = (float) (Math.Atan2(B, D) * 1809/Math.PI);
            float wa3 = (float) (Math.Atan2(A, D) * 1809/Math.PI);
            float wa4 = (float) (Math.Atan2(A, C) * 1809/Math.PI);

            //Normalize wheel speeds
            //max=ws1; 
            //if(ws2>max)max=ws2; 
            //if(ws3>max)max=ws3; 
            //if(ws4>max)max=ws4;
            //if(max>1){ws1/=max; ws2/=max; ws3/=max; ws4/=max;} 

            float max = ws1;
            if (ws2 > max) max = ws2;
            if (ws3 > max) max = ws3;
            if (ws4 > max) max = ws4;
            if (max > 1)
            {
                ws1 /= max;
                ws2 /= max;
                ws3 /= max;
                ws4 /= max;
            }

            //Update the modules

            swerveModules[0].setDrive(ws3);
            swerveModules[0].setAngle(wa3);

            swerveModules[1].setDrive(ws2);
            swerveModules[1].setAngle(wa2);

            swerveModules[2].setDrive(ws1);
            swerveModules[2].setAngle(wa1);

            swerveModules[3].setDrive(ws4);
            swerveModules[3].setAngle(wa4);
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
            //Front right
            swerveModules[0].Draw((int) (x + chassisSize/2), (int) (y - chassisSize/2), spriteBatch);
            trajectoryLines[0].setPosition(x + chassisSize/2, y - chassisSize/2);
            trajectoryLines[0].setBaseLineLength(swerveModules[0].getDrive() * 100);
            trajectoryLines[0].setAngleLineLength(swerveModules[0].getDrive() * 100);
            trajectoryLines[0].setAngle(swerveModules[0].getAngle());
            trajectoryLines[0].Draw(spriteBatch);

            //Front left
            swerveModules[1].Draw((int) (x - chassisSize/2), (int) (y - chassisSize/2), spriteBatch);
            trajectoryLines[1].setPosition(x - chassisSize/2, y - chassisSize/2);
            trajectoryLines[1].setBaseLineLength(swerveModules[0].getDrive() * 100);
            trajectoryLines[1].setAngleLineLength(swerveModules[0].getDrive() * 100);
            trajectoryLines[1].setAngle(swerveModules[0].getAngle());
            trajectoryLines[1].Draw(spriteBatch);

            //Back left
            swerveModules[2].Draw((int) (x - chassisSize/2), (int) (y + chassisSize/2), spriteBatch);
            trajectoryLines[2].setPosition(x - chassisSize/2, y + chassisSize/2);
            trajectoryLines[2].setBaseLineLength(swerveModules[0].getDrive() * 100);
            trajectoryLines[2].setAngleLineLength(swerveModules[0].getDrive() * 100);
            trajectoryLines[2].setAngle(swerveModules[0].getAngle());
            trajectoryLines[2].Draw(spriteBatch);

            //Back right
            swerveModules[3].Draw((int) (x + chassisSize/2), (int) (y + chassisSize/2), spriteBatch);
            trajectoryLines[3].setPosition(x + chassisSize/2, y + chassisSize/2);
            trajectoryLines[3].setBaseLineLength(swerveModules[0].getDrive() * 100);
            trajectoryLines[3].setAngleLineLength(swerveModules[0].getDrive() * 100);
            trajectoryLines[3].setAngle(swerveModules[0].getAngle());
            trajectoryLines[3].Draw(spriteBatch);
        }
    }
}
