﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cathei.BakingSheet.Raw
{
    public interface IRawSheetExporterPage
    {
        void SetCell(int col, int row, string data);
    }

    public static class RawSheetExporterPageExtensions
    {
        public static void Export(this IRawSheetExporterPage page, RawSheetConverter exporter, SheetConvertingContext context, Sheet sheet)
        {
            var sheetDict = sheet as IDictionary;

            if (sheetDict.Count == 0)
                return;

            var bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty;

            PropertyInfo[] sheetRowProperties = null, sheetElemProperties = null;

            int pageRow = 1;

            foreach (ISheetRow sheetRow in (sheet as IDictionary))
            {
                if (sheetRowProperties == null)
                {
                    sheetRowProperties = sheetRow.GetType()
                        .GetProperties(bindingFlags)
                        .OrderBy(x => x.Name == nameof(ISheetRow.Id))
                        .ToArray();
                }

                for (int i = 0; i < sheetRowProperties.Length; ++i)
                {
                    var prop = sheetRowProperties[i];
                    var value = prop.GetValue(sheetRow);
                    var cellValue = exporter.ValueToString(context, prop.PropertyType, value);

                    page.SetCell(i, pageRow, cellValue);

                }

                if (sheetRow is ISheetRowArray sheetRowArray)
                {
                    foreach (var sheetElem in sheetRowArray.Arr)
                    {
                        if (sheetElemProperties == null)
                        {
                            sheetElemProperties = sheetElem.GetType().GetProperties(bindingFlags);
                        }

                        for (int i = 0; i < sheetElemProperties.Length; ++i)
                        {
                            var prop = sheetElemProperties[i];
                            var value = prop.GetValue(sheetElem);
                            var cellValue = exporter.ValueToString(context, prop.PropertyType, value);

                            page.SetCell(sheetRowProperties.Length + i, pageRow, cellValue);
                        }

                        pageRow += 1;
                    }
                }

                pageRow += 1;
            }
        }
    }
}