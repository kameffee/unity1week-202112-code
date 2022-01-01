//  ProbabilityCalculator.cs
//  http://kan-kikuchi.hatenablog.com/entry/ProbabilityCalclator2
//
//  Created by kan.kikuchi on 2021.11.30.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1week202112.Domain.Extensions
{
    /// <summary>
    /// 確率計算用クラス(インスタンス作って使う用)
    /// </summary>
    public class ProbabilityCalculator
    {
        private readonly System.Random _random;

        //=================================================================================
        //初期化
        //=================================================================================

        public ProbabilityCalculator(int seed)
        {
            _random = new System.Random(seed);
        }

        //=================================================================================
        //真偽を判定
        //=================================================================================

        /// <summary>
        /// 入力した確率で判定を行う
        /// </summary>
        public bool Detect(int percent)
        {
            return Detect((float)percent);
        }

        /// <summary>
        /// 入力した確率で判定を行う
        /// </summary>
        public bool Detect(float percent)
        {
            //小数点以下の桁数
            int digitNum = 0;
            if (percent.ToString().IndexOf(".") > 0)
            {
                digitNum = percent.ToString().Split(new[] { "." }, StringSplitOptions.None)[1].Length;
            }

            //小数点以下を無くすように乱数の上限と判定の境界を上げる
            int rate = (int)Mathf.Pow(10, digitNum);

            //乱数の上限と真と判定するボーダーを設定
            int randomValueLimit = 100 * rate;
            int border = (int)(rate * percent);

            return _random.Next(0, randomValueLimit) < border;
        }

        //=================================================================================
        //配列
        //=================================================================================

        /// <summary>
        /// 入力した物の中から一つをランダムに取得する
        /// </summary>
        public T Determine<T>(params T[] targetArray)
        {
            return targetArray[_random.Next(0, targetArray.Length)];
        }

        /// <summary>
        /// 入力したListから一つをランダムに取得する
        /// </summary>
        public T Determine<T>(List<T> targetList)
        {
            return Determine(targetList.ToArray());
        }

        //=================================================================================
        //Dictionary
        //=================================================================================

        /// <summary>
        /// 確率とその対象をまとめたDictを入力しその中から一つを決定、対象を返す
        /// </summary>
        public T Determine<T>(Dictionary<T, int> targetDict)
        {
            //累計確率
            int totalPer = 0;
            foreach (var per in targetDict.Values)
            {
                totalPer += per;
            }

            //0〜累計確率の間で乱数を作成
            float rand = _random.Next(0, totalPer);

            //乱数から各確率を引いていき、0未満になったら終了
            foreach (KeyValuePair<T, int> pair in targetDict)
            {
                rand -= pair.Value;

                if (rand <= 0)
                {
                    return pair.Key;
                }
            }

            //エラー、ここに来た時はプログラムが間違っている
            Debug.LogWarning("抽選ができませんでした");
            return new List<T>(targetDict.Keys)[0];
        }

        /// <summary>
        /// 確率とその対象をまとめたDictを入力しその中から一つを決定、対象を返す
        /// </summary>
        public T Determine<T>(Dictionary<T, float> targetDict)
        {
            //一番多い桁数を取得
            int digitNum = 0;
            foreach (var pair in targetDict)
            {
                if (pair.Value.ToString().IndexOf(".") > 0)
                {
                    digitNum = Mathf.Max(digitNum,
                        pair.Value.ToString().Split(new[] { "." }, StringSplitOptions.None)[1].Length);
                }
            }

            //小数点がなくなるように変換してintで判定
            int rate = (int)Mathf.Pow(10, digitNum);
            return Determine(targetDict.ToDictionary(pair => pair.Key, pair => Mathf.RoundToInt(pair.Value * rate)));
        }
    }
}
