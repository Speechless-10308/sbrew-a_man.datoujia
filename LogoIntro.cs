using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{
    public class LogoIntro : StoryboardObjectGenerator
    {
        #region Sprite Path
        public string LogoPath = "sb/logo.jpg";
        public string circlePath = "sb/c.png";
        public string noisePath = "sb/realnoise/noise_.png";
        public string noisePathForBitmap = "sb/realnoise/noise_1.png";
        public string squareWallPath = "sb/square_wall1.png";
        public string grayBackPath = "sb/gray_back.jpg";
        #endregion
        public override void Generate()
        {
            #region Sprite Bitmap
            var logoBitmap = GetMapsetBitmap(LogoPath);
            var circleBitmap = GetMapsetBitmap(circlePath);
            var noiseBitmap = GetMapsetBitmap(noisePathForBitmap);
            var squareWallBitmap = GetMapsetBitmap(squareWallPath);
            var grayBackBitmap = GetMapsetBitmap(grayBackPath);
            #endregion

            #region Loading Animation Settings
            var startTime = 1057;
            var endTime = 79424;
            double aniTime = 5000;
            var circleCount = 5;
            float circlePixel = 4;
            var radius = 30;
            var loadingPosition = new Vector2(320, 240);
            var timeStep = 1;
            CubicBezier cubicBezier = new CubicBezier(0.7f, 0.18f, 0.3f, 0.82f);
            #endregion

            var layer = GetLayer("Loading");
            var grayBack = layer.CreateSprite(grayBackPath, OsbOrigin.Centre);
            grayBack.Scale(startTime, Math.Max(854.0f / grayBackBitmap.Width, 480.0f / grayBackBitmap.Height));
            grayBack.Fade(startTime, endTime, 0, 0.3);

            Loading(circleBitmap, startTime, endTime, aniTime, circleCount, circlePixel, radius, loadingPosition, timeStep, cubicBezier);
            
            var noise = layer.CreateAnimation(noisePath, 4,1000/15f, OsbLoopType.LoopForever, OsbOrigin.Centre);
            noise.Scale(startTime, 854.0f / noiseBitmap.Width);
            noise.Fade(startTime, (startTime + endTime)/2, 0, 0.2);
            noise.Fade(endTime, 0);

            var p = layer.CreateSprite("sb/p.png", OsbOrigin.Centre);
            p.ScaleVec(startTime, 854.0f, 480.0f);
            p.Color(startTime, Color4.Black);
            p.Fade(startTime, (startTime + endTime)/2, 0.5, 0);

            
        }

        private void Loading(Bitmap circleBitmap, int startTime, int endTime, double aniTime, int circleCount, float circlePixel, int radius, Vector2 loadingPosition, int timeStep, CubicBezier cubicBezier)
        {

            // record the animation keyframe for the loading animation, it is like win10 loading animation. btw, set a different direction, I am scared!!!!
            var loadingAnimation = new KeyframedValue<Vector2>(null);
            for (double time = 0; time < aniTime; time += timeStep)
            {
                var t = time / (aniTime / 3) % 1.0;
                var percentage = cubicBezier.Solve(t);
                Log(percentage);
                var angle = percentage * Math.PI * 2 + Math.PI;
                var position = loadingPosition + new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) * radius;

                loadingAnimation.Add(time, position);
            }

            loadingAnimation.Simplify2dKeyframes(0.8, p => p);

            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < circleCount; i++)
                {
                    var circle = GetLayer("Loading").CreateSprite(circlePath, OsbOrigin.Centre, loadingPosition);
                    circle.Scale(startTime, circlePixel / circleBitmap.Width);

                    var realStartTime = startTime + i * (aniTime / 3 / 15) - aniTime / 3 / 2 + j * (aniTime - aniTime / 3 / 2);
                    var loopCount = (int)Math.Ceiling((endTime - realStartTime) / (aniTime * 5));
                    circle.Scale(endTime, 0);
                    circle.StartLoopGroup(realStartTime, loopCount);
                    circle.Fade(0, 0);
                    circle.Fade(aniTime / 3 / 2, 1);
                    circle.Fade(aniTime - aniTime / 3 / 2, 0);
                    circle.Fade(aniTime * 5, 0);
                    loadingAnimation.ForEachPair((st, et) =>
                    {
                        circle.Move(st.Time, et.Time, st.Value, et.Value);
                    });
                    circle.EndGroup();
                }
            }
        }
    }
    
}
