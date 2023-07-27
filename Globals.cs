﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace SwerveVisualizer
{
    internal class Globals
    {
        public static class Window
        {
            public static int WIDTH;
            public static int HEIGHT;
        }

        public static class Swerve
        {
            public static float MAX_VELOCITY = 5.5f;
            public static float MAX_ANGULAR_VELOCITY = 5.5f;
        }
    }
}
