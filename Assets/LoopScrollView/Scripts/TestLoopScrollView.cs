using LoopScrollTest.LoopScroll;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LoopScrollTest
{
    public class TestLoopScrollView : MonoBehaviour, ILoopScrollView
    {
        [Tooltip("源数据列表的个数，修改后需要重新运行才能生效")]
        public uint count = 300;

        // 对滚动列表的引用
        public LoopScrollView loopScrollView;

        // 对添加按钮的引用
        public Button addBtn;

        // 对ScrollView的Content物体的引用
        public RectTransform content;

        // 对单个列表项预制体的引用
        public TextItem textItemPrefab;

        // 源数据列表
        private List<SourceData> _sourceList = new List<SourceData>();
        private int nextId = 1;

        private void Awake()
        {
            addBtn.onClick.AddListener(OnClickAddItem);

            // 获取单个列表项的高度
            float itemHeight = (textItemPrefab.transform as RectTransform).rect.height;
            // 初始化滚动列表中的引用
            loopScrollView.InitScrollView(itemHeight, this);
        }

        private void Start()
        {
            // 初始化源数据列表，作为列表的数据源
            for (int i = 0; i < count; i++)
            {
                SourceData data = new SourceData
                {
                    text = nextId.ToString()
                };

                _sourceList.Add(data);

                nextId++;
            }

            // 初始化滚动列表的显示
            loopScrollView.InitScrollViewList(_sourceList.Count);
        }

        /// <summary>
        /// 在列表中追加新项
        /// </summary>
        private void OnClickAddItem()
        {
            SourceData data = new SourceData
            {
                text = nextId.ToString(),
            };

            _sourceList.Add(data);
            nextId++;

            loopScrollView.AddOneItem();
            loopScrollView.MoveIndexToBottom(_sourceList.Count - 1);
        }

        /// <summary>
        /// 根据给定的源数据生成单个列表项
        /// </summary>
        private TextItem InitTextItem(SourceData sourceData)
        {
            TextItem item = Instantiate(textItemPrefab, content);
            item.Init(sourceData, DeleteItemAction);
            return item;
        }

        /// <summary>
        /// 点击单个列表项的删除按钮的回调，删除单个列表项
        /// </summary>
        private void DeleteItemAction(TextItem item)
        {
            _sourceList.Remove(item.Data);
            loopScrollView.DeleteOneItem();
        }

        #region 实现的接口方法

        public virtual void DeleteOneItem(Transform item)
        {
            DestroyImmediate(item.gameObject);
        }

        public virtual GameObject InitOneItem()
        {
            TextItem item = InitTextItem(_sourceList[_sourceList.Count - 1]);
            return item.gameObject;
        }

        public virtual GameObject InitScrollViewList(int index)
        {
            TextItem item = InitTextItem(_sourceList[index]);
            return item.gameObject;
        }

        public virtual void OtherItemAfterDeleteOne(LoopScrollItem item)
        {

        }

        public virtual void UpdateItemContent(int index, LoopScrollItem item)
        {
            // 更新列表项引用的源数据
            TextItem textItem = item.RectTransform.GetComponent<TextItem>();
            textItem.UpdateSourceData(_sourceList[index]);
        }

        #endregion
    }
}
