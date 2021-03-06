﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SEImageListView
{
    public class ImageListViewItem
    {
        #region 私有成员

        /// <summary>
        /// 获取项的图像
        /// 加这个委托是为了延迟加载
        /// 只在需要第一次呈现时才会调用，初始化ImageListViewItem时只传一个名子就可以
        /// 无需在加载项时为所有项获取生成Image对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public delegate Image GetImageHandler(object key);
        private GetImageHandler _getImageHandler;

        #endregion

        #region 受保护的成员

        private ImageListViewCollection _ownerCollection;
        internal ImageListViewCollection OwnerCollection
        {
            get { return _ownerCollection; }
            set { _ownerCollection = value; }
        }

        #endregion

        #region 公开属性

        public int Index
        {
            get
            {
                return _ownerCollection.IndexOf(this);
            }
        }

        private ImageListViewItemState _state = ImageListViewItemState.None;
        /// <summary>
        /// 该项当前的选中状态
        /// </summary>
        public ImageListViewItemState State
        {
            get { return _state; }
        }

        public bool Selected
        {
            get
            {
                return (_state & ImageListViewItemState.Selected) == ImageListViewItemState.Selected;
            }
            set
            {
                bool selected = Selected;

                if (value)
                    _state = _state | ImageListViewItemState.Selected;
                else
                    _state = _state ^ ImageListViewItemState.Selected;

                if (selected != Selected)
                    Render();
            }
        }

        public bool Hovered
        {
            get
            {
                return (_state & ImageListViewItemState.Hovered) == ImageListViewItemState.Hovered;
            }
            set
            {
                bool hovered = Hovered;

                if (value)
                    _state = _state | ImageListViewItemState.Hovered;
                else
                    _state = _state ^ ImageListViewItemState.Hovered;

                if (hovered != Hovered)
                    Render();
            }
        }

        public bool Focused
        {
            get
            {
                return (_state & ImageListViewItemState.Focused) == ImageListViewItemState.Focused;
            }
            set
            {
                bool focused = Focused;

                if (value)
                    _state = _state | ImageListViewItemState.Focused;
                else
                    _state = _state ^ ImageListViewItemState.Focused;

                if (focused != Focused)
                    Render();
            }
        }

        /// <summary>
        /// 项的唯一标识
        /// 可以是一个文件名，或者就是FileInfo
        /// </summary>
        public object Key
        {
            get;
            set;
        }

        /// <summary>
        /// 呈现在缩略图下方的标题文本
        /// </summary>
        public string Header
        {
            get;
            set;
        }

        private Image _image;
        public Image Image
        {
            get
            {
                if (_image == null)
                    _image = _getImageHandler(Key);

                return _image;
            }
        }

        #endregion

        #region 构造

        public ImageListViewItem(object key, GetImageHandler getImageHandler)
            : this(key, key.ToString(), getImageHandler)
        {

        }

        public ImageListViewItem(object key, string header, GetImageHandler getImageHandler)
        {
            this.Key = key;
            this.Header = header;
            this._getImageHandler = getImageHandler;
        }

        #endregion

        #region 私有方法

        private void Render()
        {
            _ownerCollection.Owner.RenderItem(this);
        }

        #endregion
    }
}
