using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;

namespace JwtBearerAuth.Utility
{
    /// <summary>
    /// RSA 帮助类
    /// </summary>
    public class RsaHelper
    {
        private static string Separator = Path.DirectorySeparatorChar.ToString();
        public static string DefaultPublicKeyPath { get; set; } = $"{Directory.GetCurrentDirectory()}{Separator}public-key.json";
        public static string DefaultPrivateKeyPath { get; set; } = $"{Directory.GetCurrentDirectory()}{Separator}private-key.json";


        /// <summary>
        /// 获取Rsa key
        /// </summary>
        /// <param name="isPublic">true：公钥，false：私钥</param>
        /// <returns></returns>
        public static RSAParameters GetRsaKey(bool isPublic)
        {
            RSAParameters publicKey = new RSAParameters();
            RSAParameters privateKey = new RSAParameters();
            if (!File.Exists(DefaultPublicKeyPath) || !File.Exists(DefaultPrivateKeyPath))
            {
                // 必须为2048，因为生成签名的时候用的是SecurityAlgorithms.RsaSsaPssSha256
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048)) 
                {
                    publicKey = rsa.ExportParameters(false);
                    privateKey = rsa.ExportParameters(true);
                    File.WriteAllText(DefaultPublicKeyPath, JsonConvert.SerializeObject(publicKey));
                    File.WriteAllText(DefaultPrivateKeyPath, JsonConvert.SerializeObject(privateKey));
                }
            }
            else
            {
                publicKey = JsonConvert.DeserializeObject<RSAParameters>(File.ReadAllText(DefaultPublicKeyPath));
                privateKey = JsonConvert.DeserializeObject<RSAParameters>(File.ReadAllText(DefaultPrivateKeyPath));
            }
            return isPublic ? publicKey : privateKey;
        }

    }
}
