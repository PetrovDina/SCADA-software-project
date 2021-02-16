using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Security.Cryptography;

namespace CORE
{
    class DatabaseManagerService : IDatabaseManager, IAuthentication{

        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();


        public bool addTag(Tag t)
        {
            bool success = TagProcessing.AddTag(t);

            if (success)
            {
                Console.WriteLine("Successfully added new tag with id " + t.Id);
            }

            else
            {
                Console.WriteLine("Tag adding failed. Id: " + t.Id);

            }

            return success;
        }

        public bool addTagAlarm(string id, AlarmType alarmType, double limit, AlarmPriority priority)
        {
            bool success = TagProcessing.addTagAlarm(id, alarmType, limit, priority);

            if (success)
            {
                Console.WriteLine("Successfully added alarm for tag with id " + id);
            }

            else
            {
                Console.WriteLine("Adding alarm failef. Id: " + id);

            }
            return success;
        }


        public bool removeTag(string id)
        {
            bool success = TagProcessing.RemoveTag(id);

            if (success)
            {
                Console.WriteLine("Successfully removed new tag with id " + id );
            }

            else
            {
                Console.WriteLine("Tag removing failed. Id: " + id);

            }
            return success;
        }
        


        public bool setOutputTagValue(string id, double value)
        {
            bool success = TagProcessing.SetOutputTagValue(id, value);

            if (success)
            {
                Console.WriteLine("Successfully set value of output tag!");
            }

            else
            {
                Console.WriteLine("Output tag value change failed. Id: " + id);

            }
            return success;
        }

        public bool setTagScan(string id, bool scan)
        {
            bool success = TagProcessing.SetTagScan(id, scan);

            if (success)
            {
                Console.WriteLine("Successfully set scan of tag with id " + id + " to " + scan);
            }

            else
            {
                Console.WriteLine("Tag scan change failed. Id: " + id);

            }
            return success;
        }

        public string showOutputTagValues()
        {
            return TagProcessing.getOutputTagValues();
        }

        public bool deleteTagAlarm(string id)
        {
            bool success = TagProcessing.deleteAlarm(id);

            if (success)
            {
                Console.WriteLine("Successfully deleted alarm with id " + id);
            }
            else
            {
                Console.WriteLine("Failed to delete alarm. Id: " + id);

            }
            return success;
        }


        //-------------------------------------------------------------------------------------------------------


        public string Login(string username, string password)
        {
            using (var db = new UsersContext())
            {
                foreach (var user in db.Users)
                {
                    if (username == user.Username &&
                   ValidateEncryptedData(password, user.EncryptedPassword))
                    {
                        string token = GenerateToken(username);
                        authenticatedUsers.Add(token, user);
                        return token;
                    }
                }
            }
            return "Login failed";
        }

        public bool Logout(string token)
        {
            lock (authenticatedUsers)
            {
                return authenticatedUsers.Remove(token);

            }
        }

        public bool Registration(string username, string password)
        {

            User user;
            string encryptedPassword = EncryptData(password);

            if (UserDatabaseEmpty())
            {
                user = new User(username, encryptedPassword, UserType.ADMIN);
            }
            else
            {
                user = new User(username, encryptedPassword, UserType.REGULAR);

            }
            using (var db = new UsersContext())
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }


        private string EncryptData(string valueToEncrypt)
        {
            return EncryptValue(valueToEncrypt);
        }


        private string EncryptValue(string strValue)
        {
            string saltValue = GenerateSalt();
            byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);
            using (SHA256Managed sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedPassword);
                return $"{Convert.ToBase64String(hash)}:{saltValue}";
            }
        }

        private string GenerateSalt()
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] salt = new byte[32];
            crypto.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        private static bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedValue);
                string enteredValueToValidate = Convert.ToBase64String(hash);
                return encryptedDbValue.Equals(enteredValueToValidate);
            }
        }

        private string GenerateToken(string username)
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randVal = new byte[32];
            crypto.GetBytes(randVal);
            string randStr = Convert.ToBase64String(randVal);
            return username + randStr;
        }

        public bool UserDatabaseEmpty()
        {
            using (var db = new UsersContext())
            {
                return db.Users.ToList().Count == 0;
            }
        }

        public bool IsAdmin(string token)
        {
            lock (authenticatedUsers)
            {
                if (authenticatedUsers.ContainsKey(token))
                {
                    return authenticatedUsers[token].UserType == UserType.ADMIN;
                }
                return false;

            }
        }


    }
}
