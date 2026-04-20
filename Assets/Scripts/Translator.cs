using UnityEngine;

public static class Translator
{
    public static string GenerateName(int min, int max)
    {
        string name = "";
        int n_am = Random.Range(min, max);
        for (int i = 0; i < n_am; i++)
        {
            char x = (char)Random.Range('A', 'S' - 3);
            if (x == 'K')
                x = 'Q';
            else if (x == 'L')
                x = 'R';
            else if (x == 'N')
                x = 'S';

                name += x;
        }

        return name;
    }

    public static string Number(int n)
    {
        string s = string.Empty;
        while (n > 0)
        {
            var tmp = n % 8;
            s = (tmp == 0 ? "0" : tmp.ToString()) + s;
            n /= 8;
        }
        return s;
    }

    public static string Resource(ResourcesPanel.resource res)
    {
        if (res == ResourcesPanel.resource.stone)
            return "EQAOJ";
        else if (res == ResourcesPanel.resource.crystal)
            return "SPIBIDI";
        else if (res == ResourcesPanel.resource.iron)
            return "HPGJSQ";
        else
            return "BGJCM";
    }
}