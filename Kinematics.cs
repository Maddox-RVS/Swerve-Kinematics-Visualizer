using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SwerveVisualizer
{
    internal class Kinematics
    {
        public enum CoordinatePlane{
            QUADRANT_1,
            QUADRANT_2, 
            QUADRANT_3, 
            QUADRANT_4,
            NONE
        }

        public static SwerveModuleState updateSwerveModuleState(float leftX, float leftY, float rightX, float maxVelocity, float maxAngularVelocity, int moduleNumber)
        {
            //Upscale joystick input to correct percentage of max velocities
            leftX *= maxVelocity;
            leftY *= maxVelocity;
            rightX *= maxAngularVelocity;

            //Create vectors
            Vector2 velocityVector = new Vector2(leftX, leftY);
            Vector2 angularVelocityVector;
            
            if (moduleNumber == 0) angularVelocityVector = new Vector2(rightX, -rightX);
            else if (moduleNumber == 1) angularVelocityVector = new Vector2(rightX, rightX);
            else if (moduleNumber == 2) angularVelocityVector = new Vector2(-rightX, rightX);
            else if (moduleNumber == 3) angularVelocityVector = new Vector2(-rightX, -rightX);
            else return new SwerveModuleState(0, 0, new Vector2(0, 0));

            Vector2 finalVelocityVector = new Vector2(
                angularVelocityVector.X + velocityVector.X, 
                angularVelocityVector.Y + velocityVector.Y);

            //Get length of the finalVelocityVector
            double lengthSquared = Math.Pow(finalVelocityVector.X, 2) + Math.Pow(finalVelocityVector.Y, 2);
            double finalVelocity = Math.Sqrt(lengthSquared);

            //Get angle of the finalVelocityVector
            if (finalVelocityVector.X > 0 && finalVelocityVector.Y == 0)
                return new SwerveModuleState(90, (float) finalVelocity, finalVelocityVector);
            else if (finalVelocityVector.X < 0 && finalVelocityVector.Y == 0)
                return new SwerveModuleState(270, (float) finalVelocity, finalVelocityVector);
            else if (finalVelocityVector.Y < 0 && finalVelocityVector.X == 0)
                return new SwerveModuleState(180, (float) finalVelocity, finalVelocityVector);
            else if (finalVelocityVector.Y > 0 && finalVelocityVector.X == 0)
                return new SwerveModuleState(0, (float) finalVelocity, finalVelocityVector);

            CoordinatePlane quadrant;
            if (finalVelocityVector.X > 0 && finalVelocityVector.Y > 0)
                quadrant = CoordinatePlane.QUADRANT_1;
            else if (finalVelocityVector.X < 0 && finalVelocityVector.Y > 0)
                quadrant = CoordinatePlane.QUADRANT_2;
            else if (finalVelocityVector.X < 0 && finalVelocityVector.Y < 0)
                quadrant = CoordinatePlane.QUADRANT_3;
            else if (finalVelocityVector.X > 0 && finalVelocityVector.Y < 0)
                quadrant = CoordinatePlane.QUADRANT_4;
            else quadrant = CoordinatePlane.NONE;

            double finalAngle = Math.Asin(Math.Abs(finalVelocityVector.Y)/Math.Abs(finalVelocity));
            finalAngle = (float) (finalAngle * (180 / Math.PI));  

            if (quadrant == CoordinatePlane.QUADRANT_1) finalAngle = 90 - finalAngle;
            else if (quadrant == CoordinatePlane.QUADRANT_2) finalAngle += 270;
            else if (quadrant == CoordinatePlane.QUADRANT_3) finalAngle = (90 - finalAngle) + 180;
            else if (quadrant == CoordinatePlane.QUADRANT_4) finalAngle += 90;

            //Return new state
            return new SwerveModuleState((float) finalAngle, (float) finalVelocity, finalVelocityVector);
        }

        public static void desaturateModuleStates(SwerveModuleState[] moduleStates)
        {
            //Get all four velocities
            float wv0 = moduleStates[0].getVelocity();
            float wv1 = moduleStates[1].getVelocity();
            float wv2 = moduleStates[2].getVelocity();
            float wv3 = moduleStates[3].getVelocity();

            //Get the largest velocity
            float largest = 0;
            for (int i = 0; i < 4; i++)
            {
                if (moduleStates[i].getVelocity() > largest) largest = moduleStates[i].getVelocity();
            }

            //If largest velocity is greater than max velocity then desaturate
            if (largest > Globals.Swerve.MAX_VELOCITY)
            {
                foreach (SwerveModuleState state in moduleStates)
                {
                    state.setVelocity(state.getVelocity() / largest * Globals.Swerve.MAX_VELOCITY);
                }
            }
        }

        public static Vector2 addVectors2(Vector2[] vectors)
        {
            Vector2 total = new Vector2(0, 0);
            foreach (Vector2 vector in vectors)
            {
                total += vector;
            }
            return total;
        }
    }
}
