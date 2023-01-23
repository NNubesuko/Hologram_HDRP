using System;
using UnityEngine;

public static class StringExtension {

    /*
     * 秒の文字列を TimeSpan に変換する
     */
    public static TimeSpan SecondsToTimeSpan(this string self) {
        return TimeSpan.FromSeconds(
            double.Parse(self)
        );
    }

    /*
     * ミリ秒の文字列を TimeSpan に変換する
     */
    public static TimeSpan MillisecondsToTimeSpan(this string self) {
        return TimeSpan.FromMilliseconds(
            double.Parse(self)
        );
    }

    /*
     * 絶対パスから Assets/ のパスへ変換
     */
    public static string AbsoluteToAssetsPath(this string self) {
        return self.Replace("\\", "/").Replace(Application.dataPath, "Assets");
    }

    /*
     * Assets/ のパスから絶対パスへ変更
     */
    public static string AssetsToAbsolutePath(this string self) {
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                return self.Replace("Assets", Application.dataPath).Replace("/", "\\");
        #else
                return self.Replace("Assets", Application.dataPath);
        #endif
    }

}