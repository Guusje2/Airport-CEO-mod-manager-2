using System.Collections;
using System.Collections.Generic;
using UnityEngine;      
using Oracle.ManagedDataAccess.Client;

public class OracleDBController : MonoBehaviour
{
    OracleCommand cmd;
    string conString;
    // Start is called before the first frame update
    void Start()
    {
        //Enter user id and password, such as ADMIN user	
        conString = "User Id=ADMIN;Password=4vHhjPm2PM26FLr;" +

        //Enter net service name for data source value
        "Data Source=ACEOMMDB_HIGH";
        GetData();
    }

    public void GetData() {
        using (OracleConnection con = new OracleConnection(conString)) 
        {
            con.Open();

            cmd = con.CreateCommand();
            Debug.Log("connected?");
            OracleParameter param = cmd.Parameters.Add("BlobParameter", OracleDbType.Blob);
            param.Direction = System.Data.ParameterDirection.Output;
            

            cmd.CommandText = "SELECT * FROM BUSINESSES";
            OracleDataReader reader = cmd.ExecuteReader();

            Debug.Log(reader.GetString(0));
            reader.Close();
        }
    }
}
