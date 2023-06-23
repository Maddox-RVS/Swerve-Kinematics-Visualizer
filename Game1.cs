using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace SwerveVisualizer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D swerveModuleTexture, robotFrameTexture, GUIOverlayTexture, ProgressBarTexture, ProgressBarFillTexture;
        private float leftY, leftX, rightY, rightX;
        private GamePadState controller;
        private Robot robot;
        private ProgressBar leftXBar, leftYBar, rightXBar;
        private SpriteFont progressBarFont, progressBarNameFont;
        private AngleMarker leftTrajectory, rightTrajectory;
        private Kinematics kinematics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = (1600 / 3) * 2;
            _graphics.PreferredBackBufferHeight = (900 / 3) * 2;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "Swerve Visualizer";

            //GLOBALS
            Globals.Window.WIDTH = _graphics.PreferredBackBufferWidth;
            Globals.Window.HEIGHT = _graphics.PreferredBackBufferHeight;

            kinematics = new Kinematics();

            leftTrajectory = new AngleMarker(
                711, 300,
                90,
                250,
                5,
                Color.Black,
                Color.Yellow,
                AngleMarker.Spin.COUNTER_CLOCKWISE,
                0,
                0,
                _graphics.GraphicsDevice);
            rightTrajectory = new AngleMarker(
                711, 300,
                90,
                250,
                5,
                Color.Black,
                Color.Blue,
                AngleMarker.Spin.COUNTER_CLOCKWISE,
                0,
                0,
                _graphics.GraphicsDevice);
            rightTrajectory.setBaseLineLength(0);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //GLOBALS
            swerveModuleTexture = Content.Load<Texture2D>("Robot\\SwerveModule");
            robotFrameTexture = Content.Load<Texture2D>("Robot\\RobotFrame");
            GUIOverlayTexture = Content.Load<Texture2D>("GUI\\GUIOverlay");
            ProgressBarTexture = Content.Load<Texture2D>("GUI\\ProgressBar");
            ProgressBarFillTexture = Content.Load<Texture2D>("GUI\\ProgressBarFilling");

            progressBarFont = Content.Load<SpriteFont>("Fonts\\ProgressBar");
            progressBarNameFont = Content.Load<SpriteFont>("Fonts\\ProgressBarName");

            robot = new Robot(swerveModuleTexture, 711, 300, 200, _graphics.GraphicsDevice);

            leftXBar = new ProgressBar(-1, 1, (355/2) - (177/2), 100, ProgressBarTexture, ProgressBarFillTexture);
            leftYBar = new ProgressBar(-1, 1, (355/2) - (177/2), 200, ProgressBarTexture, ProgressBarFillTexture);
            rightXBar = new ProgressBar(-1, 1, (355/2) - (177/2), 300, ProgressBarTexture, ProgressBarFillTexture);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            updateJoystickValues();

            robot.updateModuleStates(leftY, leftX, rightX);

            leftXBar.Update();
            leftYBar.Update();
            rightXBar.Update();

            leftXBar.setValue(leftX);
            leftYBar.setValue(leftY);
            rightXBar.setValue(rightX);

            leftTrajectory.setAngle(kinematics.convertSlopeToDegrees(leftY, leftX));
            leftTrajectory.setAngleLineLength(Math.Clamp(250 * kinematics.convertSlopeToHypotenuse(leftY, leftX), 0, 250));

            rightTrajectory.setAngle(kinematics.convertSlopeToDegrees(rightY, rightX));
            rightTrajectory.setAngleLineLength(Math.Clamp(250 * kinematics.convertSlopeToHypotenuse(rightY, rightX), 0, 250));

            base.Update(gameTime);
        }

        public void updateJoystickValues()
        {
            controller = GamePad.GetState(PlayerIndex.One);

            if (controller.IsConnected)
            {
                leftY = controller.ThumbSticks.Left.Y;
                leftX = controller.ThumbSticks.Left.X;
                rightY = controller.ThumbSticks.Right.Y;
                rightX = controller.ThumbSticks.Right.X;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSlateGray);
            _spriteBatch.Begin();

            _spriteBatch.Draw(GUIOverlayTexture, new Vector2(0, 0), Color.SlateGray);

            leftXBar.Draw(_spriteBatch, progressBarFont, Color.SlateGray);
            leftYBar.Draw(_spriteBatch, progressBarFont, Color.SlateGray);
            rightXBar.Draw(_spriteBatch, progressBarFont, Color.SlateGray);

            _spriteBatch.DrawString(progressBarNameFont, "Left Joystick X:", new Vector2(leftXBar.getPosition().X + 5, leftXBar.getPosition().Y - 30), Color.White);
            _spriteBatch.DrawString(progressBarNameFont, "Left Joystick Y:", new Vector2(leftYBar.getPosition().X + 5, leftYBar.getPosition().Y - 30), Color.White);
            _spriteBatch.DrawString(progressBarNameFont, "Right Joystick X:", new Vector2(rightXBar.getPosition().X + 5, rightXBar.getPosition().Y - 30), Color.White);

            //_spriteBatch.DrawString(progressBarNameFont, "Angle: " + leftTrajectory.getAngle().ToString("0.000"), new Vector2(rightXBar.getPosition().X + 5, rightXBar.getPosition().Y + 100), Color.White);
            //_spriteBatch.DrawString(progressBarNameFont, "Strength: " + (leftTrajectory.getAngleLineLength()/250).ToString("0.000"), new Vector2(rightXBar.getPosition().X + 5, rightXBar.getPosition().Y + 130), Color.White);

            robot.Draw(_spriteBatch);
            leftTrajectory.Draw(_spriteBatch);
            rightTrajectory.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}