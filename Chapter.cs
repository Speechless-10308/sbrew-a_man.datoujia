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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace StorybrewScripts
{
    public class Chapter : StoryboardObjectGenerator
    {
        [Configurable]
        public string OutputPath = "time";
        [Configurable]
        public Color4 MinColor = Color4.White;
        [Configurable]
        public Color4 MaxColor = Color4.Red;
        [Configurable]
        public Color4 ScriptColor = Color4.Red;
        public override void Generate()
        {
            // GenerateSquareOutline(new Vector2(320, 240), 208679, 539640, 500, Color4.White, new Vector2(200, 200), 2, 1, true);
            var font1 = GetFont("Copperplate Gothic Light", 70);
            var font2 = GetFont("Futura PT Book", 40);
            var scriptFont = GetFont("Freestyle Script", 70, 1);

            var albumPath = "sb/album.jpg";
            var albumHeight = GetMapsetBitmap(albumPath).Height;
            var albumPixel = 170;
            var albumPosition = new Vector2(20, 270);
            /// chapter 1
            var bg = GetLayer("Chapter-Back").CreateSprite("sb/red_back.jpg", OsbOrigin.Centre);
            bg.Scale(208679, 1.1f * 480.0f / GetMapsetBitmap("sb/red_back.jpg").Height);
            bg.Fade(OsbEasing.OutSine, 208679, 208679 + 500, 0, 0.9);
            bg.Fade(539640 - 500, 539640, 0.9, 0);
            bg.Additive(208679);
            var girl = GetLayer("Chapter-Girl").CreateSprite("sb/girl.png", OsbOrigin.Centre, new Vector2(600, 240));
            girl.Scale(208679, 1.05f * 480.0f / GetMapsetBitmap("sb/girl.png").Height);
            girl.Fade(208679, 208679 + 500, 0, 0.9);
            girl.Fade(539640 - 500, 539640, 0.9, 0);
            // girl.Additive(208679);


            WiggleScreen(208679, 539640, 60, new Vector2(320, 240), 0, 20, bg);
            WiggleScreen(208679, 539640, 60, new Vector2(600, 240), 0, 20, girl);

            ShowAlbum(208679, 539640, 500, albumPath, albumHeight, albumPixel, albumPosition);

            ProgressBar(208679, 539640, new Vector2(320 - 90, 400), new Vector2(600, 20), MinColor, MaxColor, 500);

            GenerateSongInfo(208679, 539640, 500, new Vector2(albumPosition.X + albumPixel / 2 + 30, albumPosition.Y - albumPixel/2 + 10f), 50, 60f, new FontGenerator[] { font2, font1, scriptFont }, new float[] { 0.3f, 0.3f, 0.5f }, "Song Title", "A Man Who \n Existed in Parallel");

            GenerateSongInfo(208679, 539640, 500, new Vector2(albumPosition.X + albumPixel / 2 + 30, albumPosition.Y + albumPixel/2 - 40), 30, 30f, new FontGenerator[] { font2, font1, scriptFont }, new float[] { 0.3f, 0.3f, 0.5f }, "Chapter 2", "The First Velocity");

            /// chapter 2
            bg.Fade(OsbEasing.OutSine, 548885, 548885 + 500, 0, 0.9);
            bg.Fade(1171447 - 500, 1171447, 0.9, 0);
            girl.Fade(548885, 548885 + 500, 0, 0.9);
            girl.Fade(1171447 - 500, 1171447, 0.9, 0);
            WiggleScreen(548885, 1171447, 120, new Vector2(320, 240), 0, 20, bg);
            WiggleScreen(548885, 1171447, 120, new Vector2(600, 240), 0, 20, girl);

            ShowAlbum(548885, 1171447, 500, albumPath, albumHeight, albumPixel, albumPosition);

            ProgressBar(548885, 945846, new Vector2(320 - 90, 400), new Vector2(600, 20), MinColor, MaxColor, 500);

            GenerateSongInfo(548885, 945846, 500, new Vector2(albumPosition.X + albumPixel / 2 + 30, albumPosition.Y - albumPixel/2 + 10f), 50, 60f, new FontGenerator[] { font2, font1, scriptFont }, new float[] { 0.3f, 0.3f, 0.5f }, "Song Title", "A Man Who \n Existed in Parallel");

            GenerateSongInfo(548885, 945846, 500, new Vector2(albumPosition.X + albumPixel / 2 + 30, albumPosition.Y + albumPixel/2 - 40), 30, 30f, new FontGenerator[] { font2, font1, scriptFont }, new float[] { 0.3f, 0.3f, 0.5f }, "Chapter 3", "Apparition of Solid");

            /// chapter 3
            ProgressBar(945846, 1171447, new Vector2(320 - 90, 400), new Vector2(600, 20), MinColor, MaxColor, 500);

            GenerateSongInfo(945846, 1171447, 500, new Vector2(albumPosition.X + albumPixel / 2 + 30, albumPosition.Y - albumPixel/2 + 10f), 50, 60f, new FontGenerator[] { font2, font1, scriptFont }, new float[] { 0.3f, 0.3f, 0.5f }, "Song Title", "A Man Who \n Existed in Parallel");

            GenerateSongInfo(945846, 1171447, 500, new Vector2(albumPosition.X + albumPixel / 2 + 30, albumPosition.Y + albumPixel/2 - 40), 30, 30f, new FontGenerator[] { font2, font1, scriptFont }, new float[] { 0.3f, 0.3f, 0.5f }, "Chapter 4", "Dream to the East");



        }
        public void GenerateSongInfo(int startTime, int endTime, int duration, Vector2 titelPosition, float nameOffsetY, float scriptOffsetY, FontGenerator[] fonts, float[] scales, string title, string name)
        {
            var pxPath = "sb/p.png";

            var textures = new List<FontTexture>{
                fonts[0].GetTexture(title),
                fonts[1].GetTexture(name),
            };

            var offsets = new List<Vector2>{
                new Vector2(0, 0),
                new Vector2(0, nameOffsetY),
            };

            int i = 0;
            
            foreach(var texture in textures)
            {
                var sprite = GetLayer("Chapter-Text").CreateSprite(texture.Path, OsbOrigin.CentreLeft, titelPosition + offsets[i]);
                sprite.Scale(startTime, scales[i]);
                sprite.Fade(startTime, 1);
                FlashFade(sprite, endTime - duration, (float)duration / 4, 4, 1, 0);

                var mask = GetLayer("Chapter-Text").CreateSprite(pxPath, OsbOrigin.CentreLeft, titelPosition + offsets[i]);
                mask.ScaleVec(startTime, texture.BaseWidth * scales[i], texture.BaseHeight * scales[i]);
                mask.Fade(startTime, 1);
                
                mask.MoveX(OsbEasing.InExpo, startTime + duration, startTime + duration*2, titelPosition.X, titelPosition.X + texture.BaseWidth * scales[i]);

                mask.Color(startTime, Color4.Black);   

                var cursor1 = GetLayer("Chapter-Text").CreateSprite(pxPath, OsbOrigin.Centre, titelPosition +  offsets[i]);
                cursor1.ScaleVec(startTime, 2, texture.BaseHeight * scales[i]);
                FlashIn(cursor1, startTime, (float)duration / 4, 4, 1, 1);
                FlashFade(cursor1, startTime + duration*2, (float)duration / 4, 4, 1, 1);
                cursor1.MoveX(OsbEasing.InExpo, startTime + duration, startTime + duration*2, titelPosition.X, titelPosition.X + texture.BaseWidth * scales[i]);

                i++;
            }

            var script = GetLayer("Chapter-Text2").CreateSprite(fonts[2].GetTexture(name).Path, OsbOrigin.CentreLeft, titelPosition + new Vector2(textures[1].BaseWidth*scales[1] / 10, scriptOffsetY));
            script.Scale(startTime, scales[2]);
            FlashIn(script, startTime, (float)duration / 4, 4, 0.5f, 1);
            FlashFade(script, endTime - duration, (float)duration / 4, 4, 0.5f, 0);
            script.Color(startTime, ScriptColor);
            script.Rotate(startTime, -Math.PI/48);


        }

        public void ProgressBar(int startTime, int endTime, Vector2 position, Vector2 size, Color4 minColor, Color4 maxColor, int duration)
        {
            var pxPath = "sb/p.png";
            var flashCount = 4;

            var bar = GetLayer("Chapter-Progress").CreateSprite(pxPath, OsbOrigin.CentreLeft, position - new Vector2(size.X / 2, 0));
            bar.ScaleVec(startTime, endTime - duration*2, 0, size.Y, size.X, size.Y);
            bar.ScaleVec(OsbEasing.InOutSine, endTime - duration * 2, endTime - duration, size.X, size.Y, 0, size.Y);
            bar.Color(startTime, endTime, minColor, maxColor);
            bar.Fade(startTime, 0);
            FlashIn(bar, startTime + duration * 2, (float)duration / flashCount, flashCount, 1, 1);
            FlashFade(bar, endTime - duration * 2, (float)duration / flashCount, flashCount, 1, 1);

            GenerateSquareOutline(position - new Vector2(size.X / 2, 0), startTime, endTime, duration, Color4.White, new Vector2(size.X, size.Y), 2, 0, true);

        }
        public void WiggleScreen(double startTime, double endTime, int rate, Vector2 InitPos, double InitRot, int wiggleAmount, params OsbSprite[] sprites)
        {

            //Rate average around 20 to 100 (bigger number, more shakes)

            var loopTime = (endTime - startTime) / rate;

            var previousCord = new Vector2(InitPos.X, InitPos.Y);

            for (int i = 0; i < rate - 1; i++)
            {

                var xCord = Random(InitPos.X - wiggleAmount, InitPos.X + wiggleAmount);

                var yCord = Random(InitPos.Y - wiggleAmount, InitPos.Y + wiggleAmount);

                var tempCord = new Vector2(xCord, yCord);

                foreach (var sprite in sprites)
                {
                    sprite.Move(OsbEasing.InOutSine, startTime + (loopTime * i), startTime + (loopTime * (i + 1)), previousCord, tempCord);
                }

                //Log($"{startTime+(loopTime*i)} until {startTime+(loopTime*(i+1))}");

                previousCord = tempCord;

            }

            double previousRotation = InitRot;

            for (int i = 0; i < (rate - 1) / 2; i++)
            {

                double[] rotate = new double[]{
                    0.01 // , 0.02, 0.03 // Add these for more rotations
                };

                var rotInd = Random(0, rotate.Length);

                var tempRot = rotate[rotInd];

                foreach (var sprite in sprites)
                {
                    sprite.Rotate(OsbEasing.InOutSine, startTime + ((2 * loopTime) * i), startTime + ((2 * loopTime) * (i + 1)), previousRotation, tempRot);
                }

                previousRotation = tempRot;

            }

            foreach (var sprite in sprites)
            {

                sprite.Rotate(OsbEasing.InOutSine, startTime + loopTime * (rate - 1), startTime + loopTime * rate, previousRotation, 0);
                sprite.Move(OsbEasing.InOutSine, startTime + loopTime * (rate - 1), startTime + loopTime * rate, previousCord, InitPos);

            }

        }
        public void ShowAlbum(int startTime, int endTime, int duration, string albumPath, int albumHeight, int albumPixel, Vector2 position)
        {
            var album = GetLayer("Chapter").CreateSprite(albumPath, OsbOrigin.Centre, position);
            album.Scale(startTime + duration * 2, (float)albumPixel / albumHeight);
            FlashIn(album, startTime + duration * 2, (float)duration / 4, 4, 1, 1);
            FlashFade(album, endTime - duration * 2, (float)duration / 4, 4, 1, 0);

            GenerateSquareOutline(position, startTime, endTime, duration, Color4.White, new Vector2(albumPixel, albumPixel) * 1.05f, 2, 1, true);
        }



        public void GenerateSquareOutline(Vector2 position, int startTime, int endTime, int duration, Color4 color, Vector2 size, float width, int type, bool isAnimated)
        {
            /// <summary>
            /// Generate a square outline
            /// </summary>
            /// <param name="position">The position of the square</param>
            /// <param name="type"> 0: left, 1: center, 2: right</param>
            /// </param>
            var pxPath = "sb/p.png";
            var flashCount = 4;

            // top
            var top = GetLayer("Chapter").CreateSprite(pxPath, (OsbOrigin)(type + 3), position - new Vector2(0, size.Y / 2));
            if (isAnimated)
            {
                top.ScaleVec(OsbEasing.InOutSine, startTime + duration, startTime + 2 * duration, 0, width, size.X + width, width);
                top.ScaleVec(OsbEasing.InOutSine, endTime - duration * 2, endTime - duration, size.X + width, width, 0, width);
                FlashIn(top, startTime, (float)duration / flashCount, flashCount, 1, 1);
                FlashFade(top, endTime - duration, (float)duration / flashCount, flashCount, 1, 0.3f);
            }
            else
            {
                top.ScaleVec(startTime, size.X, width);
                top.Fade(startTime, 1);
                top.Fade(endTime, 0);
            }
            top.Color(startTime, color);

            // bottom
            var bottom = GetLayer("Chapter").CreateSprite(pxPath, (OsbOrigin)(type + 3), position + new Vector2(0, size.Y / 2));
            if (isAnimated)
            {
                bottom.ScaleVec(OsbEasing.InOutSine, startTime + duration, startTime + 2 * duration, 0, width, size.X + width, width);
                bottom.ScaleVec(OsbEasing.InOutSine, endTime - duration * 2, endTime - duration, size.X + width, width, 0, width);
                FlashIn(bottom, startTime, (float)duration / flashCount, flashCount, 1, 1);
                FlashFade(bottom, endTime - duration, (float)duration / flashCount, flashCount, 1, 0.3f);
            }
            else
            {
                bottom.ScaleVec(startTime, size.X, width);
                bottom.Fade(startTime, 1);
                bottom.Fade(endTime, 0);
            }
            bottom.Color(startTime, color);

            // left

            var left = GetLayer("Chapter").CreateSprite(pxPath, (OsbOrigin)(type + 3), position);
            left.ScaleVec(startTime, width, size.Y + width);
            if (isAnimated)
            {
                FlashIn(left, startTime, (float)duration / flashCount, flashCount, 1, 1);
                FlashFade(left, endTime - duration, (float)duration / flashCount, flashCount, 1, 1);
                if (type == 1)
                {
                    left.MoveX(OsbEasing.InOutSine, startTime + duration, startTime + 2 * duration, position.X, position.X - size.X / 2);
                    left.MoveX(OsbEasing.InOutSine, endTime - duration * 2, endTime - duration, position.X - size.X / 2, position.X);
                }
                else if (type == 2)
                {
                    left.MoveX(OsbEasing.InOutSine, startTime + duration, startTime + 2 * duration, position.X, position.X - size.X);
                    left.MoveX(OsbEasing.InOutSine, endTime - duration * 2, endTime - duration, position.X - size.X, position.X);
                }

            }
            else
            {
                left.Fade(startTime, 1);
                left.Fade(endTime, 0);
            }
            left.Color(startTime, color);

            // right
            var right = GetLayer("Chapter").CreateSprite(pxPath, (OsbOrigin)(type + 3), position);
            right.ScaleVec(startTime, width, size.Y + width);
            if (isAnimated)
            {
                FlashIn(right, startTime, (float)duration / flashCount, flashCount, 1, 1);
                FlashFade(right, endTime - duration, (float)duration / flashCount, flashCount, 1, 1);
                if (type == 1)
                {
                    right.MoveX(OsbEasing.InOutSine, startTime + duration, startTime + 2 * duration, position.X, position.X + size.X / 2);
                    right.MoveX(OsbEasing.InOutSine, endTime - duration * 2, endTime - duration, position.X + size.X / 2, position.X);
                }
                else if (type == 0)
                {
                    right.MoveX(OsbEasing.InOutSine, startTime + duration, startTime + 2 * duration, position.X, position.X + size.X);
                    right.MoveX(OsbEasing.InOutSine, endTime - duration * 2, endTime - duration, position.X + size.X, position.X);
                }
            }
            else
            {
                right.Fade(startTime, 1);
                right.Fade(endTime, 0);
            }


        }
        private void FlashIn(OsbSprite sprite, int startTime, float intervalTime, int flahsCount, float startOpacity, float endOpacity)
        {
            for (int i = 0; i < flahsCount - 1; i++)
            {
                sprite.Fade(startTime + i * intervalTime, startTime + (i + 1) * intervalTime, startOpacity * 1, startOpacity * 0.3);
            }
            sprite.Fade(startTime + (flahsCount - 1) * intervalTime, startTime + flahsCount * intervalTime + 50, startOpacity * 0.3, startOpacity * 1);
        }
        private void FlashFade(OsbSprite sprite, int endTime, float intervalTime, int flahsCount, float startOpacity, float endOpacity)
        {
            for (int i = 0; i < flahsCount - 1; i++)
            {
                sprite.Fade(endTime + i * intervalTime, endTime + (i + 1) * intervalTime, startOpacity * 1, startOpacity * 0.3);
            }
            sprite.Fade(endTime + (flahsCount - 1) * intervalTime, endTime + flahsCount * intervalTime + 50, startOpacity * 1, startOpacity * 0);
        }
        private FontGenerator GetFont(string fontName, int size, int ShadowThickness = 0)
        {
            return LoadFont($"sb/f/{fontName}", new FontDescription()
            {
                FontPath = fontName,
                FontSize = size,
                Color = Color4.White,
                Padding = Vector2.Zero,
                FontStyle = FontStyle.Regular,
                TrimTransparency = true,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = Color4.Black,
            });
        }


    }
}
