using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OnlineMallManagement.Areas.Admin.Data
{
    public class AccountModel
    {

        public AccountModel()
        {
            
        }

        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

    }

    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserName cannot be empty!")]
        public string username { set; get; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Passwords cannot be empty!")]
        public string password { set; get; }
        public bool RememberMe { set; get; }
    }
}