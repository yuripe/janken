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
            byte[] pressKey = new byte[256];        // 押されたキーボードの格納用配列

            DX.ChangeWindowMode(DX.TRUE);       // 画面をウィンドウモードにする
            DX.SetGraphMode(800, 600, 32);      // 画面の解像度設定

            /* DXライブラリの初期化 */
            if (DX.DxLib_Init() == -1)
            {
                return;     // エラーの場合は終了
            }

            DX.SetDrawScreen(DX.DX_SCREEN_BACK);        // 描画先を裏画面にする

            GameData.InitStartScreen();     // スタート画面初期化

            /* メインループ */
            while (DX.ScreenFlip() == 0 && DX.ProcessMessage() == 0 && DX.ClearDrawScreen() == 0 && DX.GetHitKeyStateAll(out pressKey[0]) == 0)
            {
                switch (GameData.gmprogstat)
                {
                    case GameData.GameProgressStatus.StartScreen:
                        GameData.Start_Screen();
                        break;

                    case GameData.GameProgressStatus.GamePlay:
                        GameData.GamePlay();
                        break;
                }

                if (pressKey[DX.KEY_INPUT_ESCAPE] == 1)
                    break;
            }

            DX.DxLib_End();     // DXライブラリの終了
        }
    }
}