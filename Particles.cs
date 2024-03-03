using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
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
    public class Particles : StoryboardObjectGenerator
    {
        [Configurable]
        public string SpritePath = "sb/p.png";
        [Configurable]
        public double StartTime;
        [Configurable]
        public double EndTime;
        [Configurable]
        public Color4 ParticleColor = Color4.White;
        [Configurable]
        public float ColorVariance = 0.6f;
        [Configurable]
        public double ParticleScale = 0.01;
        [Configurable]
        public int ParticleCount = 100;
        [Configurable]
        public double moveDistance = 50;
        [Configurable]
        public double movingDuration = 4000;
        [Configurable]
        public double opacity = 0.33;
        [Configurable]
        public bool isAdditive = true;
        [Configurable]
        public bool isRotate = true;
        


        public override void Generate()
        {
            Bitmap particleBitmap = GetMapsetBitmap(SpritePath);
            var height = (float)particleBitmap.Height;
            var width = (float)particleBitmap.Width;
            for (int i = 0; i < ParticleCount; i++)
            {
                var startX = Random(-107d, 747d);
                var startY = Random(30d, 480d-30d);

                var movingX = Random(moveDistance, moveDistance * 2);
                var movingY = Random(moveDistance /2, moveDistance);

                var endY = startY + Random(-movingY, movingY);
                var randomDuration = Random(movingDuration, movingDuration * 2);
                var randomScale = Random(0.5, 2d) * ParticleScale / height;
                var randomOpacity =MathHelper.Clamp(Random(0.5, 2) * opacity, 0, 1);
                
                var randomStartTime = Random(StartTime, EndTime- randomDuration);

                var particle = GetLayer("").CreateSprite(SpritePath, OsbOrigin.Centre, new Vector2((float)startX, (float)startY));
                particle.Fade(OsbEasing.OutSine, randomStartTime, randomStartTime + randomDuration / 5, 0, randomOpacity);
                particle.Fade(OsbEasing.InSine, EndTime - randomDuration / 5, EndTime, randomOpacity, 0);
                if (isAdditive)
                {
                    particle.Additive(randomStartTime, EndTime);
                }

                var color = ParticleColor;
                if (ColorVariance > 0)
                {
                    ColorVariance = MathHelper.Clamp(ColorVariance, 0, 1);

                    var hsba = Color4.ToHsl(color);
                    var sMin = Math.Max(0, hsba.Y - ColorVariance * 0.5f);
                    var sMax = Math.Min(sMin + ColorVariance, 1);
                    var vMin = Math.Max(0, hsba.Z - ColorVariance * 0.5f);
                    var vMax = Math.Min(vMin + ColorVariance, 1);

                    color = Color4.FromHsl(new Vector4(
                        hsba.X,
                        (float)Random(sMin, sMax),
                        (float)Random(vMin, vMax),
                        hsba.W));
                }
                if (color.R != 1 || color.G != 1 || color.B != 1)
                    particle.Color(randomStartTime, color);
                var loopCount = (int)Math.Ceiling((EndTime - randomStartTime) / randomDuration);
                particle.StartLoopGroup(randomStartTime, loopCount);
                particle.MoveY(OsbEasing.InOutSine, 0, randomDuration, startY, endY);
                particle.MoveX(OsbEasing.None, 0, randomDuration, startX, startX + movingX);
                particle.Scale(OsbEasing.OutSine, 0, randomDuration/10, 0, randomScale);
                particle.Scale(OsbEasing.InSine,randomDuration - randomDuration/10, randomDuration, randomScale, 0);
                if (isRotate)
                {
                    particle.Rotate(OsbEasing.InOutSine, 0, randomDuration, 0, MathHelper.DegreesToRadians(Random(0, 360)));
                }
                particle.EndGroup();
            }

        }
    }
}
