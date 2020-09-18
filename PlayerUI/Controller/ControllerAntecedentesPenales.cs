using PlayerUI.ServicioAntecedentesPenalesSWJavita;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Google.Cloud.Firestore;


namespace PlayerUI.Controller
{
    class ControllerAntecedentesPenales
    {

        FirestoreDb dataBase;

        public void conexion()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"soaantecedentespenales2020a-firebase-adminsdk-y2zqm-8a83f1ec7c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            dataBase = FirestoreDb.Create("soaantecedentespenales2020a");
        }

        //----------------------------Ciudadano----------------------------
        public bool insert_ciudadano(string cedula, int tipoDocumento, string nombre, string apellido, DateTime fecha)
        {
            try
            {
                conexion();
                DocumentReference docref = dataBase.Collection("ciudadanos").Document(cedula);
                Dictionary<string, object> data = new Dictionary<string, object>()
                {
                    {"cedula", cedula},
                    {"tipoDocumento", tipoDocumento},
                    {"nombre", nombre},
                    {"apellido", apellido},
                    {"fecha", fecha}
                };
                docref.SetAsync(data);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
                return false;
            }

        }

        public async void update_ciudadano(string cedula, int tipoDocumento, string nombre, string apellido, DateTime fecha)
        {
            try
            {
                conexion();
                DocumentReference DOC = dataBase.Collection("ciudadanos").Document(cedula);
                Dictionary<string, object> data1 = new Dictionary<string, object>()
                {
                    {"cedula", cedula},
                    {"tipoDocumento", tipoDocumento},
                    {"nombre", nombre},
                    {"apellido", apellido},
                    {"fecha", fecha}
                };

                await DOC.UpdateAsync(data1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
        }

        public async void delete_ciudadano(string cedula)
        {
            try
            {
                conexion();
                DocumentReference docref = dataBase.Collection("ciudadanos").Document(cedula);
                await docref.DeleteAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
        }

        public async Task<List<Dictionary<string, object>>> ListAllCiudadanos()
        {
            conexion();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            try
            {
                CollectionReference collRef = dataBase.Collection("ciudadanos");
                QuerySnapshot snapshot = await collRef.GetSnapshotAsync();

                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    Dictionary<string, object> documentDictionary = document.ToDictionary();
                    list.Add(documentDictionary);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }

            return list;
        }

        public async Task<Dictionary<string, object>> FindClient(string cedula)
        {
            conexion();
            if (cedula.Equals(""))
            {
                throw new Exception("Por favor ingrese un número de identificación.");
            }
            else
            {
                DocumentReference docRef = dataBase.Collection("ciudadanos").Document(cedula);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> client = snapshot.ToDictionary();
                    return client;
                }
                else
                {
                    throw new Exception("El ciudadano con el número de identificación " + cedula +
                    " no ha sido encontrado");
                }

            }
        }

        private String urlGeneral = "http://192.168.16.13:7101/ServidorAntecedentesJDBCSW-ServidorAntecedentesSW-context-root/resources/model/";
        private static ControllerAntecedentesPenales controller;

        private ControllerAntecedentesPenales()
        {

        }

        public static ControllerAntecedentesPenales getInstance()
        {
            if (controller == null)
                controller = new ControllerAntecedentesPenales();
            return controller;
        }

        //----------------------------Delito----------------------------

        public async Task<List<Dictionary<string, object>>> ListAllDelitos()
        {
            conexion();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            try
            {
                CollectionReference collRef = dataBase.Collection("delitos");
                QuerySnapshot snapshot = await collRef.GetSnapshotAsync();

                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    Dictionary<string, object> documentDictionary = document.ToDictionary();
                    list.Add(documentDictionary);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }

            return list;
        }

        public List<delito> darDelitos()
        {
            delito[] delitosList = null;
            String urlRelativa = "darDelitos";
            var url = $"" + urlGeneral + urlRelativa;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return null;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            delitosList = JsonConvert.DeserializeObject<delito[]>(responseBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
            List<delito> delitos = new List<delito>();
            for (int i = 0; i < delitosList.Length; i++)
            {
                delitos.Add(delitosList[i]);
            }
            return delitos;
        }
        //----------------------------Antecedente----------------------------

        public async Task<List<Dictionary<string, object>>> ListAllAntecedentes()
        {
            conexion();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            try
            {
                CollectionReference collRef = dataBase.Collection("antecedentes");
                QuerySnapshot snapshot = await collRef.GetSnapshotAsync();

                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    Dictionary<string, object> documentDictionary = document.ToDictionary();
                    list.Add(documentDictionary);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }

            return list;
        }

        public bool insert_antecedente(int id, String ciudadanoDi, int delitoCodigo, String ciudad, DateTime fechaDelito, int sentencia, String estado)
        {
            try
            {
                conexion();
                DocumentReference docref = dataBase.Collection("antecedentes").Document(id.ToString());
                Dictionary<string, object> data = new Dictionary<string, object>()
                {
                    {"id",id},
                    {"ciudadanoDi", ciudadanoDi},
                    {"delitoCodigo", delitoCodigo},
                    {"ciudad", ciudad},
                    {"fechaDelito", fechaDelito},
                    {"sentencia", sentencia},
                    {"estado", estado}
                };
                docref.SetAsync(data);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
                return false;
            }
        }

        public async void update_antecedente(int id, String ciudadanoDi, int delitoCodigo, String ciudad, DateTime fechaDelito, int sentencia, String estado)
        {
            try
            {
                conexion();
                DocumentReference DOC = dataBase.Collection("antecedentes").Document(id.ToString());
                Dictionary<string, object> data1 = new Dictionary<string, object>()
                {
                    {"id",id},
                    {"ciudadanoDi", ciudadanoDi},
                    {"delitoCodigo", delitoCodigo},
                    {"ciudad", ciudad},
                    {"fechaDelito", fechaDelito},
                    {"sentencia", sentencia},
                    {"estado", estado}
                };

                await DOC.UpdateAsync(data1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
        }

        public async void delete_antecedente(int id)
        {
            try
            {
                conexion();
                DocumentReference docref = dataBase.Collection("antecedentes").Document(id.ToString());
                await docref.DeleteAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
        }

        public async Task<Dictionary<string, object>> FindId(int id)
        {
            conexion();
            if (id.Equals(""))
            {
                throw new Exception("Por favor ingrese un número de identificación.");
            }
            else
            {
                DocumentReference docRef = dataBase.Collection("antecedentes").Document(id.ToString());
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> client = snapshot.ToDictionary();
                    return client;
                }
                else
                {
                    throw new Exception("El ciudadano con el número de id " + id +
                    " no ha sido encontrado");
                }
            }
        }

        public int darSentenciaTotalPorCiudadano(String ciudadanoDi)
        {
            int resultado = 0;
            String urlRelativa = "darSentenciaTotal";
            var url = $"" + urlGeneral + urlRelativa + "?" + "ciudadanoDi=" + ciudadanoDi;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return 0;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            resultado = JsonConvert.DeserializeObject<int>(responseBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
            return resultado;
        }

        ////--------------------------Filtros------------------------------////

        public List<antecedente> darAntecedentesPorCiudadano(String ciudadanoDi)
        {
            //darAntecedentesCiudadano?ciudadanoDi=1111
            antecedente[] antecedentesList = null;
            String urlRelativa = "darAntecedentesCiudadano";
            var url = $"" + urlGeneral + urlRelativa + "?" + "ciudadanoDi=" + ciudadanoDi;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return null;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            antecedentesList = JsonConvert.DeserializeObject<antecedente[]>(responseBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
            List<antecedente> antecedentes = new List<antecedente>();
            for (int i = 0; i < antecedentesList.Length; i++)
            {
                antecedentes.Add(antecedentesList[i]);
            }
            return antecedentes;
        }

        public List<antecedente> darAntecedentesPorDelito(int delitoCodigo)
        {
            //darAntecedentesDelito?delitoCodigo=111
            antecedente[] antecedentesList = null;
            String urlRelativa = "darAntecedentesDelito";
            var url = $"" + urlGeneral + urlRelativa + "?" + "delitoCodigo=" + delitoCodigo;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return null;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            antecedentesList = JsonConvert.DeserializeObject<antecedente[]>(responseBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
            List<antecedente> antecedentes = new List<antecedente>();
            if (antecedentesList != null)
            {
                for (int i = 0; i < antecedentesList.Length; i++)
                {
                    antecedentes.Add(antecedentesList[i]);
                }
            }
            return antecedentes;
        }

        public List<antecedente> darAntecedentesPorCiudadanoYDelito(String ciudadanoDi, int delitoCodigo)
        {
            //darAntecedentesCiudadanoDelito?delitoCodigo=1111&ciudadanoDi=aaaa
            antecedente[] antecedentesList = null;
            String urlRelativa = "darAntecedentesCiudadanoDelito";
            var url = $"" + urlGeneral + urlRelativa + "?" + "delitoCodigo=" + delitoCodigo + "&ciudadanoDi=" + ciudadanoDi;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return null;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            antecedentesList = JsonConvert.DeserializeObject<antecedente[]>(responseBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
            List<antecedente> antecedentes = new List<antecedente>();
            for (int i = 0; i < antecedentesList.Length; i++)
            {
                antecedentes.Add(antecedentesList[i]);
            }
            return antecedentes;
        }
        //----------------------------TipoDocumento----------------------------
        public tipoDocumento darTipoDocumentoPorCodigo(int codigo)
        {
            tipoDocumento antecedente = null;
            String urlRelativa = "darTipoDocumento";
            var url = $"" + urlGeneral + urlRelativa + "?codigo=" + codigo;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return antecedente;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            antecedente = JsonConvert.DeserializeObject<tipoDocumento>(responseBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
            return antecedente;
        }

        public List<tipoDocumento> darTipoDocumentos()
        {
            tipoDocumento[] tipoDocumentosList = null;
            String urlRelativa = "darTipoDocumentos";
            var url = $"" + urlGeneral + urlRelativa;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return null;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            tipoDocumentosList = JsonConvert.DeserializeObject<tipoDocumento[]>(responseBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex);
            }
            List<tipoDocumento> tipoDocumentos = new List<tipoDocumento>();
            for (int i = 0; i < tipoDocumentosList.Length; i++)
            {
                tipoDocumentos.Add(tipoDocumentosList[i]);
            }
            return tipoDocumentos;
        }
        ////-------------------------Otros------------------------------/////

    }
}
