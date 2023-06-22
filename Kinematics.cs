using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwerveVisualizer
{
    internal class Kinematics
    {
        public float convertSlopeToDegrees(float rise, float run)
        {
            //Add forward/straith

            //double hypotenuse = Math.Sqrt(Math.Pow(Math.Abs(rise), 2) + Math.Pow(Math.Abs(run), 2));
            ////a^2 = b^2 + c^2 - (2bc * CosA)
            ////a = rise
            ////b = run
            ////c = hyp
            //double temp1 = Math.Pow(run, 2) + Math.Pow(hypotenuse, 2); //b^2 + c^2
            //double temp2 = 2 * run * hypotenuse; //2bc
            //double temp3 = Math.Pow(rise, 2) - temp1; //a^2 - (b^2 + c^2)
            //double temp4 = temp3 / -temp2; //(a^2 - (b^2 + c^2)) / -(2bc)
            //double temp5 = Math.Acos(temp4); //Inverse Cos of (a^2 - (b^2 + c^2)) / -(2bc)
            //double angle = temp5 * (180 / Math.PI); //Convert radians to degrees
            //return (float)angle;

            double hypotenuse = Math.Pow(rise, 2) + Math.Pow(run, 2);
            double temp = Math.Asin(Math.Abs(rise)/hypotenuse);
            double angle = temp * (180/Math.PI);

            if (rise == 0 && run == 0) return 0;
            else if (rise >= 0 && run >= 0 && double.IsNaN(angle)) return 90;
            else if (rise >= 0 && run >= 0) return (float) angle;
            else if (rise >= 0 && run < 0) return (float) (90 + (90 - angle));
            else if (rise < 0 && run < 0) return (float) (angle + 180);
            else if (rise < 0 && run >= 0 && double.IsNaN(angle)) return 270;
            else if (rise < 0 && run >= 0) return (float) (360 - angle);
            else return (float) 0;
        }

        public float convertSlopeToHypotenuse(float rise, float run)
        {
            double hypotenuse = Math.Pow(rise, 2) + Math.Pow(run, 2);
            return (float) hypotenuse;
        }
    }
}
