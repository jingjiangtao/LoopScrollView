using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace LoopScrollTest.LoopScroll
{
    [RequireComponent(typeof(ScrollRect))]
    public class LoopScrollView : MonoBehaviour
    {
        public ScrollRect scrollRect;

        // 列表项数组
        protected List<LoopScrollItem> _items = new List<LoopScrollItem>();
        protected float _itemHeight;
        protected int _visibleCount;
        protected float _visibleHeight;
        protected int _sourceListCount;

        // 列表操作接口类型的实例，需要在初始化时赋值
        protected ILoopScrollView _scrollViewOperate;

        protected virtual void Update()
        {
            RefreshGestureScrollView();
        }

        /// <summary>
        /// 初始化循环列表的数值和引用
        /// </summary>
        public virtual void InitScrollView(float itemHeight, ILoopScrollView scrollViewOperate)
        {
            _itemHeight = itemHeight;
            _visibleHeight = (scrollRect.transform as RectTransform).rect.height;
            _visibleCount = (int)(_visibleHeight / _itemHeight) + 1;

            _scrollViewOperate = scrollViewOperate;
        }

        /// <summary>
        /// 初始化循环列表的数据源
        /// </summary>
        public virtual void InitScrollViewList(int sourceListCount)
        {
            _sourceListCount = sourceListCount;
            int generateCount = ResizeContent();
            scrollRect.content.anchoredPosition = Vector2.zero;
            _items.Clear();

            for (int i = 0; i < generateCount; i++)
            {
                GameObject itemGameObject = _scrollViewOperate.InitScrollViewList(i);
                LoopScrollItem item = new LoopScrollItem(itemGameObject);
                float itemY = -i * _itemHeight;
                item.RectTransform.anchoredPosition =
                    new Vector2(scrollRect.content.anchoredPosition.x, itemY);

                _items.Add(item);
            }
        }

        /// <summary>
        /// 将指定索引的项对齐到列表界面的顶部
        /// </summary>
        public virtual void MoveIndexToTop(int index)
        {
            float contentY = index * _itemHeight;
            scrollRect.content.anchoredPosition =
                new Vector2(scrollRect.content.anchoredPosition.x, contentY);

            RefreshGestureScrollView();
        }

        /// <summary>
        /// 将指定索引的项对齐到列表界面的底部
        /// </summary>
        public virtual void MoveIndexToBottom(int index)
        {
            float contentY = (index + 1) * _itemHeight - _visibleHeight;
            contentY = contentY < 0 ? 0f : contentY;
            scrollRect.content.anchoredPosition =
                new Vector2(scrollRect.content.anchoredPosition.x, contentY);

            RefreshGestureScrollView();
        }

        /// <summary>
        /// 判断指定的索引是否需要聚焦到底部，如果需要就对齐
        /// </summary>
        public virtual void MoveToBottomIfNeeded(int index)
        {
            float itemY = -(index + 1) * _itemHeight;
            float bottomY = -(scrollRect.content.anchoredPosition.y + _visibleHeight);

            if (itemY < bottomY)
            {
                MoveIndexToBottom(index);
            }
        }

        /// <summary>
        /// 添加一条新项到列表中
        /// </summary>
        public virtual void AddOneItem()
        {
            _sourceListCount++;
            int generateCount = ResizeContent();

            if (_items.Count < generateCount)
            {
                GameObject itemGameObject = _scrollViewOperate.InitOneItem();
                LoopScrollItem item = new LoopScrollItem(itemGameObject);
                _items.Add(item);
            }

            RefreshGestureScrollView();
        }

        /// <summary>
        /// 删除一条列表项
        /// </summary>
        public virtual void DeleteOneItem()
        {
            _sourceListCount--;
            int generateCount = ResizeContent();
            if (generateCount < _items.Count)
            {
                int lastIndex = _items.Count - 1;
                _scrollViewOperate.DeleteOneItem(_items[lastIndex].RectTransform);
                _items.RemoveAt(lastIndex);
            }

            RefreshGestureScrollView();

            foreach (LoopScrollItem item in _items)
            {
                _scrollViewOperate.OtherItemAfterDeleteOne(item);
            }
        }

        /// <summary>
        /// 根据当前手势项的数量重新调整内容的高度
        /// </summary>
        protected virtual int ResizeContent()
        {
            int generateCount = Mathf.Min(_visibleCount, _sourceListCount);
            float contentHeight = _sourceListCount * _itemHeight;
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, contentHeight);
            return generateCount;
        }

        /// <summary>
        /// 刷新列表内容
        /// </summary>
        protected virtual void RefreshGestureScrollView()
        {
            float contentY = scrollRect.content.anchoredPosition.y;
            int skipCount = (int)(contentY / _itemHeight);

            for (int i = 0; i < _items.Count; i++)
            {
                if (skipCount >= 0 && skipCount < _sourceListCount)
                {
                    _scrollViewOperate.UpdateItemContent(skipCount, _items[i]);

                    float itemY = -skipCount * _itemHeight;
                    _items[i].RectTransform.anchoredPosition =
                        new Vector2(scrollRect.content.anchoredPosition.x, itemY);

                    skipCount++;
                }
            }
        }
    }
}
