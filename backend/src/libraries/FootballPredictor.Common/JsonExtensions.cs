using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace FootballPredictor.Common
{
    /// <summary>
    /// JSON Extensions
    /// </summary>
    public static partial class Extensions
    {
        #region JSON Extensions

        /// <summary>
        /// Converts object to JSON string
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns></returns>
        public static string ToJson(this object obj) =>
            JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

        /// <summary>
        /// Converts object to JSON string
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string ToJson(this object obj, Formatting format) => ToJson(obj, format, false);

        /// <summary>
        /// Converts object to JSON string with an option to ignore errors
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="format">The format.</param>
        /// <param name="ignoreErrors">if set to <c>true</c> [ignore errors].</param>
        /// <returns></returns>
        public static string ToJson(this object obj, Formatting format, bool ignoreErrors)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = format
            };

            if (ignoreErrors)
            {
                settings.Error += new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>((s, e) =>
                {
                    e.ErrorContext.Handled = true;
                });
            }

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// Attempts to deserialize a JSON string into T.  If it can't be deserialized, returns null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static T FromJsonOrNull<T>(this string val)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(val)) return default;

                return JsonConvert.DeserializeObject<T>(val);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to deserialize to {typeof(T).Name}. {ex}");
                return default(T);
            }
        }

        /// <summary>
        /// Attempts to deserialize a JSON string into either a <see cref="ExpandoObject" /> or a list of <see cref="ExpandoObject" />.  If it can't be deserialized, return null
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static object FromJsonDynamicOrNull(this string val)
        {
            try
            {
                return val.FromJsonDynamic();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to deserialize to dynamic. {ex}");
                return null;
            }
        }

        /// <summary>
        /// Attempts to deserialize a JSON string into either a <see cref="ExpandoObject" /> or a list of <see cref="ExpandoObject" />. If it can't be deserialized, throws an exception
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static object FromJsonDynamic(this string val)
        {
            var converter = new ExpandoObjectConverter();
            object dynamicObject = null;

            // keep track of which exception most applies. 
            Exception singleObjectException = null;
            Exception arrayObjectException = null;

            try
            {
                // first try to deserialize as straight ExpandoObject
                dynamicObject = JsonConvert.DeserializeObject<ExpandoObject>(val, converter);
            }
            catch (Exception firstException)
            {
                try
                {
                    singleObjectException = firstException;
                    dynamicObject = JsonConvert.DeserializeObject<List<ExpandoObject>>(val, converter);

                }
                catch (Exception secondException)
                {
                    try
                    {
                        arrayObjectException = secondException;

                        // if it didn't deserialize as a List of ExpandoObject, try it as a List of plain objects
                        dynamicObject = JsonConvert.DeserializeObject<List<object>>(val, converter);
                    }
                    catch
                    {
                        // if both the attempt to deserialize an object and an object list fail, it probably isn't valid JSON, so throw the singleObjectException
                        if (singleObjectException != null)
                        {
                            throw singleObjectException;
                        }
                        else
                        {
                            throw arrayObjectException;
                        }
                    }
                }
            }

            return dynamicObject;
        }

        #endregion

        #region JObject extension methods

        /// <summary>
        /// Converts a jObject to a dictionary
        /// </summary>
        /// <param name="jobject">The JObject.</param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this JObject jobject)
        {
            var result = jobject.ToObject<Dictionary<string, object>>();

            var valueKeys = result
                .Where(r => r.Value != null && r.Value.GetType() == typeof(JObject))
                .Select(r => r.Key)
                .ToList();

            var arrayKeys = result
                .Where(r => r.Value != null && r.Value.GetType() == typeof(JArray))
                .Select(r => r.Key)
                .ToList();

            arrayKeys.ForEach(k => result[k] = ((JArray)result[k]).ToObjectArray());
            valueKeys.ForEach(k => result[k] = ToDictionary(result[k] as JObject));

            return result;
        }

        /// <summary>
        /// Converts a JArray to a Object array
        /// </summary>
        /// <param name="jarray">The JArray.</param>
        /// <returns></returns>
        public static object[] ToObjectArray(this JArray jarray)
        {
            var valueList = new List<object>();

            for (var i = 0; i < jarray.Count; i++)
            {
                var obj = jarray[i];
                if (obj.GetType() == typeof(JObject))
                {
                    valueList.Add(((JObject)obj).ToDictionary());
                }

                if (obj.GetType() == typeof(JValue))
                {
                    valueList.Add(((JValue)obj).Value);
                }
            }

            return valueList.ToArray();
        }

        #endregion
    }
}
