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
    internal class SwerveModuleState
    {
        float angleDegrees;
        float velocity;
        Vector2 vectorDirection;

        public SwerveModuleState(float angleDegrees, float velocity, Vector2 vectorDirection)
        {
            this.angleDegrees = angleDegrees;
            this.velocity = velocity;
            this.vectorDirection = vectorDirection;
        }

        public Vector2 getVectorDirection()
        {
            return vectorDirection;
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
