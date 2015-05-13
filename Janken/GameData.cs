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
            GamePlay            /* ゲームプレイ画面 */
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