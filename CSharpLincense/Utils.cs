using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace CSharpLicense;

public static class ComputerInfo
{
    public static async Task<string> GetComputerInfoAsync()
    {
        return await Task.Run(() =>
        {
            string info = string.Empty;
            string cpu = GetCPUInfo();
            string baseBoard = GetBaseBoardInfo();
            string bios = GetBIOSInfo();
            string disk = GetDiskSerialNumber();
            info = string.Concat(bios, cpu, disk, baseBoard);
            return info;
        });
    }

    public static string GetHashString(string str)
    {
        var orginStr = string.Concat(str);
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
            string hashedMachineId = BitConverter.ToString(data).Replace("-", "").ToLower();
            return hashedMachineId;
        }
    }

    private static string GetCPUInfo()
    {
        string info = string.Empty;
        info = GetHardWareInfo("Win32_Processor", "ProcessorId");
        return info;
    }

    private static string GetBIOSInfo()
    {
        string info = string.Empty;
        info = GetHardWareInfo("Win32_BIOS", "SerialNumber");
        return info;
    }

    private static string GetBaseBoardInfo()
    {
        string info = string.Empty;
        info = GetHardWareInfo("Win32_BaseBoard", "SerialNumber");
        return info;
    }

    private static string GetDiskSerialNumber()
    {
        string info = string.Empty;
        info = GetHardWareInfo("Win32_DiskDrive", "Model");
        return info.Trim().Replace(" ","");
    }

    private static string GetHardWareInfo(string typePath, string key)
    {
        try
        {
            ManagementClass managementClass = new ManagementClass(typePath);
            ManagementObjectCollection mn = managementClass.GetInstances();
            PropertyDataCollection properties = managementClass.Properties;
            foreach (PropertyData property in properties)
            {
                if (property.Name == key)
                {
                    foreach (ManagementObject m in mn)
                    {
                        return m.Properties[property.Name].Value.ToString();
                    }
                }

            }
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
        return string.Empty;
    }
}
