using System.Collections;
using System.Collections.Generic;

public class ThemeNameComparison : IComparer
{
    int IComparer.Compare(object x, object y)
    {
        string a = (string) x; 
        string b = (string) y;

        if (int.Parse(a) < int.Parse(b))
            return -1;
        if (int.Parse(a) > int.Parse(b))
            return 1;
        else
            return 0;

    }

    public static IComparer sortByThemeName()
    {
        return (IComparer) new ThemeNameComparison();
    }
}
