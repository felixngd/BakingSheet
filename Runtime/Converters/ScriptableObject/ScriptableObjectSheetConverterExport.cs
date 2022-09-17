﻿// BakingSheet, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

#if UNITY_EDITOR

using System;
using System.Threading.Tasks;
using Cathei.BakingSheet.Internal;
using UnityEditor;

namespace Cathei.BakingSheet
{
    public partial class ScriptableObjectSheetConverter : ISheetConverter
    {
        public Task<bool> Export(SheetConvertingContext context)
        {
            var props = context.Container.GetSheetProperties();

            foreach (var prop in props)
            {
                prop.GetValue(context.Container);


            }



            AssetDatabase.CreateAsset();

            AssetDatabase.AddObjectToAsset();


            throw new NotImplementedException();
        }
    }
}

#endif