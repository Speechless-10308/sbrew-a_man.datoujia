using OpenTK;
using OpenTK.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using StorybrewCommon.Util;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;

namespace StorybrewScripts
{
    public class Timer : StoryboardObjectGenerator
    {

        [Configurable]
        public string FontName = "Verdana";

        [Configurable]
        public string OutputPath = "sb/f/timer";


        [Configurable]
        public bool ShowMilliseconds = true;

        [Configurable]
        public float LetterSpacing = 10f;

        [Configurable]
        public float Fade = 0.8f;

        [Configurable]
        public float FadeInDelay = 0;

        [Configurable]
        public int FontSize = 26;

        [Configurable]
        public float FontScale = 0.5f;

        [Configurable]
        public Vector2 Position = new Vector2(320, 240);

        [Configurable]
        public Vector2 ColonOffset = new Vector2(4, 3);

        [Configurable]
        public Color4 FontColor = Color4.White;

        [Configurable]
        public FontStyle FontStyle = FontStyle.Regular;

        [Configurable]
        public int GlowRadius = 0;

        [Configurable]
        public Color4 GlowColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public bool AdditiveGlow = true;

        [Configurable]
        public int OutlineThickness = 0;

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public int ShadowThickness = 4;

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 200);

        [Configurable]
        public Vector2 Padding = Vector2.Zero;

        [Configurable]
        public bool TrimTransparency = true;

        [Configurable]
        public bool EffectsOnly = false;

        [Configurable]
        public bool Debug = false;

        [Configurable]
        public bool Additive = false;

        [Configurable]
        public bool RandomColor = false;

        [Configurable]
        public Color4 MinColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 MaxColor = new Color4(100, 100, 100, 255);

        public FontGenerator GetFont()
        {
            return LoadFont(OutputPath, new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = FontStyle,
                TrimTransparency = TrimTransparency,
                EffectsOnly = EffectsOnly,
                Debug = Debug,
            },
            new FontGlow()
            {
                Radius = AdditiveGlow ? 0 : GlowRadius,
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });
        }

        public override void Generate()
        {
            // GenerateAnimation();
            ShowTimer(0, 1171447, new Vector2(Position.X - 55, Position.Y - 5));
        }

        private void ShowTimer(double sTime, double eTime, Vector2 position)
        {
            var layer = GetLayer("");

            if (!ShowMilliseconds)
            {
                var delay = new int[] { 600000, 60000, 0, 10000, 1000 };
                for (int i = 0; i < delay.Count(); i++)
                {

                    OsbSprite sprite;
                    if (delay[i] <= 0)
                        sprite = layer.CreateSprite(OutputPath + "/t.png", OsbOrigin.TopLeft, position + new Vector2(LetterSpacing * i + ColonOffset.X * FontScale, ColonOffset.Y * FontScale));
                    else
                        sprite = layer.CreateAnimation(OutputPath + "/t_.png", delay[i] == 10000 ? 6 : 10, delay[i], OsbLoopType.LoopForever, OsbOrigin.TopLeft, position + new Vector2(LetterSpacing * i, 0));


                    sprite.Scale(sTime, FontScale);
                    
                    sprite.Fade(208679, 208679 + FadeInDelay + 200, 0f, Fade);
                    sprite.Fade(539640 - 200, 539640, Fade, 0);

                    sprite.Fade(548900, 548900 + FadeInDelay + 200, 0f, Fade);
                    sprite.Fade(eTime - 200, eTime, Fade, 0);

                    if (Additive)
                        sprite.Additive(sTime, eTime);

                    var RealColor1 = RandomColor ? new Color4((float)Random(MinColor.R, MaxColor.R),
                                                                (float)Random(MinColor.G, MaxColor.G),
                                                                (float)Random(MinColor.B, MaxColor.B), 255) : MinColor;
                    var RealColor2 = RandomColor ? new Color4((float)Random(MinColor.R, MaxColor.R),
                                                                (float)Random(MinColor.G, MaxColor.G),
                                                                (float)Random(MinColor.B, MaxColor.B), 255) : MaxColor;

                    sprite.Color(sTime, eTime, RealColor1, RealColor2);
                }
            }

            else
            {
                var delay = new int[] { 600000, 60000, 0, 10000, 1000, 0, 100, 10, 1 };
                for (int i = 0; i < delay.Count(); i++)
                {

                    OsbSprite sprite;
                    if (delay[i] <= 0)
                        sprite = layer.CreateSprite(OutputPath + "/t.png", OsbOrigin.TopLeft, position + new Vector2(LetterSpacing * i + ColonOffset.X * FontScale, ColonOffset.Y * FontScale));
                    else
                        sprite = layer.CreateAnimation(OutputPath + "/t_.png", delay[i] == 10000 ? 6 : 10, delay[i], OsbLoopType.LoopForever, OsbOrigin.TopLeft, position + new Vector2(LetterSpacing * i, 0));


                    sprite.Scale(sTime, FontScale);
                    sprite.Fade(208679, 208679 + FadeInDelay + 200, 0f, Fade);
                    sprite.Fade(539640 - 200, 539640, Fade, 0);

                    sprite.Fade(548900, 548900 + FadeInDelay + 200, 0f, Fade);
                    sprite.Fade(eTime - 200, eTime, Fade, 0);

                    if (Additive)
                        sprite.Additive(sTime, eTime);

                    var RealColor1 = RandomColor ? new Color4((float)Random(MinColor.R, MaxColor.R),
                                                                (float)Random(MinColor.G, MaxColor.G),
                                                                (float)Random(MinColor.B, MaxColor.B), 255) : MinColor;
                    var RealColor2 = RandomColor ? new Color4((float)Random(MinColor.R, MaxColor.R),
                                                                (float)Random(MinColor.G, MaxColor.G),
                                                                (float)Random(MinColor.B, MaxColor.B), 255) : MaxColor;

                    sprite.Color(sTime, eTime, RealColor1, RealColor2);
                }
            }
        }

        private void GenerateAnimation()
        {
            var font = GetFont();
            for (var i = 0; i < 10; i++)
            {
                var texture = font.GetTexture(i.ToString());
                Log($"{texture.Path} {i}");
                var finalPath = MapsetPath + "/" + OutputPath + $"/t_{i}.png";
                if (File.Exists(finalPath))
                    File.Delete(finalPath);
                if (File.Exists(MapsetPath + "/" + texture.Path))
                    File.Move(MapsetPath + "/" + texture.Path, finalPath);
            }


            var textureSymbole = font.GetTexture(":");
            var finalPathSymbole = MapsetPath + "/" + OutputPath + $"/t.png";
            if (File.Exists(finalPathSymbole))
                File.Delete(finalPathSymbole);
            if (File.Exists(MapsetPath + "/" + textureSymbole.Path))
                File.Move(MapsetPath + "/" + textureSymbole.Path, finalPathSymbole);
        }
    }
}