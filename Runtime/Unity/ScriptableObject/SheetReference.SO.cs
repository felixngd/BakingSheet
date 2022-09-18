﻿// BakingSheet, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections.Generic;
using Cathei.BakingSheet.Internal;
using UnityEngine;

namespace Cathei.BakingSheet
{
    public partial interface ISheetReference
    {
        public SheetRowScriptableObject SO { get; set; }
    }

    public partial class Sheet<TKey, TValue>
    {
        [Serializable]
        public partial struct Reference : ISerializationCallbackReceiver
        {
            [SerializeField]
            private SheetRowScriptableObject asset;

            SheetRowScriptableObject ISheetReference.SO
            {
                get => asset;
                set => asset = value;
            }

            public void OnBeforeSerialize()
            {
                // do nothing
            }

            public void OnAfterDeserialize()
            {
                Ref = asset.GetRow<TValue>();
                Id = Ref != null ? Ref.Id : default;
            }
        }
    }
}