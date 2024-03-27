using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nhom4_DoAnWeb_CHDT.Models
{
    public class MomoResult
    {
        public string partnerCode { get; set; } //Mã đối tác của Momo
        public string accessKey { get; set; } //Khóa truy cập của Momo
        public string requestId { get; set; } //Mã yêu cầu thanh toán của bạn
        public string amount { get; set; } //Số tiền thanh toán
        public string orderId { get; set; } //Mã đơn hàng của bạn
        public string orderInfo { get; set; } //Thông tin đơn hàng của bạn
        public string orderType { get; set; } //Loại đơn hàng của bạn
        public string transId { get; set; } //Mã giao dịch của Momo
        public string message { get; set; } //Thông điệp từ Momo
        public string localMessage { get; set; } //Thông điệp từ máy chủ của bạn
        public string responseTime { get; set; } //Thời gian phản hồi của Momo
        public string errorCode { get; set; } // Mã lỗi(nếu có)
        public string payType { get; set; } //Loại thanh toán của bạn
        public string extraData { get; set; } //Dữ liệu bổ sung (nếu có)
        public string signature { get; set; } //Chữ ký số để xác thực kết quả thanh toán
    }
}
