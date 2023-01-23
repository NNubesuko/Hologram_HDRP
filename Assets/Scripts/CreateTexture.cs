using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System.Drawing.Imaging;

/*
 * テクスチャを生成するクラス
 */
public class CreateTexture {

    public static Texture2D Create(
        int width,
        int height,
        Brush backgroundColor,
        int fontSize,
        FontFamily fontFamily,
        Brush fontColor,
        string drawText
    ) {
        using (Bitmap bitmap = new Bitmap(width, height)) {
            
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
            using (System.Drawing.Font font = new System.Drawing.Font(fontFamily, fontSize)) {

                Rectangle backgroundRect = new Rectangle(0, 0, width, height);
                graphics.FillRectangle(backgroundColor, backgroundRect);
                graphics.DrawString(drawText, font, fontColor, backgroundRect);

            }

            // ビットマップでは、オブジェクトに割り当てられないため、テスクチャに変換する
            return BitmapToTexture2D(bitmap);

        }

        throw new Exception("テクスチャを生成できませんでした");
    }

    /*
     * ビットマップからテクスチャに変換するメソッド
     *
     * ビットマップをメモリに保存
     * バッファを生成し、メモリからビットマップの情報を読み込む
     * バッファからテクスチャを生成
     */
    private static Texture2D BitmapToTexture2D(Bitmap bitmap) {
        using (MemoryStream ms = new MemoryStream()) {
            bitmap.Save(ms, ImageFormat.Png);
            byte[] buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(buffer);
            return texture2D;
        }

        throw new Exception("テクスチャに変換できませんでした");
    }

}
