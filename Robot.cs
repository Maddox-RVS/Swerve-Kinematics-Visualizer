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
        private AngleMarker[] velocityLines;
        private float x, y, chassisSize;

        public Robot(Texture2D swerveModuleTexture, float x, float y, float chassisSize, GraphicsDevice graphicsDevice)
        {
            //Front right
            SwerveModule module0 = new SwerveModule(0, 0, swerveModuleTexture);

            //Front left
            SwerveModule module1 = new SwerveModule(0, 1, swerveModuleTexture);

            //Back left
            SwerveModule module2 = new SwerveModule(0, 2, swerveModuleTexture);

            //Back right
            SwerveModule module3 = new SwerveModule(0, 3, swerveModuleTexture);

            swerveModules = new SwerveModule[] { module0, module1, module2, module3 };
            swerveModuleStates = new SwerveModuleState[swerveModules.Length];

            AngleMarker velocityLineSetup = new AngleMarker(
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
            velocityLineSetup.setShowArrowHead(true);
            velocityLines = new AngleMarker[] {
                velocityLineSetup,
                velocityLineSetup,
                velocityLineSetup,
                velocityLineSetup};

            this.x = x;
            this.y = y;
            this.chassisSize = chassisSize;
        }

        public void updateModuleStates(float forwardVelocity, float straithVelocity, float rotationVelocity)
        {
            //// Make robot field relative
            ////temp = FWD·cos(θ) + STR·sin(θ);
            ////STR = -FWD·sin(θ) + STR·cos(θ);
            ////FWD = temp; 

            //float temp = forwardVelocity * ((float) Math.Cos(gyro)) + straithVelocity * ((float) Math.Sin(gyro));
            //straithVelocity = -forwardVelocity * ((float) Math.Sin(gyro)) + straithVelocity * ((float) Math.Cos(gyro));
            //forwardVelocity = temp;

            ////Convert the vehicle motion commands into wheel speed and angle commands (inverse kinematics)
            ////L is the vehicle’s wheelbase
            ////W is the vehicle’s trackwidth
            ////R = sqrt(L2+W2);

            //float L = 20.75f;
            //float W = 20.75f;
            //float R = (float) (Math.Sqrt(Math.Pow(L, 2) + Math.Pow(W, 2)));

            ////A = STR - RCW·(L/R);
            ////B = STR + RCW·(L/R);
            ////C = FWD - RCW·(W/R);
            ////D = FWD + RCW·(W/R);
            ////ws1 = sqrt(B2+C2); wa1 = atan2(B,C)·180/pi;
            ////ws2 = sqrt(B2+D2); wa2 = atan2(B,D)·180/pi;
            ////ws3 = sqrt(A2+D2); wa3 = atan2(A,D)·180/pi;
            ////ws4 = sqrt(A2+C2); wa4 = atan2(A,C)·180/pi; 

            //float A = straithVelocity - rotationVelocity * (L/R);
            //float B = straithVelocity + rotationVelocity * (L/R);
            //float C = forwardVelocity - rotationVelocity * (W/R);
            //float D = forwardVelocity + rotationVelocity * (W/R);

            //float ws1 = (float) (Math.Sqrt(Math.Pow(B, 2) + Math.Pow(C, 2)));
            //float ws2 = (float) (Math.Sqrt(Math.Pow(B, 2) + Math.Pow(D, 2)));
            //float ws3 = (float) (Math.Sqrt(Math.Pow(A, 2) + Math.Pow(D, 2)));
            //float ws4 = (float) (Math.Sqrt(Math.Pow(A, 2) + Math.Pow(C, 2)));

            //float wa1 = (float) (Math.Atan2(B, C) * 1809/Math.PI);
            //float wa2 = (float) (Math.Atan2(B, D) * 1809/Math.PI);
            //float wa3 = (float) (Math.Atan2(A, D) * 1809/Math.PI);
            //float wa4 = (float) (Math.Atan2(A, C) * 1809/Math.PI);

            ////Normalize wheel speeds
            ////max=ws1; 
            ////if(ws2>max)max=ws2; 
            ////if(ws3>max)max=ws3; 
            ////if(ws4>max)max=ws4;
            ////if(max>1){ws1/=max; ws2/=max; ws3/=max; ws4/=max;} 

            //float max = ws1;
            //if (ws2 > max) max = ws2;
            //if (ws3 > max) max = ws3;
            //if (ws4 > max) max = ws4;
            //if (max > 1)
            //{
            //    ws1 /= max;
            //    ws2 /= max;
            //    ws3 /= max;
            //    ws4 /= max;
            //}

            ////Update the modules

            //swerveModules[0].setDrive(ws3);
            //swerveModules[0].setAngle(wa3);

            //swerveModules[1].setDrive(ws2);
            //swerveModules[1].setAngle(wa2);

            //swerveModules[2].setDrive(ws1);
            //swerveModules[2].setAngle(wa1);

            //swerveModules[3].setDrive(ws4);
            //swerveModules[3].setAngle(wa4);

            //---------------------------------

            //if (rotationVelocity > 0) angleVelocityLines[0].setAngle(135);
            //else if (rotationVelocity < 0) angleVelocityLines[0].setAngle(315);
            //else angleVelocityLines[0].setAngle(Trigonometry.convertSlopeToDegrees(forwardVelocity, straithVelocity));
            //angleVelocityLines[0].setAngleLineLength(100 * Math.Abs(rotationVelocity));

            //driveVelocityLines[0].setAngle(Trigonometry.convertSlopeToDegrees(forwardVelocity, straithVelocity));
            //driveVelocityLines[0].setAngleLineLength(100 * Trigonometry.convertSlopeToHypotenuse(forwardVelocity, straithVelocity));
            //driveVelocityLines[0].setPosition(
            //    angleVelocityLines[0].getPosition().X - Trigonometry.getHypotenuseLegPoint(angleVelocityLines[0].getAngleLineLength(), angleVelocityLines[0].getAngle()).X,
            //    angleVelocityLines[0].getPosition().Y + Trigonometry.getHypotenuseLegPoint(angleVelocityLines[0].getAngleLineLength(), angleVelocityLines[0].getAngle()).Y);

            //angleVelocityAbsoluteAngle.setAngle(angleVelocityLines[0].getAngle());
            //driveVelocityAbsoluteAngle.setAngle(driveVelocityLines[0].getAngle());
            //float angleDifference = Math.Abs(angleVelocityAbsoluteAngle.getAngle() - driveVelocityAbsoluteAngle.getAngle());

            //trajectoryLines[0].setPosition(angleVelocityLines[0].getPosition().X, angleVelocityLines[0].getPosition().Y);
            //trajectoryLines[0].setAngleLineLength(Trigonometry.getMissingSideLength(angleDifference, angleVelocityLines[0].getAngleLineLength(), driveVelocityLines[0].getAngleLineLength()));
            //trajectoryLines[0].setAngle(
            //    Trigonometry.getLineAngle(
            //        Trigonometry.getHypotenuseLegPoint(driveVelocityLines[0].getAngleLineLength(), driveVelocityLines[0].getAngle()).Y,
            //        angleVelocityLines[0].getPosition().Y,
            //        Trigonometry.getHypotenuseLegPoint(driveVelocityLines[0].getAngleLineLength(), driveVelocityLines[0].getAngle()).X,
            //        angleVelocityLines[0].getPosition().X));

            //trajectoryLines[0].setAngle(Trigonometry.convertSlopeToDegrees(5, -1));

            //driveVelocityAbsoluteAngle.setAngle(driveVelocityLines[0].getAngle());
            //angleVelocityAbsoluteAngle.setAngle(angleVelocityLines[0].getAngle());
            //trajVelocityAbsoluteAngle.setAngle(trajectoryLines[0].getAngle());

            //---------------------------------

            for (int i = 0; i < 4; i++) {
                swerveModuleStates[i] = Kinematics.updateSwerveModuleState(
                    straithVelocity, 
                    forwardVelocity, 
                    rotationVelocity, 
                    Globals.Swerve.MAX_VELOCITY, 
                    Globals.Swerve.MAX_ANGULAR_VELOCITY, 
                    i);
            }
            swerveModuleStates = Kinematics.desaturateModuleStates(swerveModuleStates);

            foreach (SwerveModule module in swerveModules)
            {
                module.setDrive(swerveModuleStates[module.moduleNumber].getVelocity());
                module.setAngle(swerveModuleStates[module.moduleNumber].getAngleDegrees());
                velocityLines[module.moduleNumber].setAngleLineLength(module.getDrive() * 20);
                velocityLines[module.moduleNumber].setAngle(module.getAngle());
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
            //Front right
            swerveModules[0].Draw((int) (x + chassisSize/2), (int) (y - chassisSize/2), spriteBatch);
            velocityLines[0].setPosition((int) (x + chassisSize/2), (int) (y - chassisSize/2));
            velocityLines[0].Draw(spriteBatch);

            //Front left
            swerveModules[1].Draw((int) (x - chassisSize/2), (int) (y - chassisSize/2), spriteBatch);
            velocityLines[1].setPosition((int) (x - chassisSize/2), (int) (y - chassisSize/2));
            velocityLines[1].Draw(spriteBatch);

            //Back left
            swerveModules[2].Draw((int) (x - chassisSize/2), (int) (y + chassisSize/2), spriteBatch);
            velocityLines[2].setPosition((int) (x - chassisSize/2), (int) (y + chassisSize/2));
            velocityLines[2].Draw(spriteBatch);

            //Back right
            swerveModules[3].Draw((int) (x + chassisSize/2), (int) (y + chassisSize/2), spriteBatch);
            velocityLines[3].setPosition((int) (x + chassisSize/2), (int) (y + chassisSize/2));
            velocityLines[3].Draw(spriteBatch);
        }
    }
}
