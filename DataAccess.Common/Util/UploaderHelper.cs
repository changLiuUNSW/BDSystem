using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace DataAccess.Common.Util
{
    public static class UploaderHelper
    {
        public static MultipartFormDataStreamProvider GetMultipartProvider(string tempFolder=null)
        {
            if (tempFolder != null)
            {
                if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);
            }
            var root = tempFolder ?? Path.GetTempPath();
            return new MultipartFormDataStreamProvider(root);
        }
        // Extracts Request FormatData as a strongly typed model
        public static T GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFprmData = Uri.UnescapeDataString(result.FormData.GetValues(0).FirstOrDefault() ?? string.Empty);
                if (!string.IsNullOrEmpty(unescapedFprmData))
                {
                    return JsonConvert.DeserializeObject<T>(unescapedFprmData);
                }
            }
            return default(T);
        }

        public static string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }
        private static string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }

        public static string FilterInvalidFileName(this string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return new string(fileName.Where(l=>!invalidChars.Contains(l)).ToArray());
        }


        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(Path.GetFileNameWithoutExtension(fileName),@"_", DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName));
        }
    }
}