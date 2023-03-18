using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace PttCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            // 設定要爬取的頁面 URL
            string url = "https://www.ptt.cc/bbs/Gossiping/index.html";

            // 設定要寫入的檔案路徑
            string filePath = @"\\Mac\Home\Desktop\title.txt";

            // 設定是否需要通過18歲限制
            bool isAdultContent = true;

            // 設定瀏覽器標頭，模擬使用者行為
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

            // 設定 cookies，模擬登入行為
            string cookies = "over18=1";

            // 建立 web 客戶端
            WebClient client = new WebClient();

            // 設定瀏覽器標頭和 cookies
            client.Headers.Add("User-Agent", userAgent);
            client.Headers.Add("cookie", cookies);

            // 下載頁面內容
            string html = client.DownloadString(url);

            // 解析 HTML 內容
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 找到所有的文章標題
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='title']/a");

            // 逐一處理每個標題，並寫入檔案中
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    foreach (var node in nodes)
                    {
                        if (isAdultContent && node.InnerHtml.Contains("18禁"))
                        {
                            continue;
                        }
                        // 取得標題
                        string title = node.InnerText.Trim();
                        // 寫入檔案
                        writer.WriteLine(title);
                    }
                }
            }
            catch (Exception)
            {
                // 若寫入檔案失敗，則在桌面創建新的記事本並寫入檔案內容
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                filePath = Path.Combine(desktopPath, "title.txt");

                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    foreach (var node in nodes)
                    {
                        if (isAdultContent && node.InnerHtml.Contains("18禁"))
                        {
                            continue;
                        }
                        string title = node.InnerText.Trim();
                        writer.WriteLine(title);
                    }
                }
            }
        }
    }
}