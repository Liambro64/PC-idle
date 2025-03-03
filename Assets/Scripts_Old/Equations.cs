using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using System.Numerics;
public static class Equations
{
    public static BigInteger formattedCalculate(string equation, object[] dubs)
    {
        return calculate(formatWithNumbers(equation, dubs));
    }
    public static BigInteger calculate(string equation)
    {
        string poop = calculateSmall(formatEquation(equation));
        try
        {
            return BigInteger.Parse(poop);
        }
        catch
        {
            Debug.Log(equation + "turns into" + poop);
            return 0;
        }
    }
    public static string replaceArea(string original, string replacement, int start, int len)
    {
        return original.Remove(start, len)
                .Insert(start, replacement);
    }
    public static BigInteger GetResult(int index, char[] chars, ref string equation, Func<BigInteger, BigInteger, BigInteger> BigIntegerfn)
    {
        int j = 0;
        int m = 0;
        int l = 0;
        BigInteger num1 = 0;
        bool num1Neg = false;
        if (chars[index - 1] == '-')
        {
            num1Neg = true; j++;
        }
        while (index >= ++j && chars[index - j] > 47 && chars[index - j] < 58)
            num1 += (chars[index - j] - 48) * BigInteger.Pow(10, j - 1);
        if (index >= j && chars[index - j] == '.')
        {
            num1 /= BigInteger.Pow(10, j - 1);
            while (index >= j + ++m && chars[index - j - m] > 47 && chars[index - j - m] < 58)
                num1 += (chars[index - j - m] - 48) * BigInteger.Pow(10, m - 1);
        }
        else if (index >= j && (chars[index - j] == '+') && (chars[index - j - 1] == 'e' || chars[index - j - 1] == 'E'))
        {
            BigInteger ploop = num1;
            m++;
            num1 = 0;
            while (index >= j + ++m && chars[index - j - m] > 47 && chars[index - j - m] < 58)
                num1 += (chars[index - j - m] - 48) * BigInteger.Pow(10, j - 1);
            if (index >= j + m && chars[index - j - m] == '.')
            {
                num1 /= BigInteger.Pow(10, (int)BigInteger.Log10(num1));
                while (index >= j + m + ++l && chars[index - j - m - l] > 47 && chars[index - j - m - l] < 58)
                    num1 += (chars[index - j - m - l] - 48) * BigInteger.Pow(10, l - 1);
            }
            num1 *= BigInteger.Pow(10, (int)ploop);
        }
        else if (index >= j && (chars[index - j] == '-') && (chars[index - j - 1] == 'e' || chars[index - j - 1] == 'E'))
        {
            BigInteger ploop = num1;
            m++;
            num1 = 0;
            while (index >= j + ++m && chars[index - j - m] > 47 && chars[index - j - m] < 58)
                num1 += (chars[index - j - m] - 48) * BigInteger.Pow(10, j - 1);
            if (index >= j + m && chars[index - j - m] == '.')
            {
                num1 /= BigInteger.Pow(10, (int)BigInteger.Log10(num1));
                while (index >= j + m + ++l && chars[index - j - m - l] > 47 && chars[index - j - m - l] < 58)
                    num1 += (chars[index - j - m - l] - 48) * BigInteger.Pow(10, l - 1);
            }
            num1 /= BigInteger.Pow(10, (int)ploop);
        }
        int k = 0;
        int n = 0;
        BigInteger num2 = 0;
        bool num2Neg = false;
        if (chars[index + 1] == '-')
        {
            num2Neg = true;
            k++;
        }
        while (equation.Length > index + ++k && chars[index + k] > 47 && chars[index + k] < 58)
        {
            num2 = num2 * 10 + (chars[index + k] - 48);
        }
        if (equation.Length > index + k && chars[index + k] == '.')
        {
            while (equation.Length > index + k + ++n && chars[index + k + n] > 47 && chars[index + k + n] < 58)
            {
                num2 += (chars[index + k + n] - 48) / BigInteger.Pow(10, n);
            }
        }
        if (equation.Length > index + k + n && (chars[index + k + n] == 'E' || chars[index + k + n] == 'e') && chars[index + k + n + 1] == '+')
        {
            BigInteger e = 0;
            while (equation.Length > index + k + ++n && chars[index + k + n] > 47 && chars[index + k + n] < 58)
                e = e * 10 + (chars[index + k + n] - 48);
            num2 *= BigInteger.Pow(10, (int)e);
        }
        if (equation.Length > index + k + n && (chars[index + k + n] == 'E' || chars[index + k + n] == 'e') && chars[index + k + n + 1] == '-')
        {
            BigInteger e = 0;
            while (equation.Length > index + k + ++n && chars[index + k + n] > 47 && chars[index + k + n] < 58)
                e = e * 10 + (chars[index + k + n] - 48);
            num2 /= BigInteger.Pow(10, (int)e);
        }
        if (num1Neg)
            num1 = -num1;
        if (num2Neg)
            num2 = -num2;
        BigInteger result = BigIntegerfn(num1, num2);
        equation = replaceArea(equation, result.ToString(), index - (j - 1 + m + l), j + k + m + l + n - 1);
        return result;
    }
    public static BigInteger GetResult(int index, char[] chars, ref string equation, Func<BigInteger, int, BigInteger> BigIntegerfn)
    {
        int j = 0;
        int m = 0;
        int l = 0;
        BigInteger num1 = 0;
        bool num1Neg = false;
        if (chars[index - 1] == '-')
        {
            num1Neg = true; j++;
        }
        while (index >= ++j && chars[index - j] > 47 && chars[index - j] < 58)
            num1 += (chars[index - j] - 48) * BigInteger.Pow(10, j - 1);
        if (index >= j && chars[index - j] == '.')
        {
            num1 /= BigInteger.Pow(10, j - 1);
            while (index >= j + ++m && chars[index - j - m] > 47 && chars[index - j - m] < 58)
                num1 += (chars[index - j - m] - 48) * BigInteger.Pow(10, m - 1);
        }
        else if (index >= j && (chars[index - j] == '+') && (chars[index - j - 1] == 'e' || chars[index - j - 1] == 'E'))
        {
            BigInteger ploop = num1;
            m++;
            num1 = 0;
            while (index >= j + ++m && chars[index - j - m] > 47 && chars[index - j - m] < 58)
                num1 += (chars[index - j - m] - 48) * BigInteger.Pow(10, j - 1);
            if (index >= j + m && chars[index - j - m] == '.')
            {
                num1 /= BigInteger.Pow(10, (int)BigInteger.Log10(num1));
                while (index >= j + m + ++l && chars[index - j - m - l] > 47 && chars[index - j - m - l] < 58)
                    num1 += (chars[index - j - m - l] - 48) * BigInteger.Pow(10, l - 1);
            }
            num1 *= BigInteger.Pow(10, (int)ploop);
        }
        else if (index >= j && (chars[index - j] == '-') && (chars[index - j - 1] == 'e' || chars[index - j - 1] == 'E'))
        {
            BigInteger ploop = num1;
            m++;
            num1 = 0;
            while (index >= j + ++m && chars[index - j - m] > 47 && chars[index - j - m] < 58)
                num1 += (chars[index - j - m] - 48) * BigInteger.Pow(10, j - 1);
            if (index >= j + m && chars[index - j - m] == '.')
            {
                num1 /= BigInteger.Pow(10, (int)BigInteger.Log10(num1));
                while (index >= j + m + ++l && chars[index - j - m - l] > 47 && chars[index - j - m - l] < 58)
                    num1 += (chars[index - j - m - l] - 48) * BigInteger.Pow(10, l - 1);
            }
            num1 /= BigInteger.Pow(10, (int)ploop);
        }
        int k = 0;
        int n = 0;
        BigInteger num2 = 0;
        bool num2Neg = false;
        if (chars[index + 1] == '-')
        {
            num2Neg = true;
            k++;
        }
        while (equation.Length > index + ++k && chars[index + k] > 47 && chars[index + k] < 58)
        {
            num2 = num2 * 10 + (chars[index + k] - 48);
        }
        if (equation.Length > index + k && chars[index + k] == '.')
        {
            while (equation.Length > index + k + ++n && chars[index + k + n] > 47 && chars[index + k + n] < 58)
            {
                num2 += (chars[index + k + n] - 48) / BigInteger.Pow(10, n);
            }
        }
        if (equation.Length > index + k + n && (chars[index + k + n] == 'E' || chars[index + k + n] == 'e') && chars[index + k + n + 1] == '+')
        {
            BigInteger e = 0;
            while (equation.Length > index + k + ++n && chars[index + k + n] > 47 && chars[index + k + n] < 58)
                e = e * 10 + (chars[index + k + n] - 48);
            num2 *= BigInteger.Pow(10, (int)e);
        }
        if (equation.Length > index + k + n && (chars[index + k + n] == 'E' || chars[index + k + n] == 'e') && chars[index + k + n + 1] == '-')
        {
            BigInteger e = 0;
            while (equation.Length > index + k + ++n && chars[index + k + n] > 47 && chars[index + k + n] < 58)
                e = e * 10 + (chars[index + k + n] - 48);
            num2 /= BigInteger.Pow(10, (int)e);
        }
        if (num1Neg)
            num1 = -num1;
        if (num2Neg)
            num2 = -num2;
        BigInteger result = BigIntegerfn(num1, (int)num2);
        equation = replaceArea(equation, result.ToString(), index - (j - 1 + m + l), j + k + m + l + n - 1);
        return result;
    }
    public static string BE(string equation)
    {
        //equations as strings
        // + - * / obvious
        // ^ to the power of (a^b - a to the power of b)
        // V root (bVa - e.g. 2Vx = sqrt(x))
        // % = mod (a%b = the remainder of a and b)
        // () = brackets, how?

        char[] chars = equation.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '^')
            {
                GetResult(i, chars, ref equation, BigInteger.Pow);
                chars = equation.ToArray();
                i = 0;
            }
            else if (chars[i] == '%')
            {
                GetResult(i, chars, ref equation, (Func<BigInteger, BigInteger, BigInteger>)((x, y) => x % y));
                chars = equation.ToArray();
                i = 0;
            }
        }
        return equation;
    }
    public static string DM(string equation)
    {
        //equations as strings
        // + - * / obvious
        // ^ to the power of (a^b - a to the power of b)
        // V root (bVa - e.g. 2Vx = sqrt(x))
        // % = mod (a%b = the remainder of a and b)
        // () = brackets, how?

        char[] chars = equation.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '/')
            {
                GetResult(i, chars, ref equation, (Func<BigInteger, BigInteger, BigInteger>)((x, y) => x / y));
                chars = equation.ToArray();
                i = 0;
            }
            else if (chars[i] == '*')
            {
                GetResult(i, chars, ref equation, (Func<BigInteger, BigInteger, BigInteger>)((x, y) => x * y));
                chars = equation.ToArray();
                i = 0;
            }
        }
        return equation;
    }
    public static string AS(string equation)
    {
        //equations as strings
        // + - * / obvious
        // ^ to the power of (a^b - a to the power of b)
        // V root (bVa - e.g. 2Vx = sqrt(x))
        // % = mod (a%b = the remainder of a and b)
        // () = brackets, how?

        char[] chars = equation.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '+' && !(chars[i - 1] == 'E' || chars[i - 1] == 'e' || chars[i + 1] == 'E' || chars[i + 1] == 'e'))
            {
                GetResult(i, chars, ref equation, (Func<BigInteger, BigInteger, BigInteger>)((x, y) => x + y));
                chars = equation.ToArray();
                i = 0;
            }
            else if (chars[i] == '-' && !(chars[i - 1] == 'E' || chars[i - 1] == 'e' || chars[i + 1] == 'E' || chars[i + 1] == 'e'))
            {
                GetResult(i, chars, ref equation, (Func<BigInteger, BigInteger, BigInteger>)((x, y) => x - y));
                chars = equation.ToArray();
                i = 0;
            }
        }
        return equation;
    }
    public static string calculateSmall(string equation)
    {
        return AS(DM(BE(equation)));
    }
    public static bool hasOpeningBracket(string str)
    {
        return str.Contains('(');
    }
    public static string formatEquation(string equation)
    {
        char[] chars = equation.ToCharArray();
        //pt(equation);
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '(')
            {
                int j = 0;
                int l = 0;
                while (chars[i + ++j] != ')' || l != 0)
                {
                    if (chars[i + j] == '(') l++;
                    else if (chars[i + j] == ')') l--;
                }
                if (hasOpeningBracket(equation.Substring(i + 1, j - 1)))
                {
                    equation = replaceArea(equation, formatEquation(equation.Substring(i + 1, j - 1)), i + 1, j - 1);
                    chars = equation.ToCharArray();
                    i = -1;
                    continue;
                }
                equation = replaceArea(equation, calculateSmall(equation.Substring(i + 1, j - 1)), i, j + 1);
                chars = equation.ToCharArray();
                i = -1;
            }
        }
        return equation;
    }
    public static string formatWithNumbers(string equation, object[] numbers)
    {
        int count = equation.Count();
        List<string> parts = new List<string>();
        bool broke;
        while (count != 0)
        {
            broke = false;
            for (int i = 0; i < count; i++)
            {
                if (equation[i] == '~')
                {
                    int j = 0;
                    while (count > i + (++j) && char.IsDigit(equation[i + j])) ;
                    parts.Add(equation.Substring(0, i));
                    int index = int.Parse(equation.Substring(i + 1, j - 1));
                    equation = numbers.Length > index ? numbers[index] + equation.Substring(i + j) : throw new ArgumentException("yeah you didnt have the right amount of BigIntegers in the numbers variable", nameof(numbers));
                    count = equation.Count();
                    broke = true;
                    break;
                }
            }
            if (broke == false)
            {
                parts.Add(equation);
                break;
            }
        }
        string str = "";
        for (int i = 0; i < parts.Count; i++)
            str += parts[i];
        return str;
    }
}
