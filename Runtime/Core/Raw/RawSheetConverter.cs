﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cathei.BakingSheet.Raw
{
    public abstract class RawSheetConverter : RawSheetImporter, ISheetConverter
    {
        public bool SplitHeader { get; set; }

        protected abstract Task<bool> SaveData();
        protected abstract IRawSheetExporterPage CreatePage(string sheetName);

        protected RawSheetConverter(TimeZoneInfo timeZoneInfo, bool splitHeader = false) : base(timeZoneInfo)
        {
            SplitHeader = splitHeader;
        }

        public async Task<bool> Export(SheetConvertingContext context)
        {
            foreach (var prop in context.Container.GetSheetProperties())
            {
                using (context.Logger.BeginScope(prop.Name))
                {
                    var sheet = prop.GetValue(context.Container) as ISheet;
                    if (sheet == null)
                        continue;

                    var page = CreatePage(sheet.Name);
                    page.Export(this, context, sheet);
                }
            }

            var success = await SaveData();

            if (!success)
            {
                context.Logger.LogError("Failed to save data");
                return false;
            }

            return true;
        }

        public virtual string ValueToString(Type type, object value)
        {
            if (value == null)
                return null;

            if (value is ISheetReference)
            {
                var reference = value as ISheetReference;
                return ValueToString(reference.IdType, reference.Id);
            }

            if (value is DateTime dt)
            {
                var local = TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo);
                return local.ToString(CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }
    }
}