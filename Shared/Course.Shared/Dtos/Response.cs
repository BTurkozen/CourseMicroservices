using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Course.Shared.Dtos
{
    public class Response<T>
    {
        /// <summary>
        /// Response içerisinde gönderilecek data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Response Durum Kodu
        /// <para>Bu Kod'u Proje içerisinde kendim kullanmak istiyorum.</para>
        /// <para>Response Body'de zaten StatusCode Dönülüyor. Biz tekrar dönmemek için burada Ignore işlemine tabi tutuyoruz.</para>
        /// <para>Nesneler dışarıdan değiştirilmeye kapalı.</para>
        /// </summary>
        [JsonIgnore]
        public int StatusCode { get; set; }

        /// <summary>
        /// Başarılı durumu.
        /// <para>Nesneler dışarıdan değiştirilmeye kapalı.</para>
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessful { get; set; }

        public List<string> Errors { get; set; }

        // Static Olarak Function'lar oluşturulacak.
        // Bu static nesneler nesne oluşturma işlemlerinde kolaylık sağlar.
        // Static Methodlarla beraber Geriye yeni bir nesne dönüyorsanız bunlara "Static Factory method" denir.

        /// <summary>
        /// Response Başarılı Durumu
        /// </summary>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        /// <summary>
        /// Response Başarılı Durumu (Success Overload method'u olarak oluşturuldu. Sadece StatusCode alıyor.)
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        /// <summary>
        /// Response Başarısız Durumu (Birden çok hata için)
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T> { StatusCode = statusCode, Errors = errors, IsSuccessful = false };
        }

        /// <summary>
        /// Response Başarısız Durumu (Tek hata için Overload method)
        /// </summary>
        /// <param name="error"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
