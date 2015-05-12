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
            byte[] key = new byte[256];         // 押されたキーボードの格納用配列

            DX.ChangeWindowMode(DX.TRUE);       // 画面をウィンドウモードにする
            DX.SetGraphMode(800, 600, 32);      // 画面の解像度設定

            /* DXライブラリの初期化 */
            if (DX.DxLib_Init() == -1)
            {
                return;     // エラーの場合は終了
            }

            DX.SetDrawScreen(DX.DX_SCREEN_BACK);        // 描画先を裏画面にする

            GameData gameData = new GameData();     // ゲームクラスの初期化
            gameData.InitStartScreen();             // スタート画面初期化

            /* メインループ */
            while (DX.ScreenFlip() == 0 && DX.ProcessMessage() == 0 && DX.ClearDrawScreen() == 0 && DX.GetHitKeyStateAll(out key[0]) == 0)
            {
                switch (gameData.gmprogstat)
                {
                    case GameData.GameProgressStatus.StartScreen:
                        gameData.Start_Screen();
                        break;

                    case GameData.GameProgressStatus.GamePlay:
                        gameData.GamePlay();
                        break;
                }
            }

            DX.DxLib_End();     // DXライブラリの終了
        }
    }
}