using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LoopScrollTest.LoopScroll
{
    public interface ILoopScrollView
    {
        /// <summary>
        /// 用给定索引处的数据更新给定的列表项
        /// </summary>
        void UpdateItemContent(int index, LoopScrollItem item);

        /// <summary>
        /// 用给定索引处的数据生成列表项并返回
        /// </summary>
        GameObject InitScrollViewList(int index);

        /// <summary>
        /// 生成一个新的列表项并返回
        /// </summary>
        GameObject InitOneItem();

        /// <summary>
        /// 删除指定的列表项
        /// </summary>
        void DeleteOneItem(Transform item);

        /// <summary>
        /// 删除一个列表项之后处理其它所有的列表项
        /// </summary>
        void OtherItemAfterDeleteOne(LoopScrollItem item);
    }
}
