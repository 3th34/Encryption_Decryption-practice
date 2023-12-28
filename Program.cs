using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
//invoked 
namespace AESEncryptionPractice;
/*AES - Advanced Encryption standard - symmetric key block cipher algorithm used for secure data transmission.
 encrypts data in fixed-size blocks of 128 bits, using a key size of 128,192 or 256 bits.

 how it works: by divding the plaintext into 128-bit blocks
and then performing a series of substituion and permutation operations on each block,
using a different key in each round.

 number of rounds performed depends on the key size,
with 10 rounds for 128-bit keys
     12 rounds for 192-bit keys
     14 rounds for 256-bit keys

AES is considered to be very secure and widely used in many apps such as encrypted file systems/SSL/TLS/VPN's.
Also used in many security protools and standards, such as WI-FI Protected Access(WPA) and the
Payment Card Industry Data Security Standard (PCI DSS)
*/
class Program
{
    static void Main(string[] args)
    {
        //initilizing timer to count effeciency of encryption time
        Stopwatch myTimer = new Stopwatch();
        myTimer.Start();



        string original = "secret message";
        byte[] encrypted;
        byte[] decrypted;

        Console.WriteLine($"encrypted: {original}\n");

        using (Aes aes = Aes.Create())
        {
            // using Aes algorithim within this block i Encrypted the String Value Below
            encrypted = EncryptStringToBytes(original, aes.Key, aes.IV);
            Console.WriteLine("Encrypted: {0}\n", Convert.ToBase64String(encrypted));

            //Decrypt the bytes
            decrypted = DecryptStringFromBytes(encrypted, aes.Key, aes.IV);
            //the value returned gives hexadicimal byte format
            Console.WriteLine("Decrypted hex values: {0}\n", BitConverter.ToString(decrypted));
            //to convert i need this so i run tgit his after to get back a string value
            Console.WriteLine("Decrypted original text: {0}\n", Encoding.UTF8.GetString(decrypted));




        }


        myTimer.Stop();
        Console.WriteLine("total run time of program: " + myTimer.ElapsedMilliseconds + "ms (milliseconds)");
        // stoping timer within main as the two methods called are run invoked and run within the main
        Console.Read();

    }
    static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
    {
        byte[] encrypted;

        //create an AES Object Algorithim with the specified type:key and  vector: IV.
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            //Creating a new MemoryStream object to contain the encrypted bytes.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //create a cryptoStream object to perform the encryption.
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    //Encrypting the plaintext here
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    //Completes the encryption process
                    //cryptoStream.FlushFinalBlock();

                    encrypted = memoryStream.ToArray();
                }
            }
        }
        return encrypted;
    }

    static byte[] DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
    {
        byte[] decrypted;

        //create an AES object with the specified key and IV
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            //Create a new memoryStream object to contain the decrypted bytes.
            using (MemoryStream memoryStream = new MemoryStream(cipherText))
            {
                //Create a CryptoStream object to perform the decryption.
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {   //Decrypt the cipher text.
                    using (MemoryStream decryptedStream = new MemoryStream())
                    {
                        cryptoStream.CopyTo(decryptedStream);
                        decrypted = decryptedStream.ToArray();
                    }
                }
            }
        }
        return decrypted;

    }
}

//a memoryStream is a stream that uses byte array as a backing store -
//provides an in-memory representation allowing you to read
//or write to a block of memory as if it were a stream.
//useful for when you want to treat a block of memory as a stream.



//a cryptoStream is a stream that performs cryptographic transformations on the data passing through it.
//consisting of three parts her CryptoStream(type:Stream, type:Transform , type:mode )
//Stream - indicates the underlying stream to which the cryptographic transformation are to be applied
//transform- the cryptographic transform to be applied(e.g. encryption/decryption algorithm type)
//mode- indicates whether the crypto stream will be used for reading/writing