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
