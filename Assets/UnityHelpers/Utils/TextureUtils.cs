using UnityEngine;

namespace UnityHelpers.Utils
{
    public static class TextureUtils
    {
        /// Scale texture to target size. Need call Apply() on output texture
        public static void Scale(Texture2D sourceTexture, Vector2 targetSize, Texture2D outputTexture = null)
        {
            BeginDrawing(targetSize.x, targetSize.y, Color.clear);
            Draw(new Rect(Vector2.zero, targetSize), sourceTexture, new Rect(0, 0, 1, 1));
            SaveDrawing(outputTexture != null ? outputTexture : sourceTexture);
            EndDrawing();
        }

        /// Scale texture to fill target size. Need call Apply() on output texture
        public static void ScaleToFill(Texture2D sourceTexture, Vector2 targetSize, Texture2D outputTexture = null)
        {
            Vector2 normalizedSourceSize = new Vector2(sourceTexture.width, sourceTexture.height) / Mathf.Max(sourceTexture.width, sourceTexture.height);
            Vector2 normalizedTargetSize = (targetSize / Mathf.Max(targetSize.x, targetSize.y));
            Rect sourceRect;
            if (normalizedSourceSize.x / normalizedTargetSize.x < normalizedSourceSize.y / normalizedTargetSize.y)
            {
                var convertedTagetSize = new Vector2(1, normalizedSourceSize.x * normalizedTargetSize.y / normalizedTargetSize.x);
                sourceRect = new Rect(0, (normalizedSourceSize.y - convertedTagetSize.y) / 2, convertedTagetSize.x, convertedTagetSize.y);
            }
            else
            {
                var convertedTagetSize = new Vector2(normalizedSourceSize.y * normalizedTargetSize.x / normalizedTargetSize.y, 1);
                sourceRect = new Rect((normalizedSourceSize.x - convertedTagetSize.x) / 2, 0, convertedTagetSize.x, convertedTagetSize.y);
            }
            BeginDrawing(targetSize.x, targetSize.y, Color.clear);
            Draw(new Rect(Vector2.zero, targetSize), sourceTexture, sourceRect);
            SaveDrawing(outputTexture != null ? outputTexture : sourceTexture);
            EndDrawing();
        }

        // Core Drawing

        public static void BeginDrawing(float drawWidth, float drawHeight, Color backgroundColor)
        {
            BeginDrawing(new RenderTexture((int)drawWidth, (int)drawHeight, 32), backgroundColor);
        }

        public static void BeginDrawing(RenderTexture renderTexture, Color backgroundColor)
        {
            EndDrawing();
            RenderTexture.active = renderTexture;   // Set renderTexture active so DrawTexture will draw to it.
            GL.Clear(true, true, backgroundColor);
        }

        public static void Draw(Rect drawIntoRect, Texture2D inputTexture, Rect inputNormalizedRect, int leftBorder = 0, int rightBorder = 0, int topBorder = 0, int bottomBorder = 0, Material mat = null, int pass = -1)
        {
            if (RenderTexture.active != null)
            {
                GL.PushMatrix(); // Saves both projection and modelview matrices to the matrix stack.
                GL.LoadPixelMatrix(0, RenderTexture.active.width, RenderTexture.active.height, 0);
                Graphics.DrawTexture(drawIntoRect, inputTexture, inputNormalizedRect, leftBorder, rightBorder, topBorder, bottomBorder, mat, pass);
                GL.PopMatrix(); //Restores both projection and modelview matrices off the top of the matrix stack.
            }
        }

        public static void SaveDrawing(Texture2D outputTexture)
        {
            SaveDrawing(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0, outputTexture);
        }

        public static void SaveDrawing(Rect source, float destX, float destY, Texture2D outputTexture)
        {
            if (RenderTexture.active != null)
            {
                outputTexture.Resize((int)source.width, (int)source.height);
                outputTexture.ReadPixels(source, (int)destX, (int)destY, true);
            }
        }

        public static void EndDrawing()
        {
            if (RenderTexture.active != null)
            {
                var rt = RenderTexture.active;
                RenderTexture.active = null;
                Object.Destroy(rt);
            }
        }
    }
}
