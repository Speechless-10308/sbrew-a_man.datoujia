using OpenTK;
using OpenTK.Graphics;
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
    public class FlyingParticles : StoryboardObjectGenerator
    {
        [Configurable]
        public double StartTime;
        [Configurable]
        public double EndTime;
        [Configurable]
        public string ParticlePath = "sb/p.png";
        [Configurable]
        public Color4 ParticleColor = Color4.White;
        [Configurable]
        public float ColorVariance = 0.6f;
        [Configurable]
        public double ParticleScale = 30;
        [Configurable]
        public int ParticleCount = 100;
        [Configurable]
        public double opacity = 0.33;
        [Configurable]
        public bool isAdditive = true;
        [Configurable]
        public bool isRotate = true;
        public override void Generate()
        {
		    Bitmap particleBitmap = GetMapsetBitmap(ParticlePath);
            var height = (float)particleBitmap.Height;

            for (int i=0; i<ParticleCount; i++)
            {
                var startX = Random(-300d, -107d);
                var startY = Random(120d, 480d);
                var endX = Random(747d, 900d);
                var endY = Random(0d, 480d);
                var randomDuration = Random(4000, 5400);
                var randomScale = Random(0.5, 2) * ParticleScale / height;
                var randomOpacity =MathHelper.Clamp(Random(0.5, 2) * opacity, 0, 1);
                
                var randomStartTime = Random(StartTime, (EndTime- randomDuration + StartTime) / 2);
                
                var particle = GetLayer("").CreateSprite(ParticlePath, OsbOrigin.Centre, new Vector2((float)startX, (float)startY));
                particle.Scale(randomStartTime, randomScale);
                particle.Scale(EndTime, 0);
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
                particle.MoveX(OsbEasing.None, 0, randomDuration, startX, endX);
                particle.Fade(OsbEasing.OutSine, 0, randomDuration/10, 0, randomOpacity);
                particle.Fade(OsbEasing.InSine, randomDuration - randomDuration/10, randomDuration, randomOpacity, 0);
                if (isRotate)
                {
                    particle.Rotate(OsbEasing.InOutSine, 0, randomDuration, 0, MathHelper.DegreesToRadians(Random(0, 360)));
                }
                particle.EndGroup();
            }

            
        }
    }
}
