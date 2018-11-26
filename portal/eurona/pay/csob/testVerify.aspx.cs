using Eurona.pay.csob.utils;
using Eurona.PAY.CSOB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.pay.csob {
    public partial class testVerify : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            //Test Response
            /*
            payId=925a8d92cda91DK
            dttm=20181126080434
            resultCode=0
            resultMessage=OK
            paymentStatus=7
            signature=t6Z4C3VrIWhiVRlez2%2FYAtc2maRh8nh2RZR7%2BaMBDcAcSU2Uw%2B7m45lLv7J4V40I1a9lU4r1hn5ziXRRP20GWivUDQuLKSzJzcp2B4eUUhgF%2FR2fi9gx6oC6wTsS6plxFjUMAlNHhqaFS1Sz%2F2wadqhQksH5w2y1L54ydQ4VE6OCAL%2BijDLCtne8g4torq%2BVgrkPu1KvwkggWz8h7Lrcvm%2BnD8duS1v%2FpyJfq4aVLz1MUOPH8G%2FDs9rWYggouqxb3cmQEi1LwUQs4pHejt7gvcFNXtQ%2FfiuKHKaLM%2FscpEx%2FFXdZesDrGiBXfotA%2BQbUXTd41pBhWucyGlw8T3SjJg%3D%3D&authCode=200153&merchantData=MzY5
            */
            ////payId|dttm|resultCode|resultMessage|paymentStatus
            //string data2Verify = "5e30619e82b47DK|20181124173302|0|OK|1";
            //string signature = "nbKWhVx2rgrd2c/CLTZooZmpg23WjvUzRT9BVzsQ0U+mjfcMVp9Wror+ucSTyve0zEzhawOEbYGPiEanNU5XFzN8/kMnXSHzO3i6olaMaxfIGjBu70HiFl4T7hv7QCALggcfrJMERPwFU1XrmcGpUQq8tzB0uLt6CUCIOz7fd7FQe92sk046slzc9b/ccuMQfmM5uil/qkj0hbIHq9mttG4PTRcORqXtHUPkSVsiZZDSyD7WdiwbJbirmD4IsqW3wDQECl2xUOCLZdPDM/5rUbfelZCrX/TToe+5VoEDwRXnD7d1fW83Q1oSyJhsShov1Wm3mqgUNVLtfDbq6FvsWA==";

            //bool verification = Digest.Verify(CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PublicKeyPath", this), data2Verify, signature);
            //if (verification == false) {
            //    throw new InvalidOperationException("PaymentInitResponse verification failed!");
            //}

            //payId|dttm|resultCode|resultMessage|paymentStatus|authCode|merchantData
            string data2Verify = "925a8d92cda91DK|20181126080434|0|OK|7|200153|MzY5";
            //string signature = "YVzunST6PmQHpuIU+NJ5FJnWQEYW43kx96wh8mAPKZWTy8av4rgiJgvd51cYhyN/htDi6xUmcdvAsSHuTgJQ4XXo7RxMvGnKeQEKb3F9imvAr942gbxzt9zyR47rpg06/W4bKOzGu6gbc9fc4IAwLA9GBJKQZeG2l4+VmI/86Ckb4GdoZE9fM/ApioEmQd5bZJAzoE4LNQ3A2gceZoAIjH5yyZmKpdhJIvZns4UxBWasdru4kNBo8n3+i6mrgwtZNojIzbCLtmE2md6YYyUVuSVPigQ3SMZjobbB8p45VsEbhO8gHxWjoT9QebPpWI0y5po+rBswyLhx3LhrEsXjeQ==";

            //signature=bW9YD3PJjyOFeWXLXudHgmWyy6eALFzFyG7JHKQlfXds2%2F1aD9eukl%2BOMX6ZRN6CnPxNZ%2FS5BIBPqZ57LwUtS7JUe%2F%2FHzirC%2Bb%2F%2FyNEstvLA%2FCN2ZXJVeGY8GzWCT5yRDxKsWTCqV1KS98qRoMkecEOvh%2BVE8EkR%2BKfpTVJHA7H%2BlSzkNmBHtWkz2rydnycsfJBquSkvbCLdCGrI%2F1mN%2Fx6K4TPTL91CG877PTiASuBj3rxmK5vfBkHVt9otw1SBmAlx%2B0WJxncC1x2BC4AsSdsWMZOmL%2B8N%2FHgbV5i4oVqPVPMSYo9OrBp9pTA2lwtjprstERWld2JkiNiEknIsiA%3D%3D

            string urlSignature = "bW9YD3PJjyOFeWXLXudHgmWyy6eALFzFyG7JHKQlfXds2%2F1aD9eukl%2BOMX6ZRN6CnPxNZ%2FS5BIBPqZ57LwUtS7JUe%2F%2FHzirC%2Bb%2F%2FyNEstvLA%2FCN2ZXJVeGY8GzWCT5yRDxKsWTCqV1KS98qRoMkecEOvh%2BVE8EkR%2BKfpTVJHA7H%2BlSzkNmBHtWkz2rydnycsfJBquSkvbCLdCGrI%2F1mN%2Fx6K4TPTL91CG877PTiASuBj3rxmK5vfBkHVt9otw1SBmAlx%2B0WJxncC1x2BC4AsSdsWMZOmL%2B8N%2FHgbV5i4oVqPVPMSYo9OrBp9pTA2lwtjprstERWld2JkiNiEknIsiA%3D%3D";
            string signature = HttpUtility.UrlDecode(urlSignature, Encoding.UTF8);

            bool verification = Crypto.Verify(CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PublicKeyPath", this), data2Verify, signature);
            if (verification == false) {
                throw new InvalidOperationException("PaymentInitResponse verification failed!");
            }

        }
    }
}