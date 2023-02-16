using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class zigSimData 
{
    public string timestamp;
    public dataSensors sensordata;

    public static zigSimData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<zigSimData>(jsonString);
    }
}

[System.Serializable]
public class dataSensors
{
    public dataGyro gravity;
}

[System.Serializable]
public class dataGyro
{
    public float x;
    public float y;
    public float z;
}


//{ "device":{ "os":"Android","osversion":"7.1.2","name":"Redmi 4X","uuid":"b6c396d4-96df-43f6-9323-04ee09b561e5","displaywidth":720,"displayheight":1280},"timestamp":"2023\/02\/07 14:26:23.073","sensordata":{ "gyro":{ "x":-0.0594635009765625,"y":0.0280303955078125,"z":0.0023345947265625} } }