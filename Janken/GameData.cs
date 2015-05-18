﻿using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Janken
{
    public class GameData
    {
        #region

        /**************************************
         * ゲーム定数の定義
         **************************************/

        public enum GameProgressStatus
        {
            StartScreen,        /* スタート画面 */
            GamePlay,           /* ゲームプレイ画面 */
            END                 /* ゲーム終了 */
        }

        public enum GamePlayStatus
        {
            Prog01,             /* 手の選択画面まで */
            HandSelect,         /* 手の選択画面 */
            Prog02,             /* 勝敗判定まで */
            SelectContinue,     /* 続けるかの選択画面 */
        }

        enum Hand
        {
            Goo,        /* グー */
            Scissors,   /* チョキ */
            Per         /* パー */
        }

        enum JudgeResult
        {
            WIN,
            DRAW,
            LOSE
        }
        /* 勝敗のグローバル変数 */
        int w = 0;    /* 勝ち */
        int l = 0;    /* 負け */
        int d = 0;    /* あいこ */
        int d2 = 0;
        int w2 = 0;
        int l2 = 0;

        #endregion



        #region

        /**************************************
         * プロパティ
         **************************************/

        public GameProgressStatus gmprogstat { get; set; }      /* ゲームの進行状況プロパティ用 */
        public GamePlayStatus gmplaystat { get; set; }          /* ゲームプレイの進行状況プロパティ用 */
        public int GameFPS { get; set; }                        /* ゲームのFPS */

        #endregion

        private int selectMenuId = 0;               /* 選択されたメニュー用変数 */
        private bool isPressKey = false;            /* 前のフレームでキーボードを押していたか */

        private int[] gamePlay_HandImg = new int[] { -1, -1, -1 };    /* 手の画像を格納する配列変数（定数でアクセス可能） */
        private int frameCounter = 1;                                 /* 何フレーム目かをカウントする変数 */
        private Hand playerHand = 0;                                  /* プレイヤーの選択した手を保存する変数 */
        private Hand enemyHand = 0;                                   /* 敵の選択した手を保存する変数 */
        private JudgeResult judgeResult;                              /* じゃんけんの結果 */



        #region

        /**************************************
         * 場面の処理メソッド
         **************************************/

        /// <summary>
        /// ゲームのスタート画面の処理メソッド
        /// </summary>
        /// <param name="key">押されたキーボードの情報配列</param>
        /// <returns>正常終了時: 0。それ以外: -1</returns>
        public int Start_Screen(byte[] key)
        {
            // 上キー or 下キーが押されていたらメニュー選択の切り替え
            if ((key[DX.KEY_INPUT_UP] == 1 && !isPressKey) || (key[DX.KEY_INPUT_DOWN] == 1 && !isPressKey))
            {
                selectMenuId = selectMenuId == 0 ? 1 : 0;
                isPressKey = true;
            }

            if (key[DX.KEY_INPUT_RETURN] == 1)      // メニューを選択（Enter）
            {
                // ゲームスタートを選択, ゲーム画面へ
                if (selectMenuId == 0)
                {
                    gmprogstat = GameProgressStatus.GamePlay;
                    InitGamePlay();     // ゲームプレイ画面のリソース準備
                    EndStartScreen();   // スタート画面のリソース開放
                }

                // 終了を選択, 終了画面へ
                else
                {
                    gmprogstat = GameProgressStatus.END;
                    EndStartScreen();   // スタート画面のリソース開放
                }
            }

            // キーフラグの解除
            if (key[DX.KEY_INPUT_UP] == 0 && key[DX.KEY_INPUT_DOWN] == 0 && isPressKey)
            {
                isPressKey = false;
            }

            int x = CalcCenterX("-> ゲームスタート") - DX.GetFontSize(), y = 360;                       // 文字の表示位置
            int selectColor = DX.GetColor(255, 100, 100), menuColor = DX.GetColor(255, 255, 255);       // メニューの文字カラー

            /* タイトル */
            DX.SetFontSize(36);
            DX.DrawString(CalcCenterX("じゃんけんゲーム"), 235, "じゃんけんゲーム", DX.GetColor(255, 153, 0));

            DX.SetFontSize(16);


            // 選択されているメニューによって表示を変える
            switch (selectMenuId)
            {
                case 0:     // ゲームスタートが選択されている
                    DX.DrawString(x, y, "-> ゲームスタート", selectColor);
                    DX.DrawString(x, (int)(y + DX.GetFontSize() * 1.5), "   終了", menuColor);
                    break;
                case 1:     // 終了が選択されている
                    DX.DrawString(x, y, "   ゲームスタート", menuColor);
                    DX.DrawString(x, (int)(y + DX.GetFontSize() * 1.5), "-> 終了", selectColor);
                    break;
            }

            return 0;
        }

        /// <summary>
        /// ゲームプレイ画面の処理メソッド
        /// </summary>
        /// <param name="key">押されたキーボードの情報配列</param>
        /// <returns>正常終了時: 0。それ以外: -1</returns>
        public int GamePlay(byte[] key)
        {
            switch (gmplaystat)
            {
                case GamePlayStatus.Prog01: // ゲームプレイ第1場面
                    if (frameCounter <= 60) // 60フレーム以下（2秒間）
                    {

                        DX.SetFontSize(24); // 文字のサイズの変更
                        DX.DrawString(CalcCenterX("ゲームを開始します"), 288, "ゲームを開始します", DX.GetColor(200, 200, 200));

                        DX.SetFontSize(16);
                        frameCounter++;
                    }

                    else if (frameCounter <= 120) // 120フレーム以下（2秒間）
                    {
                        DX.SetFontSize(24);
                        DX.DrawString(CalcCenterX("最初はグー！"), 288, "最初はグー！", DX.GetColor(200, 200, 200));
                        DX.DrawRotaGraph(400, 100, 0.34, Math.PI, gamePlay_HandImg[(int)Hand.Goo], DX.TRUE);   // 相手の手の画像を表示

                        DX.SetFontSize(16);
                        frameCounter++;

                        /* 判定用変数の初期化 */
                        d2 = 0;
                        w2 = 0;
                        l2 = 0;
                    }
                    else if (frameCounter <= 180) // 180フレーム以下（2秒間）
                    {
                        DX.SetFontSize(24);
                        DX.DrawString(CalcCenterX("じゃんけん！"), 288, "じゃんけん！", DX.GetColor(200, 200, 200));
                        DX.DrawRotaGraph(400, 100, 0.34, Math.PI, gamePlay_HandImg[(int)Hand.Goo], DX.TRUE);   // 相手の手の画像を表示

                        DX.SetFontSize(16);
                        frameCounter++;
                    }
                    else // 次の処理へ
                    {
                        frameCounter = 1;
                        gmplaystat = GamePlayStatus.HandSelect;     // 次の場面へ行くためのフラグセット
                    }
                    break;

                case GamePlayStatus.HandSelect: // 手を選択する場面
                    DX.SetFontSize(24);
                    DX.DrawString(CalcCenterX("手を選んでください！"), 264, "手を選んでください！", DX.GetColor(200, 200, 200));
                    DX.DrawString(CalcCenterX("グー…0　チョキ…1　パー…2"), 288, "グー…0　チョキ…1　パー…2", DX.GetColor(255, 100, 100));

                    DX.DrawRotaGraph(400, 100, 0.34, Math.PI, gamePlay_HandImg[(int)Hand.Goo], DX.TRUE);   // 相手の手の画像を表示

                    DX.DrawRotaGraph(100, 500, 0.34, 0, gamePlay_HandImg[(int)Hand.Goo], DX.TRUE);        // 手の画像を表示　グー
                    DX.DrawRotaGraph(400, 495, 0.34, 0, gamePlay_HandImg[(int)Hand.Scissors], DX.TRUE);   // 手の画像を表示　チョキ
                    DX.DrawRotaGraph(700, 500, 0.34, 0, gamePlay_HandImg[(int)Hand.Per], DX.TRUE);        // 手の画像を表示　パー


                    if (key[DX.KEY_INPUT_0] == 1)   // グーを選択
                    {
                        playerHand = 0; // 自分の手を格納
                        enemyHand = (Hand)DX.GetRand(2);      // 敵の手を決定
                        gmplaystat = GamePlayStatus.Prog02;  // 次の場面へ行くためのフラグセット
                    }
                    else if (key[DX.KEY_INPUT_1] == 1)  // チョキを選択
                    {
                        playerHand = (Hand)1;
                        enemyHand = (Hand)DX.GetRand(2);      // 敵の手を決定
                        gmplaystat = GamePlayStatus.Prog02;
                    }
                    else if (key[DX.KEY_INPUT_2] == 1)  // パーを選択
                    {
                        playerHand = (Hand)2;
                        enemyHand = (Hand)DX.GetRand(2);      // 敵の手を決定
                        gmplaystat = GamePlayStatus.Prog02;
                    }

                    DX.SetFontSize(16);
                    break;

                case GamePlayStatus.Prog02: // 勝敗判定
                    if (frameCounter <= 60)    // 60フレーム以下（2秒間）
                    {
                        DX.SetFontSize(24);
                        DX.DrawString(CalcCenterX("ポン！"), 288, "ポン！", DX.GetColor(200, 200, 200));
                        DX.DrawRotaGraph(400, 100, 0.34, Math.PI, gamePlay_HandImg[(int)enemyHand], DX.TRUE);    // 相手の手の画像を表示
                        DX.DrawRotaGraph(400, 495, 0.34, 0, gamePlay_HandImg[(int)playerHand], DX.TRUE);         // 自分の手の画像を表示
                        judgeResult = Judge(playerHand, enemyHand);     // 判定
                        frameCounter++;
                    }
                    else if(frameCounter <= 150)    // 150フレーム以下（3秒間）
                    {
                        DX.SetFontSize(24);

                        switch(judgeResult)     // 判定ごとの表示
                        {
                            case JudgeResult.WIN:
                                DX.DrawString(CalcCenterX("あなたの勝ち！！"), 288, "あなたの勝ち！！", DX.GetColor(255, 50, 50));
                                break;
                            case JudgeResult.DRAW:
                                DX.DrawString(CalcCenterX("引き分け"), 288, "引き分け", DX.GetColor(50, 255, 50));
                                break;
                            case JudgeResult.LOSE:
                                DX.DrawString(CalcCenterX("あなたの負け・・"), 288, "あなたの負け・・", DX.GetColor(50, 50, 255));
                                break;
                        }

                        frameCounter++;
                    }
                    else    // 終了判定
                    {
                        // 上キー or 下キーが押されていたらメニュー選択の切り替え
                        if ((key[DX.KEY_INPUT_UP] == 1 && !isPressKey) || (key[DX.KEY_INPUT_DOWN] == 1 && !isPressKey))
                        {
                            selectMenuId = selectMenuId == 0 ? 1 : 0;
                            isPressKey = true;
                        }

                        if (key[DX.KEY_INPUT_RETURN] == 1)      // メニューを選択（Enter）
                        {
                            // もう一度を選択, ゲーム画面へ
                            if (selectMenuId == 0)
                            {
                                gmplaystat = GamePlayStatus.Prog01;
                                frameCounter = 1;
                            }

                            // 終了を選択, 終了画面へ
                            else
                            {
                                gmprogstat = GameProgressStatus.END;
                                EndGamePlay();   // ゲーム画面のリソース開放
                            }
                        }

                        // キーフラグの解除
                        if (key[DX.KEY_INPUT_UP] == 0 && key[DX.KEY_INPUT_DOWN] == 0 && isPressKey)
                        {
                            isPressKey = false;
                        }

                        int x = CalcCenterX("もう一度") - DX.GetFontSize(), y = 360;                                // 文字の表示位置
                        int selectColor = DX.GetColor(255, 100, 100), menuColor = DX.GetColor(255, 255, 255);       // メニューの文字カラー

                        DX.DrawString(CalcCenterX("0勝0敗0分"), 235, w +"勝"+ l +"敗"+ d +"分", DX.GetColor(255, 255, 255));   //勝敗の表示

                        // 選択されているメニューによって表示を変える
                        switch (selectMenuId)
                        {
                            case 0:     // もう一度が選択されている
                                DX.DrawString(x, y, "もう一度", selectColor);
                                DX.DrawString(x, (int)(y + DX.GetFontSize() * 1.5), "終了", menuColor);
                                break;
                            case 1:     // 終了が選択されている
                                DX.DrawString(x, y, "もう一度", menuColor);
                                DX.DrawString(x, (int)(y + DX.GetFontSize() * 1.5), "終了", selectColor);
                                break;
                        }
                    }
                    DX.SetFontSize(16);
                    break;
            }

            return 0;
        }

        #endregion



        #region

        /**************************************
         * 場面ごとの初期化・終了時のメソッド
         **************************************/

        /// <summary>
        /// スタート画面で使用する画像などの初期化をするメソッド
        /// </summary>
        public void InitStartScreen()
        {

        }

        /// <summary>
        /// スタート画面ではなくなる時に呼び出すメソッド
        /// </summary>
        public void EndStartScreen()
        {

        }

        /// <summary>
        /// ゲームプレイ画面で使用する画像などの初期化をするメソッド
        /// </summary>
        public void InitGamePlay()
        {
            gamePlay_HandImg[(int)Hand.Goo] = DX.LoadGraph("img/gamePlay_HandGoo.png");                     // グーの画像の読み込み
            gamePlay_HandImg[(int)Hand.Scissors] = DX.LoadGraph("img/gamePlay_HandScissors.png");           // チョキの画像の読み込み
            gamePlay_HandImg[(int)Hand.Per] = DX.LoadGraph("img/gamePlay_HandPer.png");                     // パーの画像の読み込み
        }

        /// <summary>
        /// ゲームプレイ画面ではなくなる時に呼び出すメソッド
        /// </summary>
        public void EndGamePlay()
        {
            DX.DeleteGraph(gamePlay_HandImg[(int)Hand.Goo]);           // リソースの解放
            DX.DeleteGraph(gamePlay_HandImg[(int)Hand.Scissors]);      // リソースの解放
            DX.DeleteGraph(gamePlay_HandImg[(int)Hand.Per]);           // リソースの解放
        }

        /// <summary>
        /// デストラクター
        /// </summary>
        ~GameData()
        {
            DX.DeleteGraph(gamePlay_HandImg[(int)Hand.Goo]);           // リソースの解放
            DX.DeleteGraph(gamePlay_HandImg[(int)Hand.Scissors]);      // リソースの解放
            DX.DeleteGraph(gamePlay_HandImg[(int)Hand.Per]);           // リソースの解放
        }

        #endregion



        /// <summary>
        /// 文字列の中央表示をするために必要なX座標を取得するメソッド
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns></returns>
        private int CalcCenterX(string str)
        {
            int x1 = 0, x2 = 0;
            int y = 0;
            int strWidth;

            DX.GetWindowSize(out x2, out y);


            strWidth = DX.GetDrawStringWidth(str, Encoding.GetEncoding("Shift_JIS").GetByteCount(str));

            return (int)((x1 + ((x2 - x1) / 2)) - (strWidth / 2));
        }

        /// <summary>
        /// じゃんけんの判定をするメソッド
        /// </summary>
        /// <param name="playerH">プレイヤーの手</param>
        /// <param name="enemyH">相手の手</param>
        /// <returns></returns>
        private JudgeResult Judge(Hand playerH, Hand enemyH)
        {
            int j = ((int)playerHand - (int)enemyH + 3) % 3;

            switch (j)
            {
                case 0:
                    d2++;
                    if (d2 >= 60){d++;}
                    return JudgeResult.DRAW;
                case 2:
                    w2++;
                    if (w2 >= 60){w++;}
                    return JudgeResult.WIN;
                default:
                    l2++;
                    if (l2 >= 60){l++;}
                    return JudgeResult.LOSE;
            }
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public GameData()
        {
            gmprogstat = GameProgressStatus.StartScreen;
            gmplaystat = GamePlayStatus.Prog01;
            InitStartScreen();      // スタート画面のリソース準備
        }
    }
}