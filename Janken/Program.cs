using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;


namespace Janken
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] pressKey = new byte[256];     // 押されたキーボードの格納用配列

            DX.ChangeWindowMode(DX.TRUE);       // 画面をウィンドウモードにする
            DX.SetGraphMode(800, 600, 32);      // 画面の解像度設定

            /* DXライブラリの初期化 */
            if (DX.DxLib_Init() == -1)
            {
                return;     // エラーの場合は終了
            }

            DX.SetDrawScreen(DX.DX_SCREEN_BACK);        // 描画先を裏画面にする


            /* メインループ */
            while (DX.ScreenFlip() == 0 && DX.ProcessMessage() == 0 && DX.ClearDrawScreen() == 0 && DX.GetHitKeyStateAll(out pressKey[0]) == 0)
            {
                /* Kキーが押されたらループを抜ける */
                if (pressKey[DX.KEY_INPUT_K] == 1)
                    break;

                DX.DrawString(100, 100, "Hello!!", DX.GetColor(0, 100, 255));       // 文字表示
            }

            DX.DxLib_End();     // DXライブラリの終了
        }
    }
}