using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SwerveVisualizer
{
    internal class Trigonometry
    {
        public static float convertSlopeToDegrees(float rise, float run)
        {
            ////a^2 = b^2 + c^2 - (2bc * CosA)
            ////a = rise
            ////b = run
            ////c = hyp
            
            double hypotenuse = Math.Sqrt(Math.Pow(Math.Abs(rise), 2) + Math.Pow(Math.Abs(run), 2));
            
            double temp1 = Math.Pow(run, 2) + Math.Pow(hypotenuse, 2); //b^2 + c^2
            double temp2 = 2 * run * hypotenuse; //2bc
            double temp3 = Math.Pow(rise, 2) - temp1; //a^2 - (b^2 + c^2)
            double temp4 = temp3 / -temp2; //(a^2 - (b^2 + c^2)) / -(2bc)
            double temp5 = Math.Acos(temp4); //Inverse Cos of (a^2 - (b^2 + c^2)) / -(2bc)
            double angle = temp5 * (180 / Math.PI); //Convert radians to degrees

            if (rise == 0 && run == 0) return float.NaN;
            else if (rise >= 0 && run == 0 && double.IsNaN(angle)) return 90;
            else if (rise < 0 && run == 0 && double.IsNaN(angle)) return 270;
            else if (rise < 0) return (float) (360 - angle);
            return (float) angle;
        }

        public static float convertSlopeToHypotenuse(float rise, float run)
        {
            double hypotenuse = Math.Pow(rise, 2) + Math.Pow(run, 2);
            return (float) hypotenuse;
        }

        public static float getMissingSideLength(float theta, float side1, float side2)
        {
            ////c^2 = a^2 + b^2 - (2ab * CosC)
            ////a = side1
            ////b = side2
            ////C = theta
            
            double temp1 = Math.Pow(side1, 2) + Math.Pow(side2, 2); //a^2 + b^2
            double temp2 = 2 * side1 * side2; //2ab
            double temp3 = Math.Cos(theta); //CosC
            double temp4 = temp3 * (180 / Math.PI); //Convert radians to degrees
            double temp5 = temp2 * temp4; //2ab * CosC
            double temp6 = temp1 - temp5; //a^2 + b^2 - (2ab * CosC)
            double missingSideLength = Math.Sqrt(temp6);
            return (float) missingSideLength;
        }

        public static Vector2 getHypotenuseLegPoint(float hypotenuseLength, float angle)
        {
            double originalAngle = angle;

            if (angle > 270) angle -= 270; 
            else if (angle > 180) angle -= 180;
            else if (angle > 90) angle -= 90;

            double angleInRadians = angle * Math.PI / 180;
            double SinTheta = Math.Sin(angleInRadians);
            double CosTheta = Math.Cos(angleInRadians);

            double rise = hypotenuseLength * SinTheta;
            double run = hypotenuseLength * CosTheta;

            if (originalAngle > 270)
            {
                double temp = rise;
                rise = run;
                run = temp;
                rise *= -1;
            }
            else if (originalAngle > 180)
            {
                rise *= -1;
                run *= -1;
            }
            else if (originalAngle > 90)
            {
                double temp = rise;
                rise = run;
                run = temp;
                run *= -1;
            }

            return new Vector2((float) run, (float) rise);
        }

        public static Vector2 getSlope(float y2, float y1, float x2, float x1)
        {
            return new Vector2(x2-x1, y2-y1);
        }
    }
}
