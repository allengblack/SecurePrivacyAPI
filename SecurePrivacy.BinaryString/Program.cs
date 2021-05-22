using System;

bool BinStringIsGood(string binString)
{
    if (string.IsNullOrEmpty(binString)) return false;
    
    int count = 0;
    foreach (char c in binString)
    {
        if (c != '0' && c != '1')
        {
            return false;
        }

        if (c == '0') count--;
        if (c == '1') count++; 

        if (count < 0) return false;
    }

    return count == 0 ? true : false;
}

Console.WriteLine("Running Binary String Check...");

Console.WriteLine(BinStringIsGood(null));           //false
Console.WriteLine(BinStringIsGood(""));             //false
Console.WriteLine(BinStringIsGood("51010"));        //false
Console.WriteLine(BinStringIsGood("11010"));        //false
Console.WriteLine(BinStringIsGood("110010"));       //true
Console.WriteLine(BinStringIsGood("1100100011"));   //false
Console.WriteLine(BinStringIsGood("1100101010"));   //true 