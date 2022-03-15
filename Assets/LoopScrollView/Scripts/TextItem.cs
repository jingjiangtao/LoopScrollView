using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace LoopScrollTest
{
    public class TextItem : MonoBehaviour
    {
        public Text text;
        public Button deleteBtn;

        public SourceData Data => _sourceData;

        private Action<TextItem> _deleteItemAction;
        private SourceData _sourceData;

        private void Awake()
        {
            deleteBtn.onClick.AddListener(OnClickDelete);
        }

        /// <summary>
        /// 初始化数据和按钮点击的委托
        /// </summary>
        public void Init(SourceData data, Action<TextItem> deleteItemAction)
        {
            _sourceData = data;
            _deleteItemAction = deleteItemAction;

            text.text = _sourceData.text;
        }

        /// <summary>
        /// 更新引用的源数据
        /// </summary>
        public void UpdateSourceData(SourceData data)
        {
            _sourceData = data;
            text.text = _sourceData.text;
        }

        /// <summary>
        /// 删除按钮的点击事件
        /// </summary>
        private void OnClickDelete()
        {
            _deleteItemAction?.Invoke(this);
        }
    }
}
