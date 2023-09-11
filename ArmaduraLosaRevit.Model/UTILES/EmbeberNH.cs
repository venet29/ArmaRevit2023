using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArmaduraLosaRevit.Model.UTILES
{
    internal class EmbeberNH
    {

        //Genera una key correcta a partir de un string
        public static string getKey(string key) => ObtenerNueva(key);
        private static string ObtenerNueva(string key)
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Derivar una clave de 128 bits utilizando PBKDF2
            int iterations = 10000; // Número de iteraciones para PBKDF2
            byte[] derivedKey = GenerateDerivedKey(key, salt, iterations, 16); // Generar una clave de 128 bits (16 bytes)

            // Convertir la clave derivada en una cadena base64 para almacenamiento o transmisión
            string claveNUeva = Convert.ToBase64String(derivedKey);
            return claveNUeva;
        }

        public static byte[] GenerateDerivedKey(string password, byte[] salt, int iterations, int keySize)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return deriveBytes.GetBytes(keySize);
            }
        }

        //******************************************************
        public static string Ejecutar(string plainText, string key)
        {
            try
            {
                //  string clave = "MiClaveSecreta"; // Clave original

                // Generar una sal aleatoria
                //string claveNueva = ObtenerNueva(key);

                byte[] encryptedBytes;
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                using (Aes aes = Aes.Create())
                {
                    // Configurar el algoritmo AES con la clave proporcionada
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // Generar un vector de inicialización aleatorio
                    aes.GenerateIV();
                    byte[] iv = aes.IV;

                    using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv))
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        // Escribir el vector de inicialización en el stream
                        memoryStream.Write(iv, 0, iv.Length);
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            // Encriptar los bytes del texto plano en el stream
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();
                        }

                        encryptedBytes = memoryStream.ToArray();
                    }
                }

                // Convertir los bytes encriptados en una cadena base64
                string encryptedText = Convert.ToBase64String(encryptedBytes);

                return encryptedText;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'EmbeberNH'. ex:{ex.Message}");
                return "";
            }

        }
        public static string DesEjecutar(string encryptedText, string key)
        {
            try
            {
          

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                using (Aes aes = Aes.Create())
                {
                    // Configurar el algoritmo AES con la clave proporcionada
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // Obtener el vector de inicialización del texto encriptado
                    byte[] iv = new byte[aes.BlockSize / 8];
                    Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);

                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv))
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                        {

   

                            // Desencriptar los bytes encriptados en el stream
                            cryptoStream.Write(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length);
                          //  cryptoStream.Close();
                            cryptoStream.FlushFinalBlock();
                        }

                        byte[] decryptedBytes = memoryStream.ToArray();
                        string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                        return decryptedText;
                    }
                }
            }
            catch (Exception)
            {

                return "";
            }
        }
      
    }
}



   public class AesExample
    {
    public AesExample()
    {
        
    }


    public bool nombreFuncion()
    {
        try
        {

            string original = "Here is some data to encrypt!";

            // Create a new instance of the Aes
            // class.  This generates a new key and initialization
            // vector (IV).
            using (Aes myAes = Aes.Create())
            {

                // Encrypt the string to an array of bytes.
                byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

                // Decrypt the bytes to a string.
                string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

                //Display the original data and the decrypted data.
                Console.WriteLine("Original:   {0}", original);
                Console.WriteLine("Round Trip: {0}", roundtrip);
            }
        }
        catch (Exception ex)
        {
           // Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
            return false;
        }
        return true;
    }

    public  byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
