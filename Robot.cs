using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SwerveVisualizer
{
    internal class Robot
    {
        private SwerveModule[] swerveModules;
        private AngleMarker[] driveVelocityLines, angleVelocityLines, trajectoryLines;
        private float x, y, chassisSize;
        private Texture2D lineThing;
        private AngleMarker angleVelocityAbsoluteAngle, driveVelocityAbsoluteAngle;

        public Robot(Texture2D swerveModuleTexture, float x, float y, float chassisSize, GraphicsDevice graphicsDevice)
        {
            //Front right
            SwerveModule module0 = new SwerveModule(0, swerveModuleTexture);

            //Front left
            SwerveModule module1 = new SwerveModule(0, swerveModuleTexture);

            //Back left
            SwerveModule module2 = new SwerveModule(0, swerveModuleTexture);

            //Back right
            SwerveModule module3 = new SwerveModule(0, swerveModuleTexture);

            swerveModules = new SwerveModule[] { module0, module1, module2, module3 };

            AngleMarker angleVelocityLineSetup = new AngleMarker(
                x, y,
                0,
                100,
                3,
                Color.Blue,
                Color.Red,
                AngleMarker.Spin.CLOCKWISE,
                270,
                0,
                graphicsDevice);
            angleVelocityLineSetup.setShowBaseLine(false);

            AngleMarker angleMarker0 = angleVelocityLineSetup;
            AngleMarker angleMarker1 = angleVelocityLineSetup;
            AngleMarker angleMarker2 = angleVelocityLineSetup;
            AngleMarker angleMarker3 = angleVelocityLineSetup;

            angleVelocityLines = new AngleMarker[] { angleMarker0, angleMarker1, angleMarker2, angleMarker3 };

            AngleMarker driveVelocityLineSetup = new AngleMarker(
                x, y,
                0,
                100,
                3,
                Color.Blue,
                Color.Blue,
                AngleMarker.Spin.COUNTER_CLOCKWISE,
                0,
                0,
                graphicsDevice);
            driveVelocityLineSetup.setShowBaseLine(false);

            AngleMarker driveMarker0 = driveVelocityLineSetup;
            AngleMarker driveMarker1 = driveVelocityLineSetup;
            AngleMarker driveMarker2 = driveVelocityLineSetup;
            AngleMarker driveMarker3 = driveVelocityLineSetup;

            driveVelocityLines = new AngleMarker[] { driveMarker0, driveMarker1, driveMarker2, driveMarker3 };

            AngleMarker trajectoryLineSetup = new AngleMarker(
                x, y,
                0,
                100,
                3,
                Color.Blue,
                Color.Yellow,
                AngleMarker.Spin.CLOCKWISE,
                90,
                0,
                graphicsDevice);
            trajectoryLineSetup.setShowBaseLine(false);
            trajectoryLineSetup.setShowArrowHead(true);

            AngleMarker trajectoryMarker0 = trajectoryLineSetup;
            AngleMarker trajectoryMarker1 = trajectoryLineSetup;
            AngleMarker trajectoryMarker2 = trajectoryLineSetup;
            AngleMarker trajectoryMarker3 = trajectoryLineSetup;

            trajectoryLines = new AngleMarker[] { trajectoryMarker0, trajectoryMarker1, trajectoryMarker2, trajectoryMarker3 };

            this.x = x;
            this.y = y;
            this.chassisSize = chassisSize;

            angleVelocityAbsoluteAngle = new AngleMarker(
                711, 300,
                0,
                250,
                5,
                Color.Black,
                Color.Red,
                AngleMarker.Spin.COUNTER_CLOCKWISE,
                0,
                0,
                graphicsDevice);
            driveVelocityAbsoluteAngle = new AngleMarker(
                711, 300,
                0,
                250,
                5,
                Color.Black,
                Color.Blue,
                AngleMarker.Spin.COUNTER_CLOCKWISE,
                0,
                0,
                graphicsDevice);
            driveVelocityAbsoluteAngle.setShowBaseLine(false);
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

            if (rotationVelocity > 0) angleVelocityLines[0].setAngle(315);
            else if (rotationVelocity < 0) angleVelocityLines[0].setAngle(135);
            else angleVelocityLines[0].setAngle(Trigonometry.convertSlopeToDegrees(forwardVelocity, straithVelocity));
            angleVelocityLines[0].setAngleLineLength(100 * Math.Abs(rotationVelocity));

            driveVelocityLines[0].setAngle(Trigonometry.convertSlopeToDegrees(forwardVelocity, straithVelocity));
            driveVelocityLines[0].setAngleLineLength(100 * Trigonometry.convertSlopeToHypotenuse(forwardVelocity, straithVelocity));
            driveVelocityLines[0].setPosition(
                angleVelocityLines[0].getPosition().X - Trigonometry.getHypotenuseLegPoint(angleVelocityLines[0].getAngleLineLength(), angleVelocityLines[0].getAngle()).X,
                angleVelocityLines[0].getPosition().Y + Trigonometry.getHypotenuseLegPoint(angleVelocityLines[0].getAngleLineLength(), angleVelocityLines[0].getAngle()).Y);

            angleVelocityAbsoluteAngle.setAngle(angleVelocityLines[0].getAngle());
            driveVelocityAbsoluteAngle.setAngle(driveVelocityLines[0].getAngle());
            float angleDifference = Math.Abs(angleVelocityAbsoluteAngle.getAngle() - driveVelocityAbsoluteAngle.getAngle());

            trajectoryLines[0].setPosition(angleVelocityLines[0].getPosition().X, angleVelocityLines[0].getPosition().Y);
            trajectoryLines[0].setAngleLineLength(Trigonometry.getMissingSideLength(angleDifference, angleVelocityLines[0].getAngleLineLength(), driveVelocityLines[0].getAngleLineLength())/10);

            Debug.WriteLine(angleDifference + " | " + angleVelocityLines[0].getAngleLineLength().ToString("0.000") + " | " + driveVelocityLines[0].getAngleLineLength().ToString("0.000") + " | " + trajectoryLines[0].getAngleLineLength().ToString("0.000"));
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
            angleVelocityLines[0].setPosition(x + chassisSize/2, y - chassisSize/2);
            angleVelocityLines[0].Draw(spriteBatch);
            driveVelocityLines[0].Draw(spriteBatch);
            trajectoryLines[0].Draw(spriteBatch);
            
            if (swerveModules[0].getDrive() > 0) driveVelocityLines[0].setShowArrowHead(true);
            else driveVelocityLines[0].setShowArrowHead(false);

            //Front left
            swerveModules[1].Draw((int) (x - chassisSize/2), (int) (y - chassisSize/2), spriteBatch);
            if (swerveModules[1].getDrive() > 0) driveVelocityLines[1].setShowArrowHead(true);
            else driveVelocityLines[1].setShowArrowHead(false);

            //Back left
            swerveModules[2].Draw((int) (x - chassisSize/2), (int) (y + chassisSize/2), spriteBatch);
            if (swerveModules[2].getDrive() > 0) driveVelocityLines[2].setShowArrowHead(true);
            else driveVelocityLines[2].setShowArrowHead(false);

            //Back right
            swerveModules[3].Draw((int) (x + chassisSize/2), (int) (y + chassisSize/2), spriteBatch);
            if (swerveModules[3].getDrive() > 0) driveVelocityLines[3].setShowArrowHead(true);
            else driveVelocityLines[3].setShowArrowHead(false);
        }
    }
}
