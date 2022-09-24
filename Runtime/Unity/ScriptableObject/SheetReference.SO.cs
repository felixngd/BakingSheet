﻿// BakingSheet, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections.Generic;
using Cathei.BakingSheet.Internal;
using UnityEngine;

namespace Cathei.BakingSheet
{
    public interface IUnitySheetReference : ISheetReference
    {
        public SheetRowScriptableObject Asset { get; set; }
    }

    public partial class Sheet<TKey, TValue>
    {
        [Serializable]
        public partial class Reference : IUnitySheetReference, ISerializationCallbackReceiver
        {
#if UNITY_EDITOR
            /// <summary>
            /// Row type to filter selection (Editor only)
            /// </summary>
            [SerializeField] internal string typeInfo;
#endif

            [SerializeField] private SheetRowScriptableObject asset;

            SheetRowScriptableObject IUnitySheetReference.Asset
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
#if UNITY_EDITOR
                typeInfo = typeof(TValue).FullName;
#endif

                if (asset == null)
                    return;

                Ref = asset.GetRow<TValue>();
                Id = Ref != null ? Ref.Id : default;
            }
        }
    }
}
