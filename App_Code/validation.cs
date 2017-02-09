using System;
using System.Text.RegularExpressions;

public class Validation
{
    public static bool isValidPhoneNumber(String str)
    {
        return new Regex(@"^05\d(-| )?\d{7}$", RegexOptions.None).Match(str).Success;
    }

    public static bool isValidPassword(String str)
    {
        return new Regex(@"^[a-zA-Z0-9 ]{8,24}$", RegexOptions.None).Match(str).Success;
    }

    public static bool isValidPlacesID(String str)
    {
        return new Regex(@"^[a-zA-Z0-9_-]{27}$", RegexOptions.None).Match(str).Success;
    }

    public static bool isValidDateString(String str)
    {
        return new Regex(@"^([1-9]|(0\d)|([12]\d)|(3[01]))\/(((0\d)|([1-9]))|(1[012]))(\/\d\d\d\d)?$", RegexOptions.None).Match(str).Success;
    }

    public static bool isValidTimeString(String str)
    {
        return new Regex(@"^((0)|([01]\d)|(2[0-3])):([0-5]\d)$", RegexOptions.None).Match(str).Success;
    }

    public static bool isValidComment(String str)
    {
        return new Regex(@"^[a-zA-Z0-9\u05D0-\u05EA \-\:\,\.\(\)\!\@\#\$\%\&\*]*$", RegexOptions.None).Match(str).Success;
    }
}