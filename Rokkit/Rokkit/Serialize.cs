using System;
using Newtonsoft.Json;
using SharpRSA;
using System.IO;
using System.Text;

namespace Rokkit
{
    /// <summary>
    /// Internal class for serializing databases in and out of memory.
    /// </summary>
    internal class Serialize
    {
        /// <summary>
        /// Writes the given object to file, serialized, as plain text.
        /// </summary>
        /// <returns><c>true</c>, if file was successfully written, <c>false</c> otherwise.</returns>
        /// <param name="o">The object to write to file.</param>
        /// <param name="loc">Location (including file) to write to.</param>
        public static bool ToFile(object o, string loc)
        {
            try
            {
                File.WriteAllText(loc, JsonConvert.SerializeObject(o));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Override for ToFile to encrypt and save to file.
        /// </summary>
        /// <returns>The Key to decrypt the file with.</returns>
        /// <param name="o">The object to save.</param>
        /// <param name="loc">Location to save to.</param>
        /// <param name="keyBits">The bits (divisible by 8) of the key.</param>
        public static Key ToFile(object o, string loc, int keyBits)
        {
            //Checking keyBits are divisible by 8, to satisfy RSA.
            //If not, increment until divisible.
            while (keyBits % 8 != 0)
            {
                keyBits++;
            }

            //Generating a key for the database.
            KeyPair kp = RSA.GenerateKeyPair(keyBits);

            //Serializing the given class to string.
            string serialized = JsonConvert.SerializeObject(o);
            byte[] serializedBytes = Encoding.ASCII.GetBytes(serialized);

            //Encrypting the bytes with public key.
            byte[] encryptedBytes = RSA.EncryptBytes(serializedBytes, kp.public_);

            //Flushing to file.
            File.WriteAllBytes(loc, encryptedBytes);

            //Returning private key.
            return kp.private_;
        }

        /// <summary>
        /// Deserializes a given class from file.
        /// </summary>
        /// <returns>The deserialized class.</returns>
        /// <param name="loc">Location of the file.</param>
        /// <typeparam name="T">The serialized type.</typeparam>
        public static T FromFile<T>(string loc)
        {
            //Attempting to grab from file.
            return (T)JsonConvert.DeserializeObject(File.ReadAllText(loc));
        }

        /// <summary>
        /// Deserializes a given class from file, using a decryption key.
        /// </summary>
        /// <returns>The deserialized object.</returns>
        /// <param name="loc">Location of the file.</param>
        /// <param name="decryptKey">Decryption key.</param>
        /// <typeparam name="T">The type of the serialized object in the file.</typeparam>
        public static T FromFile<T>(string loc, Key decryptKey)
        {
            //Reading bytes from file.
            byte[] encryptedBytes = File.ReadAllBytes(loc);

            //Decrypting with given key.
            byte[] decryptedBytes = RSA.DecryptBytes(encryptedBytes, decryptKey);

            //Converting to text, deserializing.
            string serialized = Encoding.ASCII.GetString(decryptedBytes);
            return (T)JsonConvert.DeserializeObject(serialized);
        }
    }
}
