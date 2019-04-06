using System;
using System.IO;
using Newtonsoft.Json;
using SharpRSA;

namespace Rokkit
{
    /// <summary>
    /// The base Rokkit class, used to load and unload individual databases.
    /// </summary>
    public class Rokkit
    {
        public string Location;
        public bool AutoFlush = false;

        public Rokkit(string dbLocation, Key decryptKey=null)
        {
            //Checking if the file exists on the system.
            if (!File.Exists(dbLocation))
            {
                throw new Exception("Invalid file path given for database. Doesn't exist or no access.");
            }

            //File does exist, deserialize.
            if (decryptKey==null)
            {
                //nodecrypt deserialize
            }
            else
            {
                //decrypt deserialize
            }
        }

        public static bool CreateDB()
        {

        }

    }
}
