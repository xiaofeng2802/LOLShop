using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using ThoConShop.DataSeedWork.Enum;

namespace ThoConShop.Web.GameBank
{
    public class GameBankAPI
    {


        //this is your proxy username,password,ip
        private string proxy_ip = "http://27.0.0.1/";//your proxy server
        private string username = "root"; //proxy username
        private string password = "";  //proxy password 

        //ULR GameBank
        private string post_url = "http://sv.gamebank.vn/api/card";


        //Rate : Tỉ giá nạp tiền. Mệnh_giá:Số_tiền . bắt buộc : 9 phần tử.
        private string conversion_rate = "10000:10000; 20000:20000; 30000:30000; 50000:500; 100000:100000; 200000:200000; 300000:300000; 500000:500000; 1000000:1000000";


        public string VerifiedCard(string seri, string pin, TypeCard type, string note, out int price)
        {
            //string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int status = 9999, coins;
            string msg = "";
            string POST_DATA = "merchant_id=" + ConfigurationManager.AppSettings["Merchant_ID"] + "&card_type=" + (int)type + "&seri=" + seri + "&pin=" + pin + "&note=" + note;

            //sent data to remote server
            string responseString = APIGamebank(POST_DATA, post_url, ConfigurationManager.AppSettings["API_User"], ConfigurationManager.AppSettings["API_Password"]);
            //using Newtonsoft.Json
            var results = JsonConvert.DeserializeObject<dynamic>(responseString);
            int code = Convert.ToInt32(results.code);
            if (code == 0)
            {
                status = 0;
                price = Convert.ToInt32(results.info_card);
                //coins = ConversionRate(conversion_rate, price);
                msg = "Nap the " + (int)type + " " + price + " thanh cong";
                //insertMySQL(datetime, seri, pin, type, price, coins, msg, status);
                return "";
            }
            price = 0;
            return ResultsNotication(code);
        }

        private string APIGamebank(String postData, String url, String user, String pwd)
        {
            try
            {
                // Create a new request to the mentioned URL.    
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);
                //if (proxy_setting == true)
                //{
                //    connectProxy(httpWReq);
                //}
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postData);
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                httpWReq.Credentials = new NetworkCredential(user, pwd);
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private void connectProxy(HttpWebRequest httpWReq)
        {
            // Obtain the 'Proxy' of the  Default browser.  
            IWebProxy proxy = httpWReq.Proxy;
            WebProxy myProxy = new WebProxy();
            Uri newUri = new Uri(proxy_ip);
            // Associate the newUri object to 'myProxy' object so that new myProxy settings can be set.
            myProxy.Address = newUri;
            // Create a NetworkCredential object and associate it with the  
            // Proxy property of request object.
            myProxy.Credentials = new NetworkCredential(username, password);
            httpWReq.Proxy = myProxy;
        }

        public string ResultsNotication(int code)
        {
            switch (code)
            {
                case 1:
                    return "Merchant_id not found";
                    
                case 2:
                    return "Unauthorized";
                    
                case 3:
                    return "Mã thẻ cào hoặc seri không chính xác";
                    
                case 4:
                    return "Thẻ đã sử dụng";
                    
                case 5:
                    return "Bạn phải nhập seri thẻ";
                    
                case 6:
                    return "Thẻ đã gửi sang hệ thống nhưng bị trễ";
                    
                case 7:
                    return "Hệ thống nạp thẻ đang bảo trì";
                    
                case 8:
                    return "Có lỗi xảy ra trong quá trình nạp thẻ.Liên hệ Gamebank";
                    
                case 9:
                    return "Thẻ không sử dụng được";
                    
                case 10:
                    return "Nhập sai định dạng thẻ";
                    
                case 11:
                    return "Nhập sai quá 3 lần";
                    
                case 12:
                    return "Lỗi hệ thống.Liên hệ Gamebank";
                    
                case 13:
                    return "IP không được phép truy cập sau 5 phút";
                    
                case 14:
                    return "Tên đăng nhập không đúng";
                    
                case 15:
                    return "Loại thẻ không đúng";
                    
                case 16:
                    return "Mã thẻ viettel phải có 13 chữ số";
                    
                case 17:
                    return "Seri viettel phải có 11 chữ số";
                    
                case 18:
                    return "Mã thẻ mobiphone phải có 12 hoặc 14 số";
                    
                case 19:
                    return "Seri mobiphone phải là 1 dãy số";
                    
                case 20:
                    return "Mã thẻ vinaphone phải có 12 hoặc 14 số";
                    
                case 21:
                    return "Mã thẻ gate có 10 số và seri có 10 ký tự gồm chữ và số";
                    
                case 22:
                    return "Thẻ đã nạp sang hệ thống, không nạp nữa.";
                    
                case 23:
                    return "Sai thông tin partner";
                    
                case 24:
                    return "Chưa nhận được kết quả trả về từ nhà cung cấp mã thẻ";
                    
                case 25:
                    return "Dữ liệu truyền vào không đúng";
                    
                case 26:
                    return "Nhà cung cấp không tồn tại";
                    
                case 27:
                    return "Sai IP";
                    
                case 28:
                    return "Sai session";
                    
                case 29:
                    return "Session hết hạn";
                    
                case 30:
                    return "Hệ thống bận, nạp lại sau ít phút";
                    
                case 31:
                    return "Tạm thời khóa kênh nạp VMS do quá tải";
                    
                case 32:
                    return "Trùng giao dịch, nạp lại sau ít phút";
                    
                case 33:
                    return "Seri hoặc mã thẻ không đúng";
                    
                case 34:
                    return "Card tạm thời bị khóa trong 24h";
                    
                case 35:
                    return "Mã thẻ và Mã seri phải có 12 ký tự gồm chữ và số";
                    
                case 36:
                    return "Tài khoản của bạn chưa thiết lập IP hoặc chưa được duyệt";
                    
                case 37:
                    return "IP hiện tại không thuộc sở hữu hoặc trong danh sách cho phép của bạn";
                    
                case 38:
                    return "Thẻ VTC không còn được hỗ trợ";
                    
                case 39:
                    return "Thẻ đang bảo trì!";
                    
                case 40:
                    return "Tài khoản của bạn đang bị khóa!";
                    
                case -1:
                    return "Tài khoản của bạn đang bị khóa!";
                    
                case -2:
                    return "Thẻ đã bị khóa";
                    
                case -3:
                    return "Thẻ hết hạn sử dụng";
                    
                case -4:
                    return "Thẻ chưa kích hoạt";
                    
                case -5:
                    return "Giao dịch không hợp lệ";
                    
                case -6:
                    return "Mã thẻ và số Serial không khớp";
                    
                case -8:
                    return "Cảnh báo số lần giao dịch lỗi của một tài khoản";
                    
                case -9:
                    return "Thẻ thử quá số lần cho phép";
                    
                case -10:
                    return "Mã seri không hợp lệ";
                    
                case -11:
                    return "Mã thẻ không hợp lệ";
                    
                case -12:
                    return "Thẻ không tồn tại hoặc đã sử dụng";
                    
                case -13:
                    return "Sai cấu trúc Description";
                    
                case -14:
                    return "Mã dịch vụ không tồn tại";
                    
                case -15:
                    return "Thiếu thông tin khách hàng";
                    
                case -16:
                    return "Mã giao dịch không hợp lệ";
                    
                case -90:
                    return "Sai tên hàm";
                    
                case -98:
                    return "Giao dịch thất bại do Lỗi hệ thống";
                    
                case -99:
                    return "Giao dịch thất bại do Lỗi hệ thống";
                    
                case -999:
                    return "Hệ thống tạm thời bận";
                    
                case -100:
                    return "Giao dịch nghi vấn";
                    
                default:
                    return "Không rõ nguyên nhân";
                    
            }
        }

    }
}