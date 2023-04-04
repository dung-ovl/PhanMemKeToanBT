using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Ke_Toan.API
{
    public class CRUD
    {
        private static readonly Uri API_Connection = new Uri("https://ketoan-api.onrender.com");
        public static string GeneratePrimaryKey(string lastKey)
        {
            string key = lastKey;
            string tableName = "";
            string num = "";
            for (int i = 0; i < key.Length; i++)
                if (char.IsDigit(key[i]))
                {
                    tableName = key.Substring(0, i);
                    num = key.Substring(i);
                    break;
                }
            int n = int.Parse(num) + 1;
            if (n < 10)
                return tableName + "00" + n.ToString();
            if (n < 100)
                return tableName + "0" + n.ToString();
            return tableName + n.ToString();
        }
        public static string GetJsonData(string tableName)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = API_Connection;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync("/" + tableName).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                        return "[]";
                    }
                }
                catch
                {
                    return "[]";
                }
            }
        }
        public static string GetJoinTableData(string tableName)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = API_Connection;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync("/" + tableName + "/join").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                        return "[]";
                    }
                }
                catch
                {
                    return "[]";
                }
            }
        }
        public static string GetDataByColumnName(string tableName, string columnName)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = API_Connection;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(tableName + "/" + columnName).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    return null;
                }
            }
        }

        public static bool InsertData(string tableName, object s)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(s);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                client.BaseAddress = API_Connection;
                var response = client.PostAsync("/" + tableName, stringContent).Result;
                return (response.IsSuccessStatusCode);
            }
        }
        public static bool UpdateData(string tableName, object s)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(s);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                client.BaseAddress = API_Connection;
                var response = client.PutAsync("/" + tableName, stringContent).Result;
                return (response.IsSuccessStatusCode);
            }
        }
        public static bool DeleteData(string tableName, string primaryKey)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = API_Connection;
                var response = client.DeleteAsync(tableName + "/" + primaryKey).Result;
                return (response.IsSuccessStatusCode);
            }
        }
        public static bool UpdateTongTien(string tableName, string SoPhieu, object s)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(s);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                client.BaseAddress = API_Connection;
                var response = client.PutAsync("/" + tableName + "/" + SoPhieu, stringContent).Result;
                return (response.IsSuccessStatusCode);
            }
        }
        public static bool TinhGiaXuatKho(DateTime start, DateTime end)
        {
            using (var client = new HttpClient())
            {
                KhoangThoiGian s = new KhoangThoiGian
                {
                    NgayBD = start,
                    NgayKT = end,
                };
                var json = JsonConvert.SerializeObject(s);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                client.BaseAddress = API_Connection;
                var response = client.PutAsync("/vattu/tinhgiaxuat" , stringContent).Result;
                return (response.IsSuccessStatusCode);
            }
        }
    }
    public class KhoangThoiGian
    {
        public DateTime NgayBD { get; set; }
        public DateTime NgayKT { get; set; }

    }
}
