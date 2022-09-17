﻿// BakingSheet, Maxwell Keonwoo Kang <code.athei@gmail.com>, 2022

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Cathei.BakingSheet.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Object = UnityEngine.Object;

namespace Cathei.BakingSheet
{
    public class JsonSheetScriptableObjectConverter : JsonConverter<ISheetScriptableObjectReference>
    {
        private readonly List<Object> _references;

        public JsonSheetScriptableObjectConverter(List<UnityEngine.Object> references)
        {
            _references = references;
        }

        public override ISheetScriptableObjectReference ReadJson(
            JsonReader reader, Type objectType, ISheetScriptableObjectReference existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            existingValue ??= (ISheetScriptableObjectReference)Activator.CreateInstance(objectType);
            existingValue.Asset = serializer.Deserialize(reader, );
            return existingValue;
        }

        public override void WriteJson(
            JsonWriter writer, ISheetScriptableObjectReference value, JsonSerializer serializer)
        {
            int referenceIndex = -1;

            if (value.Asset != null)
            {
                referenceIndex = _references.IndexOf(value.Asset);

                if (referenceIndex < 0)
                {
                    referenceIndex = _references.Count;
                    _references.Add(value.Asset);
                }
            }

            writer.WriteValue(referenceIndex);
        }
    }
}
