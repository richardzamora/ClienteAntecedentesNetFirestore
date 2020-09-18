using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace PlayerUI.Controller
{
    public class generalController
    {
        private static FirestoreDb dataBase = null;

        public static FirestoreDb DataBase
        {
            get
            {
                if (dataBase == null)
                {
                    crateConnection();
                }
                return dataBase;
            }
        }

        private static void crateConnection()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + @"proyectosoa2020bankemb-firebase-adminsdk-cvrqv-5c56158e88.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            dataBase = FirestoreDb.Create("proyectosoa2020bankemb");
        }
    }


}

