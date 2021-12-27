using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(menuName = "ScriptableObjects/NewGun", order = 999)]
public class GunItem : ScriptableObject
{
    public string GunName;
    [Tooltip("shots per second")]
    public float RateShot;
    public Color GunColor;
    public int MaxAmmo;
}

public static class GunSerializer
{
    public static byte[] writerData;
    public static void WriteGun(this NetworkWriter writer, GunItem gun)
    {
        writer.WriteString(gun.name);
        writerData = writer.ToArray();
    }
    public static void WriteGun(GunItem gun)
    {
        NetworkWriter writer = new NetworkWriter();
        WriteGun(writer, gun);
    }

    public static GunItem ReadGun(this NetworkReader reader)
    {
        return (GunItem)Resources.Load(reader.ReadString());
    }
}
