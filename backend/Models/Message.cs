using System.Net.WebSockets;
using System.Text.RegularExpressions;
using ModelsUsers;

namespace Models.Messages;

public class Message
{
    public int user_id { get; set; } 
    public string message { get; set; } 
    private static readonly string pattern_bad_words = @"\b(bosta|merda|caralho|porra|puta|puto|fdp|filho da puta|babaca|idiota|imbecil|corno|vagabunda|vagabundo|desgraçado|desgraçada|cu|cuzão|cuzona|cacete|arrombado|arrombada|escroto|escrota|retardado|retardada|nojento|nojenta|piranha|viado|veado|boiola|baitola|canalha|safado|safada|trouxa|otário|otária|panaca)\b";
    private static readonly string pattern_injection = @"['""\\;%\-‐–—]|(--|\bOR\b\s+\d+=\d+|\bAND\b\s+\d+=\d+)";
    private static readonly string pattern_suspect =  @"xp_cmdshell|\bOR\b\s+\d+=\d+|\b(SELECT|UNION|INSERT|UPDATE|DELETE|DROP|ALTER|EXEC|CREATE|TRUNCATE|MERGE)\b|--|;|'|\*";

    public Message(int id, string message)
    {
        this.user_id = id;
        this.message = Sanitize(message);
    }

    private string Sanitize(string msg)
    {
        string sanitized_msg = msg;

        sanitized_msg = Regex.Replace(sanitized_msg, pattern_bad_words, "*");
        sanitized_msg = Regex.Replace(sanitized_msg, pattern_injection, String.Empty);
        sanitized_msg = Regex.Replace(sanitized_msg, pattern_suspect, String.Empty);

        if (sanitized_msg.Length > 250)
        {
            sanitized_msg = sanitized_msg.Substring(0, 250);
        }

        return sanitized_msg;
    }
}