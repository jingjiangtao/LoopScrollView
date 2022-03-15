using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LoopScrollTest.LoopScroll
{
    public class LoopScrollItem
    {
        public RectTransform RectTransform => _transform;

        private RectTransform _transform;
        private GameObject _gameObject;

        public LoopScrollItem(GameObject gameObject)
        {
            _gameObject = gameObject;
            _transform = _gameObject.transform as RectTransform;
        }
    }
}
