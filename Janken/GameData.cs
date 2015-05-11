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

        public static GameProgressStatus gmprogstat { get; set; }       /* ゲームの進行状況プロパティ用 */
        public static GamePlayStatus gmplaystat { get; set; }           /* ゲームプレイの進行状況プロパティ用 */
        
        #endregion

        private static int startScreen_BackGraph = -1;     /* スタート画面の背景画像用変数 */

        private static int gamePlay_BackGraph = -1;        /* ゲームプレイ画面の背景画像用変数 */
        private static int gamePlay_HandGoo = -1;          /* グーの手の画像用変数 */
        private static int gamePlay_HandScissors = -1;     /* チョキの手の画像用変数 */
        private static int gamePlay_HandPer = -1;          /* パーの手の画像用変数 */



        #region

        /**************************************
         * 場面の処理メソッド
         **************************************/

        /// <summary>
        /// ゲームのスタート画面の処理メソッド
        /// </summary>
        /// <returns>正常終了時: 0。それ以外: -1</returns>
        public static int Start_Screen()
        {


            return 0;
        }

        /// <summary>
        /// ゲームプレイ画面の処理メソッド
        /// </summary>
        /// <returns>正常終了時: 0。それ以外: -1</returns>
        public static int GamePlay()
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
        public static void InitStartScreen()
        {
            startScreen_BackGraph = DX.LoadGraph("img/startScreen_BackGraph.png");      // スタート画面の背景読み込み
        }

        /// <summary>
        /// スタート画面ではなくなる時に呼び出すメソッド
        /// </summary>
        public static void EndStartScreen()
        {
            DX.DeleteGraph(startScreen_BackGraph);      // リソースの解放
        }

        /// <summary>
        /// ゲームプレイ画面で使用する画像などの初期化をするメソッド
        /// </summary>
        public static void InitGamePlay()
        {
            gamePlay_BackGraph = DX.LoadGraph("img/gamePlay_BackGraph.png");        // ゲームプレイ画面の背景読み込み
            gamePlay_HandGoo = DX.LoadGraph("img/gamePlay_HandGoo.png");            // グーの画像の読み込み
            gamePlay_HandScissors = DX.LoadGraph("img/HandScissors.png");           // チョキの画像の読み込み
            gamePlay_HandPer = DX.LoadGraph("img/HandPer.png");                     // パーの画像の読み込み
        }

        /// <summary>
        /// ゲームプレイ画面ではなくなる時に呼び出すメソッド
        /// </summary>
        public static void EndGamePlay()
        {
            DX.DeleteGraph(gamePlay_BackGraph);         // リソースの解放
            DX.DeleteGraph(gamePlay_HandGoo);           // リソースの解放
            DX.DeleteGraph(gamePlay_HandScissors);      // リソースの解放
            DX.DeleteGraph(gamePlay_HandPer);           // リソースの解放
        }

        #endregion



        /// <summary>
        /// コンストラクター
        /// </summary>
        static GameData()
        {
            gmprogstat = GameProgressStatus.StartScreen;
            gmplaystat = GamePlayStatus.Prog01;
        }
    }
}
