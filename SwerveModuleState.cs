using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwerveVisualizer
{
    internal class SwerveModuleState
    {
        float angleDegrees;
        float velocity;

        public SwerveModuleState(float angleDegrees, float velocity)
        {
            this.angleDegrees = angleDegrees;
            this.velocity = velocity;
        }

        public float getAngleDegrees()
        {
            return angleDegrees;
        }

        public void setAngleDegrees(float angleDegrees)
        {
            this.angleDegrees = angleDegrees;
        }

        public float getVelocity()
        {
            return velocity;
        }

        public void setVelocity(float velocity)
        {
            this.velocity = velocity;
        }
    }
}
