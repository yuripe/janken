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

        static enum GameProgressStatus
        {
            StartScreen,        /* スタート画面 */
            GamePlay            /* ゲームプレイ画面 */
        }

        static enum GamePlayStatus
        {
            HandSelect,         /* 手の選択画面 */
            SelectContinue,     /* 続けるかの選択画面 */
            None                /* それ以外 */
        }

        #endregion



        #region

        /**************************************
         * 場面の処理メソッド
         **************************************/

        /// <summary>
        /// ゲームのスタート画面の処理メソッド
        /// </summary>
        /// <returns>正常終了時: 0。それ以外: -1</returns>
        static int Start_Screen()
        {
            return 0;
        }

        /// <summary>
        /// ゲームプレイ画面の処理メソッド
        /// </summary>
        /// <returns>正常終了時: 0。それ以外: -1</returns>
        static int GamePlay()
        {
            return 0;
        }

        #endregion
    }
}
