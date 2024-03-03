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
    public class Intro : StoryboardObjectGenerator
    {
        #region Sprite Path
        public string LogoPath = "sb/logo.jpg";
        public string circlePath = "sb/c.png";
        public string squareWallPath = "sb/square_wall1.png";
        public string redBackPath = "sb/red_back.jpg";
        #endregion
        public override void Generate()
        {
            #region Sprite Bitmap
            var logoBitmap = GetMapsetBitmap(LogoPath);
            var circleBitmap = GetMapsetBitmap(circlePath);
            var squareWallBitmap = GetMapsetBitmap(squareWallPath);
            var redBackBitmap = GetMapsetBitmap(redBackPath);
            #endregion

            #region Timing Settings
            // these two timing are important and reused in the following scripts
            var logoStartTime = 79424;
            var albumAniStartTime = 118608;
            var clockTime = 157792;
            var totalEndTime = 208169;
            #endregion

            var layer = GetLayer("Intro");
            var logo = layer.CreateSprite(LogoPath, OsbOrigin.Centre);
            logo.Scale(OsbEasing.OutCubic , logoStartTime, albumAniStartTime, 100.0f / logoBitmap.Height, 170.0f / logoBitmap.Height);
            logo.Fade(logoStartTime, (logoStartTime + albumAniStartTime) / 2, 0, 1);
            logo.Fade((logoStartTime + albumAniStartTime) / 2, albumAniStartTime, 1, 0);

            FontGenerator font = LoadFont("sb/IntroText", new FontDescription()
            {
                FontPath = "Lemosty",
                FontSize = 70,
                Color = Color4.White,
                Padding = new Vector2(0, 0),
                FontStyle = FontStyle.Regular,
                TrimTransparency = true,
                EffectsOnly = false,
            });
            var textScale = 0.9f;
            

            var text = "Phant";
            var directionList = new List<string> {"left", "left", "down", "left", "right"};
            var yPositionList = new List<int> { 240, 220, 220, 250, 240 };
            var xMarginList = new List<int> { 0, 1, 1, 2, 0};
            var colorList = new List<Color4> { Color4.White, Color4.Blue, Color4.White, Color4.White, Color4.Blue };
            float letterX = 330;
            var width = 0f;
            var i = 0;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                if (xMarginList[i] == 0)
                {
                    width += texture.BaseWidth * textScale;
                }
                else if (xMarginList[i] == 1)
                {
                    width += texture.BaseWidth * textScale * 0.9f;
                }
                else if (xMarginList[i] == 2)
                {
                    width += texture.BaseWidth * textScale * 0.5f;
                }
                i++;
            }
            letterX = 350 - width / 2;
            i = 0;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                var position = new Vector2(letterX, yPositionList[i]);
                var sprite = layer.CreateSprite(texture.Path, OsbOrigin.Centre, position);
                sprite.Scale(albumAniStartTime, textScale);
                sprite.Fade(albumAniStartTime, albumAniStartTime + 1000, 0, 1);
                sprite.Fade(clockTime - 2000, clockTime, 1, 0);
                sprite.Color(albumAniStartTime, colorList[i]);

                var textBitmap = GetMapsetBitmap(texture.Path);
                var mask = layer.CreateSprite("sb/p.png", OsbOrigin.Centre, position);
                mask.Fade(albumAniStartTime, 1);
                mask.Fade(albumAniStartTime+1000, 0);
                mask.ScaleVec(albumAniStartTime, textBitmap.Width * textScale, textBitmap.Height * textScale);
                // mask.Fade(albumAniStartTime, albumAniStartTime + 1000, 1, 0);
                mask.Color(albumAniStartTime, Color.Black);
                if (directionList[i] == "left")
                {
                    sprite.MoveX(OsbEasing.OutCubic, albumAniStartTime, albumAniStartTime + 1000, position.X - 10 * (5-i), position.X);
                    mask.MoveX(OsbEasing.OutCubic, albumAniStartTime, albumAniStartTime + 1000, position.X - 10 * (5-i) + texture.BaseWidth * textScale * 0.2f, position.X + texture.BaseWidth * textScale);
                }
                else if (directionList[i] == "right")
                {
                    sprite.MoveX(OsbEasing.OutCubic, albumAniStartTime, albumAniStartTime + 1000, position.X + 10 * (5-i), position.X);
                    mask.MoveX(OsbEasing.OutCubic, albumAniStartTime, albumAniStartTime + 1000,position.X + 10 * (5-i), position.X + 30 * (5-i));
                    
                }
                else if (directionList[i] == "up")
                {
                    sprite.MoveY(OsbEasing.OutCubic, albumAniStartTime, albumAniStartTime + 1000, position.Y + 10 * (5-i), position.Y);
                    mask.MoveY(OsbEasing.OutCubic, albumAniStartTime, albumAniStartTime + 1000, position.Y + 10 * (5-i) - texture.BaseHeight * textScale * 0.2f, position.Y - texture.BaseHeight * textScale);
                }
                else if (directionList[i] == "down")
                {
                    sprite.MoveY(OsbEasing.OutCubic, albumAniStartTime, albumAniStartTime + 1000, position.Y - 10 * (5-i), position.Y);
                    mask.MoveY(OsbEasing.OutCubic, albumAniStartTime, albumAniStartTime + 1000, position.Y - 10 * (5-i), position.Y + textBitmap.Height * textScale);
                }

                
                letterX += texture.BaseWidth * textScale * (xMarginList[i] == 0 ? 1 : (xMarginList[i] == 1 ? 0.9f : 0.5f));
                i++;
            }
            var numTexture = font.GetTexture("2");
            var numPosition = new Vector2(330, 230);
            var numSprite = layer.CreateSprite(numTexture.Path, OsbOrigin.Centre, numPosition);
            numSprite.Scale(albumAniStartTime, textScale * 2);
            numSprite.Fade(albumAniStartTime, albumAniStartTime + 1000, 0, 0.2);
            numSprite.Fade(clockTime - 2000, clockTime, 0.2, 0);

            var anotherFont = SetFont();
            rendText("ALBUM", new Vector2(335, 320), albumAniStartTime, clockTime, 1000, anotherFont, 0.2f, true);

            var redBack = layer.CreateSprite(redBackPath, OsbOrigin.Centre);
            redBack.Scale(logoStartTime, 1.3f * Math.Max(854.0f / redBackBitmap.Width, 480.0f / redBackBitmap.Height));
            redBack.Fade(albumAniStartTime, albumAniStartTime + 1000, 0, 0.9);
            redBack.Fade(clockTime - 2000, clockTime, 0.9, 0);
            redBack.Move(OsbEasing.OutCubic, albumAniStartTime, clockTime, 340, 240 + 100, 200, 240 - 100);
            redBack.Additive(albumAniStartTime, clockTime);

            Letterbox(albumAniStartTime, clockTime);

        }
        public float rendText(string text, Vector2 position, int startTime, int endTime, int aniTime, FontGenerator font,  float fontScale, bool isCenter = false)
        {
            var letterX = position.X;
            var letterY = position.Y;
            var lineWidth = 0f;
            var lineHeight = 0f;
            OsbOrigin Origin = OsbOrigin.Centre;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * fontScale;
                lineHeight = Math.Max(lineHeight, texture.BaseHeight * fontScale);
            }
            letterY -= lineHeight * 0.5f;
            if (isCenter)
            {
                letterX -= lineWidth * 0.5f;
            } 
            
            int i = 0;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var textPosition = new Vector2(letterX, letterY)
                        + texture.OffsetFor(Origin) * fontScale;

                    var sprite = GetLayer("TEXT").CreateSprite(texture.Path, Origin, textPosition);
                    sprite.Scale(startTime, fontScale);
                    sprite.MoveX(OsbEasing.OutQuad, startTime, startTime + aniTime, textPosition.X, textPosition.X  + 20 * (i - (float)text.Length / 2));
                    sprite.Fade(startTime, startTime+aniTime, 0, 1);
                    sprite.Fade(endTime - aniTime, endTime, 1, 0);
                }
                letterX += texture.BaseWidth * fontScale;
                i += 1;
            }
            return lineWidth;
        }
        public void Letterbox(int startTime, int endTime)
        {
            var t = GetLayer("IntroOverlay").CreateSprite("sb/p.png", OsbOrigin.TopCentre);
            var b = GetLayer("IntroOverlay").CreateSprite("sb/p.png", OsbOrigin.BottomCentre);


            t.ScaleVec(OsbEasing.OutSine, startTime, startTime + 1000, 900, 0, 900, 140);
            b.ScaleVec(OsbEasing.OutSine, startTime, startTime + 1000, 900, 0, 900, 140);
            
            t.ScaleVec(OsbEasing.OutSine , endTime - 1000, endTime, 900, 140, 900, 0);
            b.ScaleVec(OsbEasing.OutSine, endTime - 1000, endTime, 900, 140, 900, 0);
            t.Color(startTime, Color4.Black);
            b.Color(startTime, Color4.Black);
            t.Move(startTime, 320, 0);
            b.Move(startTime, 320, 480);
        }

        public FontGenerator SetFont()
        {
            string FontName = "华光准圆_CNKI";
            int FontSize = 50;
            var Padding = new Vector2(5, 5);
            var TrimTransparency = true;

            var font = LoadFont("sb/f/G", new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = Color4.White,
                Padding = Padding,
                TrimTransparency = TrimTransparency,
                FontStyle = FontStyle.Regular,
            });
            return font;
        }
    }
}
