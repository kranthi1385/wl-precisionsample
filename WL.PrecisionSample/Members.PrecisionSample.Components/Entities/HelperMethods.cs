using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class HelperMethods
    {
        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="applicationKey"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public String DecryptIt(String s, String applicationKey, String iv)
        {

            String result;
            byte[] key = str2pack(applicationKey);
            byte[] IV = Convert.FromBase64String(System.Web.HttpUtility.UrlDecode(iv));
            byte[] payload = Convert.FromBase64String(System.Web.HttpUtility.UrlDecode(s));

            RijndaelManaged rijn = new RijndaelManaged();
            rijn.Mode = CipherMode.CBC;
            rijn.Padding = PaddingMode.Zeros;
            using (MemoryStream msDecrypt = new MemoryStream(payload))
            {
                using (ICryptoTransform decryptor = rijn.CreateDecryptor(key, IV))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swDecrypt = new StreamReader(csDecrypt))
                        {
                            result = swDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            rijn.Clear();
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public byte[] str2pack(String str)
        {
            int nibbleshift = 4;
            int position = 0;
            int len = str.Length / 2 + str.Length % 2;

            byte[] output = new byte[len];
            foreach (char v in str.ToCharArray())
            {
                byte n = (byte)v;
                if (n >= '0' && n <= '9')
                {
                    n -= (byte)'0';
                }
                else if (n >= 'A' && n <= 'F')
                {
                    n -= ('A' - 10);
                }
                else if (n >= 'a' && n <= 'f')
                {
                    n -= ('a' - 10);
                }
                else
                {
                    continue;
                }

                output[position] |= (byte)(n << nibbleshift);

                if (nibbleshift == 0)
                {
                    position++;
                }
                nibbleshift = (nibbleshift + 4) & 7;
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getInitVector()
        {
            RijndaelManaged rijn = new RijndaelManaged();
            rijn.GenerateIV();
            string iv = System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(rijn.IV));
            return iv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="applicationKey"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public string EncryptIt(string payload, string applicationKey, string iv)
        {
            String result;
            byte[] key = str2pack(applicationKey);
            byte[] IV = Convert.FromBase64String(System.Web.HttpUtility.UrlDecode(iv));

            RijndaelManaged rijn = new RijndaelManaged();

            var encryptor = rijn.CreateEncryptor(key, IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(payload);
            }

            return System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(msEncrypt.ToArray()));

        }

        #endregion
    }
}
