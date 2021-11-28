
using UnityEngine;
using System;

namespace DataEntities
{
    [Serializable]
    public class UserConnect : ISerializationCallbackReceiver
    {
        public string sessionid;
        public string username;
        public string img;
        public string role;
        public string firstname;
        public int iduser;
        public DateTime Datelastconnection;

        // serialized backing field that will actually receive the json string
        [SerializeField] 
        private string datelastconnection;

        public void OnBeforeSerialize()
        {
            // or whatever format you want to use when serializing to string
            datelastconnection = Datelastconnection.ToString("O");
        }

        public void OnAfterDeserialize()
        {
            // after deserialization try to parse the date string into the DateTime field
            DateTime.TryParse(datelastconnection, out Datelastconnection);
        }
    }

}