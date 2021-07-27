﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cathei.BakingSheet
{
    public abstract partial class Sheet<TKey, TValue> : KeyedCollection<TKey, TValue>, ISheet
        where TValue : SheetRow<TKey>, new()
    {
        public string Name { get; set; }

        public Type RowType => typeof(TValue);

        public new TValue this[TKey id]
        {
            get
            {
                if (id == null || !Contains(id))
                    return default(TValue);
                return base[id];
            }
        }
        
        public TValue Find(TKey id) => this[id];

        bool ISheet.Contains(object key) => Contains((TKey)key);
        void ISheet.Add(object value) => Add((TValue)value);

        protected override TKey GetKeyForItem(TValue item)
        {
            return item.Id;
        }

        public virtual void PostLoad(SheetConvertingContext context)
        {
            using (context.Logger.BeginScope(Name))
            {
                foreach (var row in Items)
                    row.PostLoad(context);
            }
        }

        public virtual void VerifyAssets(SheetConvertingContext context)
        {
            using (context.Logger.BeginScope(Name))
            {
                foreach (var row in Items)
                    row.VerifyAssets(context);
            }
        }
    }
    
    // Convenient shorthand
    public abstract class Sheet<T> : Sheet<string, T>
        where T : SheetRow<string>, new() {}
}
