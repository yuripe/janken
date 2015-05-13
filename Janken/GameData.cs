using DxLibDLL;
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

        #endregion



        #region

        /**************************************
         * プロパティ
         **************************************/

        public GameProgressStatus gmprogstat { get; set; }      /* ゲームの進行状況プロパティ用 */
        public GamePlayStatus gmplaystat { get; set; }          /* ゲームプレイの進行状況プロパティ用 */
        public int GameFPS { get; set; }                        /* ゲームのFPS */

        #endregion

        private int startScreen_BackGraph = -1;     /* スタート画面の背景画像用変数 */
        private int selectMenuId = 0;               /* 選択されたメニュー用変数 */
        private bool isPressKey = false;            /* 前のフレームでキーボードを押していたか */

        private int gamePlay_BackGraph = -1;        /* ゲームプレイ画面の背景画像用変数 */
        private int gamePlay_HandGoo = -1;          /* グーの手の画像用変数 */
        private int gamePlay_HandScissors = -1;     /* チョキの手の画像用変数 */
        private int gamePlay_HandPer = -1;          /* パーの手の画像用変数 */



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
                    gmprogstat = GameProgressStatus.GamePlay;
                
                // 終了を選択, 終了画面へ
                else
                    gmprogstat = GameProgressStatus.END;
            }

            // キーフラグの解除
            if (key[DX.KEY_INPUT_UP] == 0 && key[DX.KEY_INPUT_DOWN] == 0 && isPressKey)
            {
                isPressKey = false;
            }

            int x = CalcCenterX("-> ゲームスタート"), y = 360;                                          // 文字の表示位置
            int selectColor = DX.GetColor(255, 100, 100), menuColor = DX.GetColor(255, 255, 255);       // メニューの文字カラー

            // 選択されているメニューによって表示を変える
            switch (selectMenuId)
            {
                case 0:     // ゲームスタートが選択されている
                    DX.DrawString(x - DX.GetFontSize() * 3, y, "-> ", selectColor);
                    DX.DrawString(x, y, "ゲームスタート", selectColor);
                    DX.DrawString(x, (int)(y + DX.GetFontSize() * 1.5), "終了", menuColor);
                    break;
                case 1:     // 終了が選択されている
                    DX.DrawString(x - DX.GetFontSize() * 3, (int)(y + DX.GetFontSize() * 1.5), "-> ", selectColor);
                    DX.DrawString(x, y, "ゲームスタート", menuColor);
                    DX.DrawString(x, (int)(y + DX.GetFontSize() * 1.5), "終了", selectColor);
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
            startScreen_BackGraph = DX.LoadGraph("img/startScreen_BackGraph.png");      // スタート画面の背景読み込み
        }

        /// <summary>
        /// スタート画面ではなくなる時に呼び出すメソッド
        /// </summary>
        public void EndStartScreen()
        {
            DX.DeleteGraph(startScreen_BackGraph);      // リソースの解放
        }

        /// <summary>
        /// ゲームプレイ画面で使用する画像などの初期化をするメソッド
        /// </summary>
        public void InitGamePlay()
        {
            gamePlay_BackGraph = DX.LoadGraph("img/gamePlay_BackGraph.png");        // ゲームプレイ画面の背景読み込み
            gamePlay_HandGoo = DX.LoadGraph("img/gamePlay_HandGoo.png");            // グーの画像の読み込み
            gamePlay_HandScissors = DX.LoadGraph("img/HandScissors.png");           // チョキの画像の読み込み
            gamePlay_HandPer = DX.LoadGraph("img/HandPer.png");                     // パーの画像の読み込み
        }

        /// <summary>
        /// ゲームプレイ画面ではなくなる時に呼び出すメソッド
        /// </summary>
        public void EndGamePlay()
        {
            DX.DeleteGraph(gamePlay_BackGraph);         // リソースの解放
            DX.DeleteGraph(gamePlay_HandGoo);           // リソースの解放
            DX.DeleteGraph(gamePlay_HandScissors);      // リソースの解放
            DX.DeleteGraph(gamePlay_HandPer);           // リソースの解放
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
        /// コンストラクター
        /// </summary>
        public GameData()
        {
            gmprogstat = GameProgressStatus.StartScreen;
            gmplaystat = GamePlayStatus.Prog01;
        }
    }
}