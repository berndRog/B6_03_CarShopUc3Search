using System.Text;
using System.Text.RegularExpressions;
namespace CarShop.Application {

   public static class SCheckInput {

      public static Result<string> IsNameOk(string name) {
         name = name.Trim();
         var result = IsTokenOk(name, "Name", min: 3, max: 64,
            minUpper: 1, maxUpper: 49, minLower: 1, maxLower: 49, minNumber: 0, maxNumber: 0,
            minWhiteSpace: 0, maxWhiteSpace: 5, minSpecial: 0, maxSpecial: 5); // ., !?-;:_@*"'%&/\(){[]}§$=+~°^€|<>`´#";
         if (result is Success<string>) return new Success<string>(name);
         else return new Error<string>($"Name {name} ist unzulässig: {result.Message}.");
      }

      public static Result<string> IsEmailOk(string email) {
         if(email.Length > 128)  return new Error<string>($"Email {email} ist zu lang.");
         if (Regex.IsMatch(email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            return new Success<string>(email);
         else return new Error<string>($"Email {email} ist unzulässig.");
      }

      public static Result<string> IsUserNameOk(string userName) {
         // Regex.IsMatch(name, @"^[a-z0-9_-]{4,25}$")
         userName = userName.Trim();
         var result = IsTokenOk(userName, "Nutzername", min: 3, max: 50,
            minUpper: 1, maxUpper: 49, minLower: 1, maxLower: 49, minNumber: 0, maxNumber: 5,
            minWhiteSpace: 0, maxWhiteSpace: 5, minSpecial: 0, maxSpecial: 5);
         if (result is Success<string>) return new Success<string>(userName);
         else return new Error<string>($"Nutzername {userName} ist unzulässig: {result.Message}.");
      }

      public static Result<string> IsPasswordOk(string pw) {
         // Regex.IsMatch(pw, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,25}$
         var result = IsTokenOk(
            pw,           "Password",
            minNumber: 1, minSpecial: 1);
         if (result is Success<string>) return new Success<string>(string.Empty);
         else return new Error<string>(string.Empty);
      }
      public static Result<string> ArePasswordsEqual(string pw1, string pw2) {
         if (pw1 != pw2) return new Error<string>("Passworte stimmen nicht überein");
         else return new Success<string>(string.Empty);
      }

      public static Result<string> IsStreetNrOk(string street) {
         street = street.Trim();
         var result = IsTokenOk(street, "Straße+Nr", min: 5, max: 64);
         if (result is Success<string>) return new Success<string>(street);
         else return new Error<string>($"Straße+Nr: {street} ist unzulässig: {result.Message}.");
      }

      public static Result<string> IsZipCityOk(string zipCode) {
         zipCode = zipCode.Trim();
         var result = IsTokenOk(zipCode, "Plz", min: 5, max: 64,
            minUpper: 0, maxUpper: 9, minLower: 0, maxLower: 19, minNumber: 1, maxNumber: 10,
            minWhiteSpace: 0, maxWhiteSpace: 5, minSpecial: 0, maxSpecial: 5);
         if (result is Success<string>) return new Success<string>(zipCode);
         else return new Error<string>($"Plz+Ort: {zipCode} ist unzulässig: {result.Message}.");
      }
            
      public static Result<string> IsTokenOk(
         string token,             string text,
         int    min           = 5, int    max           = 64,
         int    minLower      = 0, int    maxLower      = 64,
         int    minUpper      = 0, int    maxUpper      = 64,
         int    minNumber     = 0, int    maxNumber     = 32,
         int    minWhiteSpace = 0, int    maxWhiteSpace = 16,
         int    minSpecial    = 0, int    maxSpecial    = 8
      ) {
         if (token.Length < min && token.Length > max)
            return new Error<string>($"{text} muss >= {min} und <= {max} Zeichen lang sein!");

         var (upperCase, lowerCase, number, whiteSpace, punctuation, symbol, misc)
            = CategorizeToken(token);

         if (upperCase.Length < minUpper)
            return new Error<string>($"{text} muss mindestens {minUpper} Grossbuchstaben haben!");
         if (upperCase.Length > maxUpper)
            return new Error<string>($"{text} muss weniger als {maxUpper} Grossbuchstaben haben!");

         if (lowerCase.Length < minLower)
            return new Error<string>($"{text} muss mindestens {minLower} Kleinbuchstaben haben!");
         if (lowerCase.Length > maxLower)
            return new Error<string>($"{text} muss weniger als {maxLower} Kleinbuchstaben haben!");

         if (number.Length < minNumber)
            return new Error<string>($"{text} muss mindestens {minNumber} Zahl(en) haben!");
         if (number.Length > maxNumber)
            return new Error<string>($"{text} muss weniger als {maxNumber} Zahl(en) haben!");

         if (whiteSpace.Length < minWhiteSpace)
            return new Error<string>($"{text} muss mindestens {minWhiteSpace} Trennzeichen haben!");
         if (whiteSpace.Length > maxWhiteSpace)
            return new Error<string>($"{text} muss weniger als {maxWhiteSpace} Trennzeichen haben!");

         if (punctuation.Length + symbol.Length < minSpecial)
            return new Error<string>($"{text} muss mindestens {minSpecial} Sonderzeichen haben!");
         if (punctuation.Length + symbol.Length > maxSpecial)
            return new Error<string>($"{text} muss weniger als {maxSpecial} Sonderzeichen haben!");

         return new Success<string>(string.Empty);
      }

      public static (
         string upper,
         string lower,
         string number,
         string whitespace,
         string punctuation,
         string symbol,
         string misc
      )
         CategorizeToken(string token) {
     
         StringBuilder upperCase   = new(); // A B C ... X Y Z 
         StringBuilder lowerCase   = new(); // a b c ... x y z
         StringBuilder number      = new(); // 0 1 2 ... 7 8 9
         StringBuilder whiteSpace  = new(); // blank, \t \n \r ...
         StringBuilder punctuation = new(); // . , ! ? - ; : _ @ * " ' % & / \ ( )  { [ ] } 
         StringBuilder symbol      = new(); // § $ = + ~ ° ^ € | < > ` ´
         StringBuilder misc        = new();

         foreach (var c in token) {
            if (char.IsUpper(c)) upperCase.Append(c);
            else if (char.IsLower(c)) lowerCase.Append(c);
            else if (char.IsNumber(c)) number.Append(c);
            else if (char.IsWhiteSpace(c)) whiteSpace.Append(c);
            else if (char.IsPunctuation(c)) punctuation.Append(c);
            else if (char.IsSymbol(c)) symbol.Append(c);
            else misc.Append(c);
         }

         return (
            upperCase.ToString(),
            lowerCase.ToString(),
            number.ToString(),
            whiteSpace.ToString(),
            punctuation.ToString(),
            symbol.ToString(),
            misc.ToString()
         );
      }
   }
}