using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1week202112.Presentation.CommandCard
{
    public class CardSlotList : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _mainHolder;

        [SerializeField]
        private List<RectTransform> _topHolders;

        [SerializeField]
        private List<RectTransform> _bottomHolders;

        public RectTransform MainHolder => _mainHolder;

        public RectTransform GetHolder(Vector2Int pos)
        {
            return GetHolder(pos.y, pos.x);
        }

        /// <summary>
        /// ホルダーのTransformを取得する
        /// </summary>
        /// <param name="row"> 0 ~ 1</param>
        /// <param name="column">0 ~ 6</param>
        public RectTransform GetHolder(int row, int column)
        {
            if (row < 0 || row > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "rowが範囲外");
            }

            if (column < 0 || column > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Columnが範囲外");
            }

            return row == 0 ? _topHolders[column] : _bottomHolders[column];
        }
    }
}
