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
    public class Credits : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 color = Color4.White;
        
        public override void Generate()
        {
            string fishPath = "sb/fish.png";
            string girlPath = "sb/girl_half_gray.png";
            string butterflyPath = "sb/butterfly.png";
            string grayBackPath = "sb/gray_back_blur.jpg";
            string leafPath = "sb/leaf.png";
            string leaf2Path = "sb/leaf2.png";
            string textPath = "sb/f/S";

            FontGenerator font = LoadFont(textPath, new FontDescription()
            {
                FontPath = "Lemosty",
                FontSize = 50,
                Color = Color4.White,
                FontStyle = FontStyle.Regular,
                TrimTransparency = true,
                EffectsOnly = false,
            });


            var initialPosition = new Vector2(320, 240);
            var layerSpeed = 5f;

            var grayBack = GetLayer("Credits").CreateSprite(grayBackPath, OsbOrigin.Centre, initialPosition);
            grayBack.Scale(157792, 1.33f * 480.0f / GetMapsetBitmap(grayBackPath).Height);
            grayBack.Fade(157792, 157792 + 1000, 0, 0.3f);
            grayBack.Fade(203883 - 1000, 203883, 0.3f, 0);
            spriteMove(grayBack, 157792, 203883, 1000, grayBack.PositionAt(157792), layerSpeed);

            layerSpeed = 10f;
            initialPosition = new Vector2(747, 240);
            var girl = GetLayer("girl").CreateSprite(girlPath, OsbOrigin.Centre, initialPosition);
            girl.Scale(157792, 480.0f / GetMapsetBitmap(girlPath).Height);
            girl.Fade(157792, 157792 + 1000, 0, 0.5f);
            girl.Fade(203883 - 1000, 203883, 0.5f, 0);
            spriteMove(girl, 157792, 203883, 1000, girl.PositionAt(157792), layerSpeed);

            layerSpeed = 15f;
            initialPosition = new Vector2(490, 240);
            ShowCredit(fishPath, font, initialPosition, layerSpeed, "Song by", "Taishi");

            initialPosition = new Vector2(490, 100);
            ShowCredit(leafPath, font, initialPosition, layerSpeed, "Song Name", "A Man Who Existed in Parallel", 167587);

            initialPosition = new Vector2(440, 240);
            ShowCredit(leaf2Path, font, initialPosition, layerSpeed, "Map by", "datoujia", 177383);

            initialPosition = new Vector2(190, 370);
            ShowCredit(butterflyPath, font, initialPosition, layerSpeed, "Storyboard by", "RiceSS", 187179);


            var circlePath = "sb/c.png";
            var circleHeight = GetMapsetBitmap(circlePath).Height;
            var circleCount = 40;
            layerSpeed = GenerateParticles(layerSpeed, circlePath, circleHeight, circleCount, 5f, 10f, 1000f);

            var squarePath = "sb/square_wall1.png";
            var squareHeight = GetMapsetBitmap(squarePath).Height;
            var squareCount = 20;
            layerSpeed = GenerateParticles(layerSpeed, squarePath, squareHeight, squareCount, 20f, 25f, 1600f, 100f, 200f, 0.2f);

        }

        private float GenerateParticles(float layerSpeed, string circlePath, int circleHeight, int circleCount, float lowSpeed, float highSpeed, float maxXPos, float lowPixel=10f, float highPixel=70f, float opacity = 0.1f)
        {
            for (int i = 0; i < circleCount; i++)
            {
                layerSpeed = Random(lowSpeed, highSpeed);
                var spawnPosition = new Vector2(Random(-107, maxXPos), Random(0, 480.0f));
                var randomOffset = Random(0, 3500);
                var circle = GetLayer("Circle").CreateSprite(circlePath, OsbOrigin.Centre, spawnPosition);
                spriteMove(circle, 157792 + randomOffset, 203883, 1000, circle.PositionAt(157792), layerSpeed, false);
                circle.Scale(157792 + randomOffset, Random(lowPixel, highPixel) / circleHeight);
                circle.Fade(OsbEasing.OutCirc, 157792 + randomOffset, 157792 + 1000 + randomOffset, 0, opacity);
                circle.Fade(OsbEasing.OutCirc, 203883 - 1000, 203883, opacity, 0);
                circle.Color(157792, color);
            }

            return layerSpeed;
        }

        private void ShowCredit(string decoPath, FontGenerator font, Vector2 initialPosition, float layerSpeed, string title, string name, int startTime = 157792, int endTime = 203883)
        {
            RendText(title, initialPosition - new Vector2(0, 40), font, startTime, endTime, 1000, layerSpeed, 0.3f);
            var posReal = RendText(name, initialPosition, font, startTime, endTime, 1000, layerSpeed, 0.5f);

            var fish = GetLayer("Credits").CreateSprite(decoPath, OsbOrigin.Centre, initialPosition + new Vector2(posReal.Item2 / 2 + 10, posReal.Item1 + 20));
            var fishHeight = GetMapsetBitmap(decoPath).Height;
            fish.Scale(startTime, 50.0f / fishHeight);
            fish.Fade(startTime, startTime + 1000, 0, 1);
            fish.Fade(endTime - 1000, endTime, 1, 0);
            spriteMove(fish, startTime, endTime, 1000, fish.PositionAt(157792), layerSpeed);
        }

        public void spriteMove(OsbSprite sprite, int startTime, int endTime, int aniTime, Vector2 startPosition, float speed, bool isMoveY = true)
        {
            sprite.MoveX(startTime, endTime, startPosition.X, startPosition.X - speed * (endTime - startTime) / 1000);
            if (isMoveY)    
            {
                sprite.MoveY(OsbEasing.OutCirc,startTime, startTime + aniTime, startPosition.Y - 20, startPosition.Y);
            }
        }
        public Tuple<float, float> RendText(string text, Vector2 position, FontGenerator font, int startTime, int endTime, int aniTime, float speed, float fontScale)
        {
            var textLayer = GetLayer("Credits");

            var lineWidth = 0f;
            foreach (var c in text)
            {
                var texture = font.GetTexture(c.ToString());
                lineWidth += texture.BaseWidth * fontScale;
            }

            var startX = position.X - lineWidth / 2;
            var moveDistance = speed * (endTime - startTime) / 1000;
            int i = 0;
            var yReturn = 0f;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var startPosition = new Vector2(startX, position.Y) + texture.OffsetFor(OsbOrigin.Centre) * fontScale;
                    yReturn = startPosition.Y - position.Y;
                    var endPosition = new Vector2(startX - moveDistance, position.Y) + texture.OffsetFor(OsbOrigin.Centre) * fontScale;
                    var sprite = textLayer.CreateSprite(texture.Path, OsbOrigin.Centre, startPosition);
                    sprite.Scale(startTime, fontScale);
                    sprite.Fade(OsbEasing.OutCirc, startTime + i * (aniTime / text.Length), startTime + aniTime + i * (aniTime / text.Length), 0, 1);
                    sprite.MoveX(startTime, endTime, startPosition.X, endPosition.X);
                    sprite.MoveY(OsbEasing.OutCirc, startTime + i * (aniTime / text.Length), startTime + aniTime + i * (aniTime / text.Length), startPosition.Y - 20, startPosition.Y);
                    sprite.Fade(OsbEasing.OutCirc, endTime - aniTime - (text.Length - i) * (aniTime / text.Length), endTime - (text.Length - i) * (aniTime / text.Length), 1, 0);
                }
                startX += texture.BaseWidth * fontScale;
                i++;
            }
            return new Tuple<float, float>(yReturn, lineWidth);
        }
    }
}
